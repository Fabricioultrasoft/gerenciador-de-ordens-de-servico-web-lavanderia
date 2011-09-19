
Ext.define('App.controller.ordensDeServico.OrdensDeServicoClienteSearchController', {
    extend: 'Ext.app.Controller',

    models: ['clientes.ClienteModel'],

    views: ['ordensDeServico.OrdensDeServicoClienteSearchView'],

    stores: ['clientes.ClientesStore'],

    init: function () {
        this.control({
            '#btnSearchClienteOS': {
                click: this.onSearchClienteOSClick
            },
            '#btnConfirmAddClienteOS': {
                click: this.onConfirmAddClienteOSClick
            }
        });
    },

    onSearchClienteOSClick:  function(btn, eventObject, options) {

        var filtros = btn.scope.form.getValues();

        var arrFilters = new Array();
        arrFilters[0] = new Ext.util.Filter({property: 'codigo',value: filtros.codigo});
        arrFilters[1] = new Ext.util.Filter({property: 'nome',value: filtros.nome});
        arrFilters[2] = new Ext.util.Filter({property: 'ativo',value: true});

        btn.scope.clientesStore.currentPage = 1;
        btn.scope.clientesStore.filters = Ext.create('Ext.util.MixedCollection',{});
        btn.scope.clientesStore.filter(arrFilters);
    },

    onConfirmAddClienteOSClick: function (btn, event, options) {
        var cliente = btn.scope.grid.getSelectionModel().getSelection()[0].data;
        btn.scope.options.moduleTarget.setCliente(cliente);
        btn.up('window').close();
    }
});