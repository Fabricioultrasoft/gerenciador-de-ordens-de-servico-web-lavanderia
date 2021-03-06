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
            },
            '#btnViewOS': {
                click: this.onViewOSClick
            },
            '#btnRelOS': {
                click: this.onRelOSClick
            },
            '#btnFinalizarOS': {
                click: this.onFinalizarOSClick
            },
            '#btnCancelarOS': {
                click: this.onCancelarOSClick
            }
        });
    },

    onSearchOSClick:  function(btn, event, options) {

        var values = btn.scope.form.getValues();
        var arrFilters = new Array();
        
        for(var member in values) {
            arrFilters.push(new Ext.util.Filter({ property: member, value: values[member] }));
        }

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
            title: 'Excluir Ordem de Servi&ccedil;o',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b>'
               + '<br />Numero: ' + sm.getSelection()[0].data.numero 
               + '<br />Cliente: ' + sm.getSelection()[0].data.nomeCliente
               + '<br />Valor Orig.: ' + Ext.util.Format.brMoney(sm.getSelection()[0].data.valorOriginal)
               + '<br />Valor Final: ' + Ext.util.Format.brMoney(sm.getSelection()[0].data.valorFinal)
               + '<br />Todas os dados ser&atilde;o apagados. Caso deseje consultar esta Ordem de Serviço posteriormente, '
               + 'opte pela op&ccedil;&atilde;o de <b>[Cancelar]</b> a Ordem de Servi&ccedil;o.',
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    btn.scope.grid.getStore().remove(sm.getSelection());
                    btn.scope.grid.getStore().sync();
                    btn.scope.grid.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
                }
            },
            icon: Ext.Msg.WARNING
        });
    },

    onShowDescricaoOSClick: function (btn, event, options) {
        btn.scope.grid.getComponent('view').getPlugin('preview').toggleExpanded(btn.pressed);
    },

    onRelOSClick: function (btn, event, options) {
        btn.scope.app.getModule("module-report-os").createWindow();
    },

    onClienteSearchOSClick: function(btn, event, options) {
        btn.scope.app.getModule("module-ordensDeServico-clientesSearch").createWindow(btn.scope);
    },

    onViewOSClick: function(btn, event, options) {
        var os = btn.scope.grid.getSelectionModel().getSelection()[0].data;
        btn.scope.createViewOSWindow(os);
    },

    onFinalizarOSClick: function(btn, event, options) {
        var sm = btn.scope.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Finalizar Ordem de Servi&ccedil;o',
            msg: '<b>Tem certeza de que deseja Finalizar esta Ordem de Servi&ccedil;o?</b>'
               + '<br />Numero: ' + sm.getSelection()[0].data.numero 
               + '<br />Cliente: ' + sm.getSelection()[0].data.nomeCliente
               + '<br />Valor Orig.: ' + Ext.util.Format.brMoney(sm.getSelection()[0].data.valorOriginal)
               + '<br />Valor Final: ' + Ext.util.Format.brMoney(sm.getSelection()[0].data.valorFinal)
               + '<br />Depois de Finalizada, n&atilde;o ser&aacute; mais poss&iacute;vel editar seus dados, '
               + 'mas ainda estar&aacute dispon&iacute;vel para visualiza&ccedil;&atilde;o.',
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    var store = btn.scope.statusStore;
                    var record = sm.getSelection()[0];
                    var cod = 2;// status FINALIZADO
                    record.set({codigoStatus: cod, nomeStatus: store.getAt(store.find('codigo', cod)).get('nome') });
                    record.store.sync();
                }
            },
            icon: Ext.Msg.QUESTION
        });
    },

    onCancelarOSClick: function(btn, event, options) {
        var sm = btn.scope.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Cancelar Ordem de Servi&ccedil;o',
            msg: '<b>Tem certeza de que deseja Cancelar esta Ordem de Servi&ccedil;o?</b>'
               + '<br />Numero: ' + sm.getSelection()[0].data.numero 
               + '<br />Cliente: ' + sm.getSelection()[0].data.nomeCliente
               + '<br />Valor Orig.: ' + Ext.util.Format.brMoney(sm.getSelection()[0].data.valorOriginal)
               + '<br />Valor Final: ' + Ext.util.Format.brMoney(sm.getSelection()[0].data.valorFinal)
               + '<br />Depois de Cancelada, n&atilde;o ser&aacute; mais poss&iacute;vel editar seus dados, '
               + 'mas ainda estar&aacute dispon&iacute;vel para visualiza&ccedil;&atilde;o.',
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    var store = btn.scope.statusStore;
                    var record = sm.getSelection()[0];
                    var cod = 3;// status CANCELADO
                    record.set({codigoStatus: cod, nomeStatus: store.getAt(store.find('codigo', cod)).get('nome') });
                    record.store.sync();
                }
            },
            icon: Ext.Msg.WARNING
        });
    }
});