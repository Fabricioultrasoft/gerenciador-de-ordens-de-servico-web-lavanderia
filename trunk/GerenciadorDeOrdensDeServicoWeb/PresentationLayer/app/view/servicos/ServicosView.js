
Ext.define('App.view.servicos.ServicosView', {
    extend: 'App.webDesktop.Module',
    id: 'module-servicos',
    init: function () {

        this.launcher = {
            text: 'Servi&ccedil;os',
            iconCls: 'servicos-thumb',
            handler: function(){return false;},
            menu: {
                items: []
            },
            scope: this
        };

        // menus
        this.launcher.menu.items.push({
            text: 'Adicionar Servi&ccedil;o',
            iconCls: 'servicos-add-thumb',
            handler : function() {
                this.app.getModule("module-servicos-add").createWindow();
            },
            scope: this,
            id: 'mnuServicosAdd'
        },
        {
            text: 'Consultar Servi&ccedil;os',
            iconCls: 'servicos-search-thumb',
            handler : function() {
                this.app.getModule("module-servicos-search").createWindow();
            },
            scope: this,
            id: 'mnuServicosSearch'
        });
    }
});