
Ext.define('App.view.relatorios.RelatoriosView', {
    extend: 'App.webDesktop.Module',
    id: 'module-report',
    init: function () {

        this.launcher = {
            text: 'Relat&oacute;rios',
            iconCls: 'report',
            handler: function () {
                return false;
            },
            menu: {
                items: []
            }
        };

        // relatorios
        this.launcher.menu.items.push({
            text: 'Clientes',
            iconCls: 'report',
            handler : function() {
                this.app.getModule("module-report-clientes").createWindow();
            },
            scope: this
        },
        {
            text: 'Ordens de Servi&ccedil;o',
            iconCls: 'report',
            handler : function() {
                this.app.getModule("module-report-ordensDeServico").createWindow();
            },
            scope: this
        });
    }
});