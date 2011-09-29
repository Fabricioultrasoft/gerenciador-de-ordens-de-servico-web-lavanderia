﻿
Ext.define('App.controller.ordensDeServico.OrdensDeServicoSearchController', {
    extend: 'Ext.app.Controller',

    models: ['ordensDeServico.OrdemDeServicoModel', 'ordensDeServico.StatusModel'],

    views: ['ordensDeServico.OrdensDeServicoSearchView'],

    stores: ['ordensDeServico.OrdensDeServicoStore','ordensDeServico.StatusStore'],

    init: function () {
        this.control({
            '#grid-ordensDeServico': {
                itemdblclick: this.editOS
            },
            '#btnSearchOS': {
                click: this.onSearchOSClick
            },
            '#btnLimparFiltrosOS': {
                click: this.onLimparFiltrosOSClick
            },
            '#btnAddOS': {
                click: this.onAddOSClick
            },
            '#btnEditOS': {
                click: this.onEditOSClick
            },
            '#btnDelOS': {
                click: this.onDelOSClick
            },
            '#btnShowDescricaoOS': {
                click: this.onShowDescricaoOSClick
            },
            '#btnClienteSearchOS': {
                click: this.onClienteSearchOSClick
            }
        });
    },

    onSearchOSClick:  function(btn, event, options) {

        var filtros = btn.scope.form.getValues();

        var arrFilters = new Array();
        arrFilters[0] = new Ext.util.Filter({property: 'numero',value: filtros.numero});
        arrFilters[1] = new Ext.util.Filter({property: 'codigoStatus',value: filtros.codigoStatus});
        arrFilters[2] = new Ext.util.Filter({property: 'valorOriginal',value: filtros.valorOriginal});
        arrFilters[3] = new Ext.util.Filter({property: 'valorFinal',value: filtros.valorFinal});
        arrFilters[4] = new Ext.util.Filter({property: 'dataDeAbertura',value: filtros.dataDeAbertura});
        arrFilters[5] = new Ext.util.Filter({property: 'previsaoDeConclusao',value: filtros.previsaoDeConclusao});
        arrFilters[6] = new Ext.util.Filter({property: 'dataDeEncerramento',value: filtros.dataDeEncerramento});
        arrFilters[7] = new Ext.util.Filter({property: 'codigoCliente',value: filtros.codigoCliente});
        arrFilters[8] = new Ext.util.Filter({property: 'nomeCliente',value: filtros.nomeCliente});
        
        btn.scope.ordensDeServicoStore.currentPage = 1;
        btn.scope.ordensDeServicoStore.filters = Ext.create('Ext.util.MixedCollection',{});
        btn.scope.ordensDeServicoStore.filter(arrFilters);
    },

    onLimparFiltrosOSClick:  function(btn, event, options) {
        btn.scope.form.getForm().reset();
        btn.scope.ordensDeServicoStore.clearFilter( true );
    },

    editOS: function (grid, record) {
        grid.panel.module.app.getModule("module-ordensDeServico-edit").createWindow({record:record});
    },

    onAddOSClick: function(btn, event, options) {
        btn.scope.app.getModule("module-ordensDeServico-add").createWindow();
    },

    onEditOSClick: function(btn, event, options) {
        var grid = btn.scope.grid;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelOSClick: function (btn, event, options) {
        var sm = btn.scope.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir Ordem de Servi7ccedil;o',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b>'
               + '<br />Numero: ' + sm.getSelection()[0].data.numero 
               + '<br />Cliente: ' + sm.getSelection()[0].data.nomeCliente
               + '<br />Valor Orig.: ' + Ext.util.Format.brMoney(sm.getSelection()[0].data.valorOriginal)
               + '<br />Valor Final: ' + Ext.util.Format.brMoney(sm.getSelection()[0].data.valorFinal),
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    btn.scope.grid.getStore().remove(sm.getSelection());
                    btn.scope.grid.getStore().sync();
                    btn.scope.grid.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
                }
            },
            icon: Ext.Msg.QUESTION
        });
    },

    onShowDescricaoOSClick: function (btn, event, options) {
        btn.scope.grid.getComponent('view').getPlugin('preview').toggleExpanded(btn.pressed);
    },

    onClienteSearchOSClick: function(btn, event, options) {
        btn.scope.app.getModule("module-ordensDeServico-clientesSearch").createWindow(btn.scope);
    }
});