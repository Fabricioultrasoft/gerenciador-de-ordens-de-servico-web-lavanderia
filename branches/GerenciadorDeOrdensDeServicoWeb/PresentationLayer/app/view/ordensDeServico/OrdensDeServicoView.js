
Ext.define('App.view.ordensDeServico.OrdensDeServicoView', {
    extend: 'App.webDesktop.Module',
    id: 'module-ordensDeServico',
    init: function () {

        this.launcher = {
            text: 'Ordens de Servi&ccedilo',
            iconCls: 'os-thumb',
            handler: function(){return false;},
            menu: {
                items: []
            },
            scope: this
        };

        // menus
        this.launcher.menu.items.push({
            text: 'Adicionar Ordem de Serviço',
            iconCls: 'os-add',
            handler : function() {
                this.app.getModule("module-ordensDeServico-add").createWindow();
            },
            scope: this
        },
        {
            text: 'Consultar Ordens de Serviço',
            iconCls: 'os-search',
            handler : function() {
                this.app.getModule("module-ordensDeServico-search").createWindow();
            },
            scope: this
        });
    }
});