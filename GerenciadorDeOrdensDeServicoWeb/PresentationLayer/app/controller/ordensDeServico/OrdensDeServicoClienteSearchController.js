
Ext.define('App.controller.ordensDeServico.OrdensDeServicoClienteSearchController', {
    extend: 'Ext.app.Controller',

    models: ['clientes.ClienteModel'],

    views: ['ordensDeServico.OrdensDeServicoClienteSearchView'],

    stores: ['clientes.ClientesStore'],

    init: function () {
        this.control({
            '#grid-clientesOS': {
                itemdblclick: this.onRowDblClick
            },
            '#btnSearchClienteOS': {
                click: this.onSearchClienteOSClick
            },
            '#btnConfirmAddClienteOS': {
                click: this.onConfirmAddClienteOSClick
            }
        });
    },

    onRowDblClick: function(grid, record) {
        var cliente = grid.panel.getSelectionModel().getSelection()[0].data;
        grid.panel.module.options.moduleTarget.setCliente(cliente);
        grid.panel.up('window').close();
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
        var grid = btn.scope.grid;
        var record = grid.getSelectionModel().getSelection()[0];
        grid.fireEvent("itemdblclick",grid.view,record ); 
    }
});