
Ext.define('App.controller.clientes.TiposDeClientesController', {
    extend: 'Ext.app.Controller',

    models: ['clientes.TipoDeClienteModel'],

    views: ['clientes.TiposDeClientesView'],

    stores: ['clientes.TiposDeClientesStore'],

    init: function () {
        this.control({
            '#grid-tiposDeClientes': {
                itemdblclick: this.editTipoDeCliente
            },
            '#btnAddTipoDeCliente': {
                click: this.onAddTipoDeClienteClick
            },
            '#btnEditTipoDeCliente': {
                click: this.onEditTipoDeClienteClick
            },
            '#btnDelTipoDeCliente': {
                click: this.onDelTipoDeClienteClick
            },
            '#win-tiposDeClientes-add button[action=save]': {
                click: this.inserirTipoDeCliente
            },
            '#win-tiposDeClientes-edit button[action=save]': {
                click: this.alterarTipoDeCliente
            }
        });
    },

    editTipoDeCliente: function (grid, record) {
        var win = grid.panel.module.createWinEditTipoDeCliente();
        win.show();
        win.down('form').loadRecord(record);
    },

    onAddTipoDeClienteClick: function (btn, eventObject, options) {
        btn.scope.createWinAddTipoDeCliente().show();
    },

    onEditTipoDeClienteClick: function (btn, eventObject, options) {
        var grid = btn.scope.grid;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelTipoDeClienteClick: function (btn, eventObject, options) {
        var module = btn.scope;
        var sm = module.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir Tipo de Cliente',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Nome: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    module.grid.getStore().remove(sm.getSelection());
                    module.grid.getStore().sync();
                    Ext.notification.msg('A&ccedil;&atilde;o Conclu&iacute;da', 'O tipo de cliente selecionado foi excluido');
                    if (module.grid.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            icon: Ext.Msg.QUESTION
        });
    },

    inserirTipoDeCliente: function (btn) {
        var win = btn.up('window'),
            form = win.down('form'),
            values = form.getValues();

        if (!form.getForm().isValid()) {
            genericErrorAlert("Erro", "Dados inv&aacute;lidos, passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        }
        
        var module = btn.scope;
        var r = Ext.ModelManager.create({
            codigo: 0,
            nome: values.nome,
            ativo: 1
        }, 'App.model.clientes.TipoDeClienteModel');
        module.grid.getStore().insert(0, r);
        module.grid.getStore().sync();
        win.close();
    },

    alterarTipoDeCliente: function (btn) {
        var win = btn.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues();

        values.ativo = (values.ativo) ? 1 : 0;

        if (!form.getForm().isValid()) {
            genericErrorAlert("Erro", "Dados inv&aacute;lidos, passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        }

        record.set(values);
        record.store.sync();
        win.close();
    }
});