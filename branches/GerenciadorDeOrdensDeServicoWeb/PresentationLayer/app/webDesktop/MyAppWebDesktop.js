
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
        'App.view.clientes.TiposDeClientesView',
        'App.view.clientes.ClientesView',
        'App.view.clientes.ClientesAddView',
        'App.view.clientes.ClientesEditView',
        'App.view.clientes.ClientesSearchView',
        'App.view.servicos.ServicosView',
        'App.view.servicos.ServicosAddView',
        'App.view.servicos.ServicosEditView',
        'App.view.servicos.ServicosSearchView',
        'App.view.usuarios.UsuariosView',
        'App.view.usuarios.UsuariosAddView',
        'App.view.usuarios.UsuariosEditView',
        'App.view.usuarios.UsuariosSearchView',
        'App.view.ordensDeServico.OrdensDeServicoView',
        'App.view.ordensDeServico.OrdensDeServicoAddView',
        'App.view.ordensDeServico.OrdensDeServicoEditView',
        'App.view.ordensDeServico.OrdensDeServicoSearchView',
        'App.view.ordensDeServico.OrdensDeServicoClienteSearchView',
        'App.view.ordensDeServico.ItensView'
    ],

    init: function () {
        // custom logic before getXYZ methods get called...

        this.callParent();

        // now ready...
    },

    getModules: function () {
        return [
            new App.view.ordensDeServico.OrdensDeServicoView(),
            new App.view.ordensDeServico.OrdensDeServicoAddView(),
            new App.view.ordensDeServico.OrdensDeServicoEditView(),
            new App.view.ordensDeServico.OrdensDeServicoSearchView(),
            new App.view.ordensDeServico.OrdensDeServicoClienteSearchView(),
            new App.view.ordensDeServico.ItensView(),
            new App.view.clientes.ClientesView(),
            new App.view.clientes.ClientesAddView(),
            new App.view.clientes.ClientesEditView(),
            new App.view.clientes.ClientesSearchView(),
            new App.view.servicos.ServicosView(),
            new App.view.servicos.ServicosAddView(),
            new App.view.servicos.ServicosEditView(),
            new App.view.servicos.ServicosSearchView(),
            new App.view.enderecos.EnderecosView(),
            new App.view.enderecos.LogradourosView(),
            new App.view.usuarios.UsuariosView(),
            new App.view.usuarios.UsuariosAddView(),
            new App.view.usuarios.UsuariosEditView(),
            new App.view.usuarios.UsuariosSearchView(),
            new App.view.clientes.TiposDeClientesView(),
            new App.view.tapetes.TapetesView()
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
                    { name: 'Tapetes', iconCls: 'tapete', module: 'module-tapetes' },
                    { name: 'Clientes', iconCls: 'clientes', module: 'module-clientes-search' },
                    { name: 'Servi&ccedil;os', iconCls: 'servicos', module: 'module-servicos-search' },
                    { name: 'Ordens de Servi&ccedil;o', iconCls: 'ordens-de-servico', module: 'module-ordensDeServico-search' }
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
            title: Ext.util.Cookies.get('nomeUsuario'),
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
                        text: 'Sair',
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
        Ext.Msg.confirm('Sair', 'Você confirma o encerramento da aplica&ccedil;&atilde;o?', function (buttonId) {
            if (buttonId == 'yes') {
                Ext.Ajax.request({
                    url: '/PresentationLayer/app/handlers/Logout.ashx',
                    method: 'POST',
                    scope: this,
                    success: function (responseObj) {
                        var response = Ext.decode(responseObj.responseText);
                        if(response.success) {
                            window.location.href = response.redirect;
                        } else {
                            genericErrorAlert("Erro ao sair", response.message);
                        }
                    },
                    exception: function(thisProxy, response, operation, options) {
                        genericExceptionHandler(thisProxy, response, operation, options);
                        this.form.setLoading(false);
                    }
                });
            }
        });
    },

    onSettings: function () {
        
        var dlg = Ext.create('App.webDesktop.Settings',{
            desktop: this.desktop
        });
        dlg.show();
    }
});