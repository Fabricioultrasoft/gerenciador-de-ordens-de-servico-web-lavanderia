﻿
Ext.define('App.controller.ordensDeServico.OrdensDeServicoEditController', {
    extend: 'Ext.app.Controller',

    models: ['ordensDeServico.OrdemDeServicoModel','ordensDeServico.ItemModel'],

    views: ['ordensDeServico.OrdensDeServicoEditView'],

    stores: ['ordensDeServico.OrdensDeServicoStore','ordensDeServico.ItensStore'],

    init: function () {
        this.control({
            '#grid-itensEditOS': {
                itemdblclick: this.editItem
            },
            '#module-ordensDeServico-edit_btnAddItemOS': {
                click: this.onAddItemClick
            },
            '#module-ordensDeServico-edit_btnEditItemOS': {
                click: this.onEditItemClick
            },
            '#module-ordensDeServico-edit_btnDelItemOS': {
                click: this.onDelItemClick
            },
            '#btnShowDescricaoOrdensDeServicoEdit': {
                click: this.onShowDescricaoClick
            },
            '#btnEditClienteOS': {
                click: this.onEditClienteOSClick
            },
            '#btnAlterarOS': {
                click: this.onAlterarOSClick
            },
            '#win-ordensDeServico-edit': {
                destroy: this.onEditOSWindowDestroy
            }
        });
    },
    
    //----------------------------------------------------------------------------------------
    onAddItemClick: function (btn, event, options) {
        if(btn.scope.cliente == null) {
            btn.scope.form.down('#moduleEditOS_codigoCliente').isValid();
            btn.scope.form.down('#moduleEditOS_nomeCliente').isValid();
            Ext.Msg.show({
                title: 'Dados incompletos',
                msg: 'Para incluir um Item na Ordem de Servi&ccedil;o <b>&eacute; preciso informar antes o Cliente</b>, '
                   + 'pois os valores dos Itens são calculados com base no Tipo de Cliente selecionado.',
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.WARNING
            });
        } else {
            btn.scope.app.getModule("module-itensOS").createWindow({targetModule: btn.scope});
        }
    },

    //----------------------------------------------------------------------------------------
    editItem: function (grid, record) {
        grid.panel.module.app.getModule("module-itensOS").createWindow({targetModule: grid.panel.module, record:record, edit: true});
    },

    //----------------------------------------------------------------------------------------
    onEditItemClick: function(btn, event, options) {
        var grid = btn.scope.grid;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    //----------------------------------------------------------------------------------------
    onDelItemClick: function(btn, event, options) {
        var sm = btn.scope.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir Item',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Tapete: ' + sm.getSelection()[0].data.nomeTapete + '<br />Valor: ' + Ext.util.Format.brMoney(sm.getSelection()[0].data.valor),
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) { if (buttonId == 'yes') { btn.scope.grid.getStore().remove(sm.getSelection()); } },
            animateTarget: btn.id,
            icon: Ext.Msg.QUESTION
        });
    },

    onShowDescricaoClick: function (btn, event, options) {
        btn.scope.grid.getComponent('view').getPlugin('preview').toggleExpanded(btn.pressed);
    },

    //----------------------------------------------------------------------------------------
    onEditClienteOSClick: function(btn, event, options) {
        btn.scope.app.getModule("module-ordensDeServico-clientesSearch").createWindow(btn.scope);
    },

    //----------------------------------------------------------------------------------------
    onAlterarOSClick: function(btn, event, options) {
        if (!btn.scope.form.getForm().isValid()) {
            genericErrorAlert("Dados inv&aacute;lidos", "<b>Ordem de Servi&ccedil;o incompleta!</b><br />passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        }

        var record = btn.scope.form.getRecord();
        var values = btn.scope.form.getValues();
        var itens = new Array();

        // data em javascript tem o formato MM/dd/yyyy
        var dataAbertura = new Date(values.dataDeAbertura.substring(3,5) + "/" + values.dataDeAbertura.substring(0,2) + "/" + values.dataDeAbertura.substring(6,11) );
        var dataConclusao = new Date(values.previsaoDeConclusao.substring(3,5) + "/" + values.previsaoDeConclusao.substring(0,2) + "/" + values.previsaoDeConclusao.substring(6,11) );

        if( dataAbertura > dataConclusao ) {
            genericErrorAlert("Dados inv&aacute;lidos", "A <b>data de abertura</b> n&atilde;o pode ser posterior &agrave; <b>data de previs&atilde;o de conclus&atilde;o</b>!");
            return false;
        }

        if(values.valorFinal <= 0) {
            genericErrorAlert("Dados inv&aacute;lidos", "O <b>valor final</b> da Ordem de Servi&ccedil;o n&atilde;o pode ser menor ou igual a zero!");
            return false;
        }

        btn.scope.mainPanel.setLoading( "Alterando...", true );

        btn.scope.itensStore.each( function(record) {
            itens.push(record.data);
        }, this);

        values.cliente = btn.scope.cliente;
        values.itens = itens;

        record.set(values);
        record.store.onWriteCallback = function() { btn.up('window').close(); }
        record.store.proxy.onRequestFailureCallback = function() { btn.scope.mainPanel.setLoading( false ); }
        record.store.sync();
    },

    onEditOSWindowDestroy: function( component, opts ) {
        component.module.form.getRecord().reject();
    }
});