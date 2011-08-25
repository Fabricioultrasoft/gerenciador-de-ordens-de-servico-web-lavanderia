
Ext.define('App.view.servicos.ServicosSearchView', {
    extend: 'App.webDesktop.Module',
    requires: ['App.ux.PreviewPlugin','Ext.String'],
    id: 'module-servicos-search',
    
    createWindow: function () {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-servicos-search');
        if (!win) {
            var servicosPanel = this.createPanel();
            win = desktop.createWindow({
                id: 'win-servicos-search',
                title: 'Consultar Servi&ccedil;os',
                width: 700,
                height: 580,
                iconCls: 'servicos-search-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [servicosPanel]
            });
        }
        win.show();
        return win;
    },

    createPanel: function () {

        var servicosStore = Ext.create('App.store.servicos.ServicosStore',{});
        //servicosStore.load();

        var grid = Ext.create('Ext.tree.Panel', {
            id: 'grid-servicos',
            border: false,
            useArrows: true,
            rootVisible: false,
            store: servicosStore,
            columns: [
                { xtype: 'treecolumn', header: 'nome', dataIndex: 'nome' },
                { xtype: 'numbercolumn', header: 'Cod', dataIndex: 'codigo', format: '0' },
                { header: 'Tipo', dataIndex: 'nomeTipoServico' }
            ],
            tbar: [
                { itemId: 'btnAddServico', text: 'Adicionar', iconCls: 'btn-add', scope: this },
                { itemId: 'btnEditServico', text: 'Editar', iconCls: 'btn-del', disabled: true, scope: this },
                { itemId: 'btnDelServico', text: 'Remover', iconCls: 'btn-del', disabled: true, scope: this }
            ],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: servicosStore,
                displayInfo: true,
                displayMsg: 'Servi&ccedil;os {0} - {1} of {2}',
                emptyMsg: "Nenhum servi&ccedil;o"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditServico').setDisabled(!records.length);
                    grid.down('#btnDelServico').setDisabled(!records.length);
                }
            }
        });
        this.gridServicos = grid;
        this.gridServicos.module = this;

        return grid;
    }
});
