
Ext.define('App.view.usuarios.UsuariosView', {
    extend: 'App.webDesktop.Module',
    id: 'module-usuarios',
    init: function () {

        this.launcher = {
            text: 'Usuarios',
            iconCls: 'user-silhouette',
            handler: function () {
                return false;
            },
            menu: {
                items: []
            }
        };

        this.launcher.menu.items.push({
            text: 'Adicionar Usuario',
            iconCls: 'user-add',
            handler : function() {
                this.app.getModule("module-usuarios-add").createWindow();
            },
            scope: this
        },
        {
            text: 'Consultar Usuarios',
            iconCls: 'user-search',
            handler : function() {
                this.app.getModule("module-usuarios-search").createWindow();
            },
            scope: this
        });
    }
});