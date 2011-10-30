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
        'clientes.TiposDeClientesController',
        'servicos.ServicosAddController',
        'servicos.ServicosEditController',
        'servicos.ServicosSearchController',
        'usuarios.UsuariosAddController',
        'usuarios.UsuariosEditController',
        'usuarios.UsuariosSearchController',
        'ordensDeServico.OrdensDeServicoAddController',
        'ordensDeServico.OrdensDeServicoEditController',
        'ordensDeServico.OrdensDeServicoSearchController',
        'ordensDeServico.OrdensDeServicoClienteSearchController',
        'ordensDeServico.ItensController'
    ],
    launch: function () {
        // define funcoes extendidas
        Ext.util.Format.brMoney = function(v) { return Ext.util.Format.currency(v, 'R$ ', 2); };

        Ext.create('App.webDesktop.MyAppWebDesktop', {});
    }
});