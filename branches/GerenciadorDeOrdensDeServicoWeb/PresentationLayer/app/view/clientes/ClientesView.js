
Ext.define('App.view.clientes.ClientesView', {
    extend: 'App.webDesktop.Module',
    id: 'win-clientes',
    init: function () {

        this.launcher = {
            text: 'Clientes',
            iconCls: 'clientes-thumb',
            handler: function(){return false;},
            menu: {
                items: []
            },
            scope: this
        };

        // menus
        this.launcher.menu.items.push({
            text: 'Adicionar Cliente',
            iconCls: 'clientes-add-thumb',
            handler : function() {
                this.app.getModule("module-clientes-add").createWindow();
            },
            scope: this,
            id: 'mnuClientesAdd'
        },
        {
            text: 'Consultar Clientes',
            iconCls: 'clientes-search-thumb',
            handler : function() {
                this.app.getModule("module-clientes-search").createWindow();
            },
            scope: this,
            id: 'mnuClientesSearch'
        });
    }
});