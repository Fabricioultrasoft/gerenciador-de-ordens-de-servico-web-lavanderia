
Ext.define('App.view.ordensDeServico.OrdensDeServicoSearchView', {
    extend: 'App.webDesktop.Module',
    requires: ['App.ux.PreviewPlugin'],
    id: 'module-ordensDeServico-search',
    init: function () {
    },

    createWindow: function () {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-ordensDeServico-search');
        if (!win) {
            var panel = this.createPanel();
            win = desktop.createWindow({
                id: 'win-ordensDeServico-search',
                title: 'Consultar Ordens de Serviço',
                width: 700,
                height: 580,
                iconCls: 'os-search',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [panel]
            });
        }
        win.show();

        return win;
    },

    createViewOSWindow: function ( ordemDeServico ) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-viewOS-' + ordemDeServico.numero);
        if (!win) {
            win = desktop.createWindow({
                id: 'win-viewOS-' + ordemDeServico.numero,
                title: 'Ordens de Serviço - Numero: <span style="color:red;">' + ordemDeServico.numero + '</span>',
                width: 600,
                height: 400,
                iconCls: 'os-thumb',
                animCollapse: false,
                constrainHeader: true,
                autoScroll: true,
                loader: {
                    autoLoad: true,
                    url: '/PresentationLayer/app/view/ordensDeServico/OrdemDeServicoViewTpl.aspx',
                    loadMask: true,
                    params: { numero: ordemDeServico.numero }
                }
            });
        }
        win.show();

        return win;
    },

    createPanel: function () {
        
        var ordensDeServicoStore = Ext.create('App.store.ordensDeServico.OrdensDeServicoStore', { remoteFilter:true, remoteSort: true});
        this.ordensDeServicoStore = ordensDeServicoStore;
        this.ordensDeServicoStore.module = this;
        ordensDeServicoStore.load({ params: { codigoStatus: 1} });

        var statusStore = Ext.create('App.store.ordensDeServico.StatusStore', { pageSize: 0 });
        statusStore.load();

        var form = Ext.create('Ext.form.Panel', {
            title: 'Filtros',
            border: false,
            collapsible: true,
            animCollapse: true,
            autoScroll: true,
            region: 'north',
            height: 210,
            iconCls: 'filtro',
            bodyPadding: 5,
            layout: 'anchor',
            defaults: {
                anchor: '100%',
                labelWidth: 50
            },
            items: [
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top', margins: '0 4 0 0' },
                    items: [
                        { xtype: 'numberfield', name: 'numero', flex : 1, fieldLabel: 'Numero', emptyText: 'Numero da OS', minValue: 0, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'combobox', name: 'codigoStatus', flex : 1, fieldLabel: 'Status', store: statusStore, queryMode: 'local', valueField: 'codigo', displayField: 'nome', emptyText: 'Selecione o status', selectOnFocus: true, forceSelection: true },
                        { xtype: 'numberfield', flex : 1, name: 'valorOriginal', fieldLabel: 'Valor Original', emptyText: 'R$ 0.00', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'numberfield', flex : 1, name: 'valorFinal', fieldLabel: 'Valor Final/Com Desconto', emptyText: 'R$ 0.00', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, margins: '0' }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'datefield', flex : 1, name: 'dataDeAbertura', fieldLabel: 'Abertura', emptyText: 'Data de Abertura', format: 'd/m/Y' },
                        { xtype: 'datefield', flex : 1, name: 'previsaoDeConclusao', fieldLabel: 'Prev. Conclus&atilde;o', emptyText: 'Previsão de Conclusão', format: 'd/m/Y', margins: '0 4' },
                        { xtype: 'datefield', flex : 1, name: 'dataDeFechamento', fieldLabel: 'Fechamento', emptyText: 'Fechamento', format: 'd/m/Y' }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: 'Cliente', layout: 'hbox', defaults: { hideLabel: true },
                    items: [
                        { xtype: 'numberfield', itemId:'codigoClienteSearchOS', width: 60, name: 'codigoCliente', emptyText: 'Codigo', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'textfield', itemId:'nomeClienteSearchOS', flex : 1, name : 'nomeCliente', emptyText: 'Nome do Cliente', margins: '0 4' },
                        { xtype: 'button', text: 'Add', itemId: 'btnClienteSearchOS', iconCls: 'clientes-add-thumb', scope: this }
                    ]
                }
            ],
            buttonAlign: 'center',
            buttons: [{
                itemId: 'btnSearchOS',
                text: 'Filtrar', 
                iconCls: 'filtro',
                scope: this
            }, {
                itemId: 'btnLimparFiltrosOS',
                text: 'Limpar',
                iconCls: 'btn-limpar',
                scope: this
            }]
        });
        this.form = form;

        var grid = Ext.create('Ext.grid.Panel',{
            id: 'grid-ordensDeServico',
            border: false,
            region: 'center',
            store: ordensDeServicoStore,
            cls: 'grid-style-1',
            columns: [
                { header: 'Cod', dataIndex: 'codigo', xtype: 'numbercolumn', format: '0', hidden: true},
                { header: 'Numero', dataIndex: 'numero', xtype: 'numbercolumn', format: '0'},
                { header: 'Cliente', dataIndex: 'nomeCliente', minWidth: 200, flex: 1, renderer: Ext.String.htmlEncode },
                { header: 'Val. Orig.', dataIndex: 'valorOriginal', xtype: 'numbercolumn', renderer: Ext.util.Format.brMoney },
                { header: 'Val. Final', dataIndex: 'valorFinal', xtype: 'numbercolumn', renderer: Ext.util.Format.brMoney },
                { header: 'Status', dataIndex: 'nomeStatus', renderer: Ext.String.htmlEncode },  
                { header: 'Abertura', dataIndex: 'dataDeAbertura' }, 
                { header: 'Prev. Conclus&atilde;o', dataIndex: 'previsaoDeConclusao' }, 
                { header: 'Fechamento', dataIndex: 'dataDeFechamento' }
            ],
            tbar: [
                { itemId: 'btnAddOS', text: 'Adicionar', iconCls: 'os-add', scope: this },
                { itemId: 'btnEditOS', text: 'Editar', iconCls: 'os-thumb', scope: this, disabled: true },
                { itemId: 'btnDelOS', text: 'Remover', iconCls: 'os-del', scope: this, disabled: true },
                { itemId: 'btnShowDescricaoOS', iconCls: 'btn-detalhes', scope: this, pressed: false, enableToggle: true, text: 'Observa&ccedil;&otilde;es', tooltip: { title: 'Observa&ccedil;&otilde;es das Ordens de Servi&ccedil;o', text: 'Visualizar a descri&ccedil;&atilde;o de cada registro na listagem' } },
                '->',
                { itemId: 'btnViewOS', text: 'Visualizar', iconCls: 'lupa', scope: this, disabled: true },
                { itemId: 'btnFinalizarOS', text: 'Finalizar', iconCls: 'concluido', scope: this, disabled: true },
                { itemId: 'btnCancelarOS', text: 'Cancelar', iconCls: 'cancel', scope: this, disabled: true }
            ],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: ordensDeServicoStore,
                displayInfo: true,
                displayMsg: 'Ordens de Servi&ccedil;o {0} - {1} de {2}',
                emptyMsg: "Nenhuma Ordem de Servi&ccedil;o"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditOS').setDisabled(!records.length);
                    grid.down('#btnDelOS').setDisabled(!records.length);
                    grid.down('#btnViewOS').setDisabled(!records.length);
                    grid.down('#btnFinalizarOS').setDisabled(!records.length);
                    grid.down('#btnCancelarOS').setDisabled(!records.length);
                }
            },
            viewConfig: { itemId: 'view', plugins: [{ pluginId: 'preview', ptype: 'preview', bodyField: 'observacoes', previewExpanded: false, labelField: '<b>Observa&ccedil;&otilde;es:</b> ' }] } 
        });
        this.grid = grid;
        this.grid.module = this;

        //--------------------------------------------------------------------
        var mainPanel = Ext.create('Ext.panel.Panel', {
            layout: 'border',
            items: [form,grid]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    },

    setCliente: function(cliente) {
        this.form.down('#codigoClienteSearchOS').setValue(cliente.codigo);
        this.form.down('#nomeClienteSearchOS').setValue(cliente.nome);
    }
});
