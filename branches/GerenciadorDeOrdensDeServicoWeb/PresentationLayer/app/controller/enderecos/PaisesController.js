
Ext.define('App.controller.enderecos.PaisesController', {
    extend: 'Ext.app.Controller',

    models: ['enderecos.PaisModel'],

    views: ['enderecos.PaisesView'],

    stores: ['enderecos.PaisesStore'],

    init: function () {
        this.control({
            '#grid-paises': {
                itemdblclick: this.editPais
            },
            '#btnAddPais': {
                click: this.onAddPaisClick
            },
            '#btnDelPais': {
                click: this.onDelPaisClick
            },
            '#win-add-pais button[action=save]': {
                click: this.inserirPais
            },
            '#win-edit-pais button[action=save]': {
                click: this.alterarPais
            }
        });
    },

    editPais: function (grid, record) {
        var editPaisWin = grid.up('paisesview').createWinEditPais();
        editPaisWin.show();
        editPaisWin.down('form').loadRecord(record);
    },

    onAddPaisClick: function (thisButton, eventObject, options) {
        thisButton.scope.createWinAddPais().show();
    },

    onDelPaisClick: function (thisButton, eventObject, options) {
        var paisesView = thisButton.scope;
        var sm = paisesView.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir pa&iacute;s',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Pa&iacute;s: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    paisesView.grid.getStore().remove(sm.getSelection());
                    paisesView.grid.getStore().sync();
                    Ext.notification.msg('A&ccedil;&atilde;o Conclu&iacute;da', 'O pa&iacute;s selecionado foi excluido');
                    if (paisesView.grid.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: thisButton.id,
            icon: Ext.Msg.QUESTION
        });
    },

    inserirPais: function (button) {
        var win = button.up('window'),
            form = win.down('form'),
            values = form.getValues();

        if (values.nome == null || Ext.String.trim(values.nome) == "") {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de inserir o pa&iacute;s');
        } else {

            var paisesView = Ext.ComponentQuery.query('paisesview')[0];
            var r = Ext.ModelManager.create({
                codigo: 0,
                nome: values.nome
            }, 'App.model.enderecos.PaisModel');
            paisesView.grid.getStore().insert(0, r);
            paisesView.grid.getStore().sync();
            win.close();
        }
    },

    alterarPais: function (button) {
        var win = button.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues();

        if (values.nome == null || Ext.String.trim(values.nome) == "") {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de alterar o pa&iacute;s');
        } else {
            record.set(values);
            record.store.sync();
            win.close();
        }
    }
});