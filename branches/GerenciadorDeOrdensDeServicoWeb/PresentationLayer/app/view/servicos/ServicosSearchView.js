
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
