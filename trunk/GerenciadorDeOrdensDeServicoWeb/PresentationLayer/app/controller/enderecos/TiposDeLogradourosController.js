
Ext.define('App.controller.enderecos.TiposDeLogradourosController', {
    extend: 'Ext.app.Controller',

    models: ['enderecos.TipoDeLogradouroModel'],

    views: ['enderecos.TiposDeLogradourosView'],

    stores: ['enderecos.TiposDeLogradourosStore'],

    init: function () {
        this.control({
            '#grid-tipos-de-logradouros': {
                itemdblclick: this.editTipoDeLogradouro
            },
            '#btnAddTipoDeLogradouro': {
                click: this.onAddTipoDeLogradouroClick
            },
            '#btnEditTipoDeLogradouro': {
                click: this.onEditTipoDeLogradouroClick
            },
            '#btnDelTipoDeLogradouro': {
                click: this.onDelTipoDeLogradouroClick
            },
            '#win-add-tipo-de-logradouro button[action=save]': {
                click: this.inserirTipoDeLogradouro
            },
            '#win-edit-tipo-de-logradouro button[action=save]': {
                click: this.alterarTipoDeLogradouro
            }
        });
    },

    editTipoDeLogradouro: function (grid, record) {
        var editTipoDeLogradouroWin = grid.up('tiposdelogradourosview').createWinEditTipoDeLogradouro();
        editTipoDeLogradouroWin.show();
        editTipoDeLogradouroWin.down('form').loadRecord(record);
    },

    onAddTipoDeLogradouroClick: function (thisButton, eventObject, options) {
        thisButton.scope.createWinAddTipoDeLogradouro().show();
    },

    onEditTipoDeLogradouroClick: function(btn, eventObject, options) {
        var grid = btn.scope.grid;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelTipoDeLogradouroClick: function (thisButton, eventObject, options) {
        var tiposDeLogradourosView = thisButton.scope;
        var sm = tiposDeLogradourosView.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir tipo de logradouro',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Tipo de logradouro: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    tiposDeLogradourosView.grid.getStore().remove(sm.getSelection());
                    tiposDeLogradourosView.grid.getStore().sync();
                    tiposDeLogradourosView.grid.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
                    if (tiposDeLogradourosView.grid.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: thisButton.id,
            icon: Ext.Msg.QUESTION
        });
    },

    inserirTipoDeLogradouro: function (button) {
        var win = button.up('window'),
            form = win.down('form'),
            values = form.getValues();

        if (values.nome == null || Ext.String.trim(values.nome) == "") {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de inserir o tipo de logradouro');
        } else {

            var tiposDeLogradourosView = Ext.ComponentQuery.query('tiposdelogradourosview')[0];
            var r = Ext.ModelManager.create({
                codigo: 0,
                nome: values.nome
            }, 'App.model.enderecos.TipoDeLogradouroModel');
            tiposDeLogradourosView.grid.getStore().insert(0, r);
            tiposDeLogradourosView.grid.getStore().sync();
            tiposDeLogradourosView.grid.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
            win.close();
        }
    },

    alterarTipoDeLogradouro: function (button) {
        var win = button.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues();

        if (values.nome == null || Ext.String.trim(values.nome) == "") {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de alterar o tipo de logradouro');
        } else {
            record.set(values);
            record.store.sync();
            win.close();
        }
    }
});