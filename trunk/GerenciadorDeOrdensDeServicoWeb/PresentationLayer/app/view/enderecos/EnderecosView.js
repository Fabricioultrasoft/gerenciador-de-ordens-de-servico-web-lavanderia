
Ext.define('App.view.enderecos.EnderecosView', {
    extend: 'App.webDesktop.Module',
    id: 'module-enderecos',
    init: function () {

        this.launcher = {
            text: 'Enderecos',
            iconCls: 'location-thumb',
            handler: function () {
                return false;
            },
            menu: {
                items: []
            }
        };

        // enderecos
        this.launcher.menu.items.push({
            text: 'Tipos de logradouros',
            iconCls: 'location-thumb',
            handler : this.createTiposDeLogradourosWindow,
            scope: this,
            id: 'mnuEnderecosTiposLogradouros'
        },
        {
            text: 'Logradouros',
            iconCls: 'location-thumb',
            handler : function() {
                this.app.getModule("module-logradouros").createWindow();
            },
            scope: this,
            id: 'mnuEnderecosLogradouros'
        },
        {
            text: 'Bairros',
            iconCls: 'location-thumb',
            handler : this.createBairrosWindow,
            scope: this,
            id: 'mnuEnderecosBairros'
        },
        {
            text: 'Cidades',
            iconCls: 'location-thumb',
            handler : this.createCidadesWindow,
            scope: this,
            id: 'mnuEnderecosCidades'
        },
        {
            text: 'Estados',
            iconCls: 'location-thumb',
            handler : this.createEstadosWindow,
            scope: this,
            id: 'mnuEnderecosEstados'
        },
        {
            text: 'Países',
            iconCls: 'location-thumb',
            handler: this.createPaisesWindow,
            scope: this,
            id: 'mnuEnderecosPaises'
        });
    },

    createTiposDeLogradourosWindow: function () {
        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-tipos-de-logradouros');
        if (!win) {
            var moduleTiposDeLogradouros = Ext.create('App.view.enderecos.TiposDeLogradourosView',{});
            win = desktop.createWindow({
                id: 'win-tipos-de-logradouros',
                title: 'Tipos de logradouros',
                width: 550,
                height: 280,
                iconCls: 'location-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                items: [moduleTiposDeLogradouros]
            });
        }
        win.show();
        return win;
    },

    createBairrosWindow: function () {
        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-bairros');
        if (!win) {
            var moduleBairros = Ext.create('App.view.enderecos.BairrosView',{});
            win = desktop.createWindow({
                id: 'win-bairros',
                title: 'Bairros',
                width: 550,
                height: 280,
                iconCls: 'location-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                items: [moduleBairros]
            });
        }
        win.show();
        return win;
    },

    createCidadesWindow: function () {
        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-cidades');
        if (!win) {
            var moduleCidades = Ext.create('App.view.enderecos.CidadesView',{});
            win = desktop.createWindow({
                id: 'win-cidades',
                title: 'Cidades',
                width: 400,
                height: 280,
                iconCls: 'location-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                items: [moduleCidades]
            });
        }
        win.show();
        return win;
    },

    createEstadosWindow: function () {
        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-estados');
        if (!win) {
            var moduleEstados = Ext.create('App.view.enderecos.EstadosView',{});
            win = desktop.createWindow({
                id: 'win-estados',
                title: 'Estados',
                width: 400,
                height: 280,
                iconCls: 'location-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                items: [moduleEstados]
            });
        }
        win.show();
        return win;
    },

    createPaisesWindow: function () {
        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-paises');
        if (!win) {
            var modulePaises = Ext.create('App.view.enderecos.PaisesView',{});
            win = desktop.createWindow({
                id: 'win-paises',
                title: 'Países',
                width: 400,
                height: 280,
                iconCls: 'location-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                items: [modulePaises]
            });
        }
        win.show();
        return win;
    }
});