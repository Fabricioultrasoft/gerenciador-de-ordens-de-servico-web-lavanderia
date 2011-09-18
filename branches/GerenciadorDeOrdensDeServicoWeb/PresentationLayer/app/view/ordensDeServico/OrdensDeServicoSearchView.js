
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
                maximized: true,
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

    createPanel: function () {
        
        var formFiltros = Ext.create('Ext.form.Panel', {
            title: 'Filtros',
            collapsible: true,
            animCollapse: true,
            autoScroll: true,
            region: 'north',
            height: 260,
            iconCls: 'filtro',
            bodyPadding: 5,
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [
                { xtype: 'numberfield', name: 'codigo', fieldLabel: 'Codigo', emptyText: '0000', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                { xtype: 'checkbox', name: 'ativo', fieldLabel: 'Ativo', inputValue: 1},
                { xtype: 'textfield', name: 'nome', fieldLabel: 'Nome', emptyText: 'Digite o nome do cliente', maxLength: 50},
                { xtype: 'textfield', name: 'conjuge', fieldLabel: 'Conjuge', emptyText: 'Digite o nome do cônjuge', maxLength: 50 },
                { xtype: 'combobox', name: 'codigoTipoDeCliente', fieldLabel: 'Tipo de cliente', store: tiposDeClientesStore, queryMode: 'local', valueField: 'codigo', displayField: 'nome', emptyText: 'Selecione o tipo de cliente', selectOnFocus: true, forceSelection: true },
                { xtype: 'datefield', name: 'dataDeNascimento', fieldLabel: 'Nascimento', emptyText: 'dd/mm/aaaa', format: 'd/m/Y' },
                { xtype: 'radiogroup', fieldLabel: 'Sexo', items: [{ xtype: 'radio', boxLabel: 'Masculino', inputValue: 1, name: 'sexo', checked: true }, { xtype: 'radio', boxLabel: 'Feminino', inputValue: 2, name: 'sexo'}] },
                { xtype: 'textfield', name: 'rg', fieldLabel: 'RG', emptyText: 'Digite o RG', maxLength: 12 },
                { xtype: 'textfield', name: 'cpf', fieldLabel: 'CPF', emptyText: 'Digite o CPF', maxLength: 14 }
            ],
            buttonAlign: 'center',
            buttons: [{
                itemId: 'btnSearchOrdensDeServico',
                text: 'Procurar', 
                iconCls: 'lupa',
                scope: this
            }, {
                itemId: 'btnLimparFiltrosOS',
                text: 'Limpar',
                iconCls: 'btn-limpar',
                scope: this
            }]
        });
        this.formFiltros = formFiltros;

        var gridClientes = Ext.create('Ext.grid.Panel',{
            id: 'grid-clientes',
            border: false,
            region: 'center',
            store: clientesStore,
            cls: 'grid-style-1',
            columns: [
                { header: 'Cod', dataIndex: 'codigo', xtype: 'numbercolumn', format: '0', hidden: true},
                { header: 'N&ordm;', dataIndex: 'numero', xtype: 'numbercolumn', format: '0'}, 
                { header: 'Cliente', dataIndex: 'nomeCliente', minWidth: 200, flex: 1, renderer: Ext.String.htmlEncode }, 
                { header: 'Tipo', dataIndex: 'nomeTipoDeCliente', renderer: Ext.String.htmlEncode }
            ],
            tbar: [
            { itemId: 'btnAddCliente', text: 'Adicionar', iconCls: 'clientes-add-thumb', scope: this, handler : function() { this.app.getModule("module-clientes-add").createWindow({store:clientesStore}); } },
            { itemId: 'btnEditCliente', text: 'Editar', iconCls: 'clientes-edit-thumb', scope: this, disabled: true },
            { itemId: 'btnDelCliente', text: 'Remover', iconCls: 'clientes-del-thumb', scope: this, disabled: true }, 
            {
                itemId: 'btnShowDescricaoCliente',
                iconCls: 'btn-detalhes',
                scope: this,
                pressed: false,
                enableToggle: true,
                text: 'Observa&ccedil;&otilde;es',
                tooltip: {
                    title: 'Observa&ccedil;&otilde;es dos clientes',
                    text: 'Visializar a descri&ccedil;&atilde;o de cada registro na listagem'
                }
            }],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: clientesStore,
                displayInfo: true,
                displayMsg: 'clientes {0} - {1} de {2}',
                emptyMsg: "Nenhum cliente"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    gridClientes.down('#btnEditCliente').setDisabled(!records.length);
                    gridClientes.down('#btnDelCliente').setDisabled(!records.length);
                }
            },
            viewConfig: {
                itemId: 'view',
                plugins: [{
                    pluginId: 'preview',
                    ptype: 'preview',
                    bodyField: 'observacoes',
                    previewExpanded: false,
                    labelField: '<b>Observa&ccedil;&otilde;es:</b> '
                }]
            }
        });
        this.gridClientes = gridClientes;
        this.gridClientes.module = this;

        //--------------------------------------------------------------------
        var mainPanel = Ext.create('Ext.panel.Panel', {
            border: false,
            layout: 'border',
            items: [formFiltros,gridClientes]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    }
});
