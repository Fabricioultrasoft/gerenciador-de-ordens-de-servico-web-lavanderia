
Ext.define('App.view.ordensDeServico.OrdensDeServicoClienteSearchView', {
    extend: 'App.webDesktop.Module',
    id: 'module-ordensDeServico-clientesSearch',
    init: function () { },

    createWindow: function (moduleTarget) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-ordensDeServico-clientesSearch');
        if (!win) {
            var panel = this.createPanel({moduleTarget: moduleTarget});
            win = desktop.createWindow({
                id: 'win-ordensDeServico-clientesSearch',
                title: 'Buscar Cliente',
                width: 500,
                height: 420,
                modal: true,
                minimizable: false,
                iconCls: 'clientes-search-thumb',
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

    createPanel: function (options) {
        this.options = options;

        var clientesStore = Ext.create('App.store.clientes.ClientesStore', { remoteFilter:true, remoteSort: true});
        this.clientesStore = clientesStore;
        this.clientesStore.module = this;
        
        var form = Ext.create('Ext.form.Panel', {
            border: false,
            region: 'north',
            bodyPadding: 5,
            height: 90,
            fieldDefaults: {
                labelAlign: 'left',
                labelWidth: 80,
                anchor: '100%'
            },
            items: [
                { xtype: 'numberfield', name: 'codigo', fieldLabel: 'Codigo', emptyText: '0000', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                { xtype: 'textfield', name: 'nome', fieldLabel: 'Nome', emptyText: 'Digite o nome do cliente', maxLength: 50}
            ],
            buttonAlign: 'center',
            buttons: [{
                itemId: 'btnSearchClienteOS',
                text: 'Filtrar Clientes', 
                iconCls: 'filtro',
                scope: this
            }]
        });
        this.form = form;

        var grid = Ext.create('Ext.grid.Panel',{
            id: 'grid-clientesOS',
            border: false,
            region: 'center',
            store: clientesStore,
            cls: 'grid-style-1',
            style: { borderTop: '1px solid #99BCE8', borderBottom: '1px solid #99BCE8' },
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
            bbar: Ext.create('Ext.PagingToolbar', {
                store: clientesStore,
                displayInfo: true,
                displayMsg: 'clientes {0} - {1} de {2}',
                emptyMsg: "Nenhum cliente"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    mainPanel.down('#btnConfirmAddClienteOS').setDisabled(!records.length);
                }
            }
        });
        this.grid = grid;
        this.grid.module = this;

        var msg = Ext.create('Ext.panel.Panel', { 
            border: false,
            height: 40,
            bodyPadding: 4,
            region: 'south',
            html:'Ap&oacute;s filtrar os clientes, clique sobre o registro correspondente ao cliente para selecion&aacute;-lo e em seguida, clique no bot&atilde;o abaixo para inclu&iacute;-lo na Ordem de Servi&ccedil;o.'
        });

        //--------------------------------------------------------------------
        var mainPanel = Ext.create('Ext.panel.Panel', {
            layout: 'border',
            items: [form,grid, msg],
            buttonAlign: 'center',
            buttons: [{ disabled: true, text: 'Adicionar Cliente &agrave; Ordem de Servi&ccedil;o', itemId: 'btnConfirmAddClienteOS', iconCls: 'clientes-add-thumb', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    }
});
