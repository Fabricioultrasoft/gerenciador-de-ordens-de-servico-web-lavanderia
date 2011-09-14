
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
            }
        });
    },

    onSearchClienteClick:  function(btn, eventObject, options) {
        
        btn.scope.gridClientes.setLoading();

        var filtros = btn.scope.formFiltros.getValues();

        var arrFilters = new Array();
        arrFilters[0] = new Ext.util.Filter({property: 'codigo',value: filtros.codigo});
        arrFilters[1] = new Ext.util.Filter({property: 'ativo',value: filtros.ativo});
        arrFilters[2] = new Ext.util.Filter({property: 'nome',value: filtros.nome});
        arrFilters[3] = new Ext.util.Filter({property: 'conjuge',value: filtros.conjuge});
        arrFilters[4] = new Ext.util.Filter({property: 'codigoTipoDeCliente',value: filtros.codigoTipoDeCliente});
        arrFilters[5] = new Ext.util.Filter({property: 'dataDeNascimento',value: filtros.dataDeNascimento});
        arrFilters[6] = new Ext.util.Filter({property: 'sexo',value: filtros.sexo});
        arrFilters[7] = new Ext.util.Filter({property: 'rg',value: filtros.rg});
        arrFilters[8] = new Ext.util.Filter({property: 'cpf',value: filtros.cpf});
        
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
    }
});