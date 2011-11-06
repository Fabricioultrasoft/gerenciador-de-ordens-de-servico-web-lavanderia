
Ext.define('App.controller.clientes.ClientesSearchController', {
    extend: 'Ext.app.Controller',

    models: ['clientes.ClienteModel'],

    views: ['clientes.ClientesSearchView'],

    stores: ['clientes.ClientesStore'],

    init: function () {
        this.control({
            '#grid-clientes': {
                itemdblclick: this.editCliente
            },
            '#btnSearchCliente': {
                click: this.onSearchClienteClick
            },
            '#btnLimparFiltros': {
                click: this.onLimparFiltrosClick
            },
            '#btnEditCliente': {
                click: this.onEditClienteClick
            },
            '#btnDelCliente': {
                click: this.onDelClienteClick
            },
            '#btnShowDescricaoCliente': {
                click: this.onShowDescricaoClienteClick
            },
            '#btnRelClientes': {
                click: this.onRelClientesClick
            }
        });
    },

    onSearchClienteClick:  function(btn, eventObject, options) {

        var values = btn.scope.formFiltros.getValues();
        var arrFilters = new Array();

        for(var member in values) {
            arrFilters.push(new Ext.util.Filter({ property: member, value: values[member] }));
        }

        btn.scope.clientesStore.currentPage = 1;
        btn.scope.clientesStore.filters = Ext.create('Ext.util.MixedCollection',{});
        btn.scope.clientesStore.filter(arrFilters);
    },

    onLimparFiltrosClick:  function(btn, eventObject, options) {
        btn.scope.formFiltros.getForm().reset();
        btn.scope.clientesStore.clearFilter( true );
    },

    editCliente: function (grid, record) {
        grid.panel.module.app.getModule("module-clientes-edit").createWindow({record:record});
    },

    onEditClienteClick: function(btn, eventObject, options) {
        var grid = btn.scope.gridClientes;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelClienteClick: function (btn, eventObject, options) {
        var sm = btn.scope.gridClientes.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir cliente',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Cliente: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    btn.scope.gridClientes.getStore().remove(sm.getSelection());
                    btn.scope.gridClientes.getStore().sync();
                    btn.scope.gridClientes.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
                    if (btn.scope.gridClientes.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: btn.id,
            icon: Ext.Msg.QUESTION
        });
    },

    onShowDescricaoClienteClick: function (btn, event, options) {
        btn.scope.gridClientes.getComponent('view').getPlugin('preview').toggleExpanded(btn.pressed);
    },

    onRelClientesClick: function (btn, event, options) {
        btn.scope.app.getModule("module-report-clientes").createWindow();
    }
});