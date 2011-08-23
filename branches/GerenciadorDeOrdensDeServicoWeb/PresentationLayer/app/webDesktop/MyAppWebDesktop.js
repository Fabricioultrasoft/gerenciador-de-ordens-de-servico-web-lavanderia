﻿
// ESTA CLASSE ESTENDE 'App.webDesktop.AppDesktop'
// e é aqui que as funcionalidades referentes a aplicacao sao adicionadas
Ext.define('App.webDesktop.MyAppWebDesktop', {
    extend: 'App.webDesktop.AppWebDesktop',

    requires: [
        'Ext.window.MessageBox',
        'App.webDesktop.ShortcutModel',
        'App.webDesktop.Settings',
        'App.view.enderecos.LogradourosView',
        'App.view.enderecos.EnderecosView',
        'App.view.tapetes.TapetesView',
        'App.view.clientes.ClientesView',
        'App.view.clientes.ClientesAddView',
        'App.view.clientes.ClientesEditView',
        'App.view.clientes.ClientesSearchView'
    ],

    init: function () {
        // custom logic before getXYZ methods get called...

        this.callParent();

        // now ready...
    },

    getModules: function () {
        return [
            new App.view.tapetes.TapetesView(),
            new App.view.clientes.ClientesView(),
            new App.view.clientes.ClientesAddView(),
            new App.view.clientes.ClientesEditView(),
            new App.view.clientes.ClientesSearchView(),
            new App.view.enderecos.LogradourosView(),
            new App.view.enderecos.EnderecosView()
        ];
    },

    getDesktopConfig: function () {
        var me = this;
        var ret = me.callParent();

        return Ext.apply(ret, {

            contextMenuItems: [
                { text: 'Alterar configurações', handler: me.onSettings, scope: me }
            ],

            shortcuts: Ext.create('Ext.data.Store', {
                model: 'App.webDesktop.ShortcutModel',
                data: [
                    { name: 'Tapetes', iconCls: 'tapete', module: 'win-tapetes' },
                    { name: 'Clientes', iconCls: 'clientes-search', module: 'module-clientes-search' }
                ]
            }),

            wallpaper: 'resources/images/wallpapers/desk.jpg',
            wallpaperStretch: true
        });
    },

    // config for the start menu
    getStartConfig: function () {
        var me = this, ret = me.callParent();

        return Ext.apply(ret, {
            title: 'MyAppWebDesktop.js',
            height: 300,
            toolConfig: {
                width: 100,
                items: [
                    {
                        text: 'Settings',
                        iconCls: 'btn-settings',
                        handler: me.onSettings,
                        scope: me
                    },
                    '-',
                    {
                        text: 'Logout',
                        iconCls: 'btn-logout',
                        handler: me.onLogout,
                        scope: me
                    }
                ]
            }
        });
    },

    getTaskbarConfig: function () {
        var ret = this.callParent();

        return Ext.apply(ret, {
            quickStart: [
                { name: 'Logradouros', iconCls: 'location-thumb', module: 'module-logradouros' }
            ],
            trayItems: [
                { xtype: 'trayclock', flex: 1 }
            ]
        });
    },

    onLogout: function () {
        Ext.Msg.confirm('Logout', 'Are you sure you want to logout?');
    },

    onSettings: function () {
        
        var dlg = Ext.create('App.webDesktop.Settings',{
            desktop: this.desktop
        });
        dlg.show();
    }
});