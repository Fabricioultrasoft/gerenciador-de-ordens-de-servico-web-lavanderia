Ext.Loader.setConfig({
    enabled: true,
    disableCaching: false
});

Ext.application({
    name: 'App',

    appFolder: 'app',
    controllers: [
        'enderecos.PaisesController',
        'enderecos.EstadosController',
        'enderecos.CidadesController',
        'enderecos.BairrosController',
        'enderecos.LogradourosController',
        'enderecos.TiposDeLogradourosController',
        'tapetes.TapetesController',
        'clientes.ClientesAddController',
        'clientes.ClientesEditController',
        'clientes.ClientesSearchController',
        'servicos.ServicosAddController',
        'servicos.ServicosEditController',
        'servicos.ServicosSearchController',
        'usuarios.UsuariosAddController',
        'usuarios.UsuariosEditController',
        'usuarios.UsuariosSearchController'
    ],
    launch: function () {
        Ext.create('App.webDesktop.MyAppWebDesktop', {});
    }
});

Ext.notification = function () {
    var msgCt;
    function createBox(t, s) {
        return '<div class="msg"><h3>' + t + '</h3><p>' + s + '</p></div>';
    }
    return {
        msg: function (title, format) {
            if (!msgCt) {
                msgCt = Ext.core.DomHelper.insertFirst(document.body, { id: 'msg-div' }, true);
            }
            var s = Ext.String.format.apply(String, Array.prototype.slice.call(arguments, 1));
            var m = Ext.core.DomHelper.append(msgCt, createBox(title, s), true);
            m.hide();
            m.slideIn('t').ghost("t", { delay: 5000, remove: true });
        },
        init: function () {}
    };
} (); 