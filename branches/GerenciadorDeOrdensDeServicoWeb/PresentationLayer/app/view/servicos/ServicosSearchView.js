
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
                title: 'Consultar Serviços',
                width: 500,
                height: 380,
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
        servicosStore.load({ params: { apenasDadosBasicos: true} });
        this.servicosStore = servicosStore;

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-servicos',
            store: servicosStore,
            cls: 'grid-style-1',
            columns: [
                { xtype: 'numbercolumn', header: 'Cod', dataIndex: 'codigo', format: '0' },
                { header: 'nome', dataIndex: 'nome', flex: 1, renderer: Ext.String.htmlEncode },
                { header: 'Cobrado por', dataIndex: 'nomeCobradoPor' }
            ],
            tbar: [
                { itemId: 'btnAddServico', text: 'Adicionar', iconCls: 'servicos-add-thumb', scope: this },
                { itemId: 'btnEditServico', text: 'Editar', iconCls: 'servicos-edit-thumb', disabled: true, scope: this },
                { itemId: 'btnDelServico', text: 'Remover', iconCls: 'servicos-del-thumb', disabled: true, scope: this },
                { itemId: 'btnShowDescricaoServico', text: 'Descri&ccedil;&atilde;o', iconCls: 'btn-detalhes', pressed: false, enableToggle: true, scope: this,tooltip: { title: 'Descri&ccedil;&atilde;o dos servi&ccedil;os', text: 'Visualizar a descri&ccedil;&atilde;o de cada registro na listagem'} }
            ],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: servicosStore,
                displayInfo: true,
                displayMsg: 'Servi&ccedil;os {0} - {1} de {2}',
                emptyMsg: "Nenhum servi&ccedil;o"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditServico').setDisabled(!records.length);
                    grid.down('#btnDelServico').setDisabled(!records.length);
                }
            },
            viewConfig: {
                itemId: 'viewServicos',
                plugins: [{
                    pluginId: 'previewServicos',
                    ptype: 'preview',
                    bodyField: 'descricao',
                    previewExpanded: false,
                    labelField: '<b>Descri&ccedil;&atilde;o:</b> '
                }]
            }
        });
        this.gridServicos = grid;
        this.gridServicos.module = this;

        return grid;
    }
});
