
Ext.define('App.view.clientes.ClientesSearchView', {
    extend: 'App.webDesktop.Module',
    requires: ['App.ux.PreviewPlugin','Ext.String'],
    id: 'module-clientes-search',
    init: function () {
//        this.launcher = {
//            text: 'Consultar Clientes',
//            iconCls: 'clientes-search-thumb',
//            handler: this.createWindow,
//            scope: this
//        };
    },

    createWindow: function () {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-clientes-search');
        if (!win) {
            var clientesPanel = this.createPanel();
            win = desktop.createWindow({
                id: 'win-clientes-search',
                title: 'Consultar Clientes',
                width: 700,
                height: 580,
                iconCls: 'clientes-search-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [clientesPanel]
            });
        }
        win.show();

        return win;
    },

    createPanel: function () {

        var tiposDeClientesStore = Ext.create('App.store.clientes.TiposDeClientesStore', { pageSize: 0 });
        tiposDeClientesStore.load({ params: { ativo: true} });

        var clientesStore = Ext.create('App.store.clientes.ClientesStore', { remoteFilter:true, remoteSort: true});
        this.clientesStore = clientesStore;
        this.clientesStore.module = this;

        clientesStore.load({ params: { ativo: true} });
        
        //--------------------------------------------------------------------
        var formFiltros = Ext.create('Ext.form.Panel', {
            title: 'Filtros',
            border: false,
            collapsible: true,
            animCollapse: true,
            autoScroll: true,
            region: 'north',
            height: 235,
            iconCls: 'filtro',
            bodyPadding: 5,
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'checkbox', name: 'ativo', fieldLabel: 'Ativo', width: 50, inputValue: 1, checked: true },
                        { xtype: 'numberfield', name: 'codigo', minValue: 0, flex: 1, fieldLabel: 'Codigo', emptyText: '0000', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, margins: '0 4' },
                        { xtype: 'combobox', name: 'codigoTipoDeCliente', flex: 1, fieldLabel: 'Tipo de cliente', store: tiposDeClientesStore, queryMode: 'local', valueField: 'codigo', displayField: 'nome', emptyText: 'Selecione o tipo de cliente', selectOnFocus: true, forceSelection: true }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'textfield', name: 'nome', flex: 1, fieldLabel: 'Nome', emptyText: 'Digite o nome do cliente', maxLength: 50, margins: '0 4 0 0'},
                        { xtype: 'textfield', name: 'conjuge', flex: 1, fieldLabel: 'Conjuge', emptyText: 'Digite o nome do cônjuge', maxLength: 50 }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top', margins: '0 4 0 0' },
                    items: [
                        { xtype: 'radiogroup', fieldLabel: 'Sexo', width: 200, items: [{ xtype: 'radio', boxLabel: 'Masculino', inputValue: 1, name: 'sexo', checked: true }, { xtype: 'radio', boxLabel: 'Feminino', inputValue: 2, name: 'sexo'}] },
                        { xtype: 'datefield', name: 'dataDeNascimento', flex: 1, fieldLabel: 'Nascimento', emptyText: 'dd/mm/aaaa', format: 'd/m/Y' },
                        { xtype: 'textfield', name: 'rg', flex: 1, fieldLabel: 'RG', emptyText: 'Digite o RG', maxLength: 12 },
                        { xtype: 'textfield', name: 'cpf', flex: 1, fieldLabel: 'CPF', emptyText: 'Digite o CPF', maxLength: 14, margins: '0' }
                    ]
                }
            ],
            buttonAlign: 'center',
            buttons: [{
                itemId: 'btnSearchCliente',
                text: 'Procurar', 
                iconCls: 'search',
                scope: this
            }, {
                itemId: 'btnLimparFiltros',
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
            //style: { borderTop: '1px solid #99BCE8' },
            columns: [
                { header: 'Cod', dataIndex: 'codigo', xtype: 'numbercolumn', format: '0'}, 
                { header: 'Nome', dataIndex: 'nome', minWidth: 200, flex: 1, renderer: Ext.String.htmlEncode }, 
                { header: 'Tipo', dataIndex: 'nomeTipoDeCliente', renderer: Ext.String.htmlEncode }, 
                { header: 'Sexo', dataIndex: 'strSexo'}, 
                { header: 'Nascimento', dataIndex: 'dataDeNascimento'},
                { header: 'RG', dataIndex: 'rg'}, 
                { header: 'CPF', dataIndex: 'cpf'}, 
                { header: 'Ativo', dataIndex: 'ativo', xtype: 'booleancolumn'},
                { header: 'Cadastro', dataIndex: 'dataDeCadastro'},
                { header: 'Atualiza&ccedil;&atilde;o', dataIndex: 'dataDeAtualizacao'}
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
                    text: 'Visualizar a descri&ccedil;&atilde;o de cada registro na listagem'
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
            layout: 'border',
            items: [formFiltros,gridClientes]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    }
});
