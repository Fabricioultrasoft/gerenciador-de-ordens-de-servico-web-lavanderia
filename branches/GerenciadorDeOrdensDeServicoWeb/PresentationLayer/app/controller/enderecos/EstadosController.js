
Ext.define('App.controller.enderecos.EstadosController', {
    extend: 'Ext.app.Controller',

    models: ['enderecos.EstadoModel'],

    views: ['enderecos.EstadosView'],

    stores: ['enderecos.EstadosStore'],

    init: function () {
        this.control({
            '#grid-estados': {
                itemdblclick: this.editEstado
            },
            '#btnAddEstado': {
                click: this.onAddEstadoClick
            },
            '#btnEditEstado': {
                click: this.onEditEstadoClick
            },
            '#btnDelEstado': {
                click: this.onDelEstadoClick
            },
            '#win-add-estado button[action=save]': {
                click: this.inserirEstado
            },
            '#win-edit-estado button[action=save]': {
                click: this.alterarEstado
            }
        });
    },

    editEstado: function (grid, record) {
        var editEstadoWin = grid.up('estadosview').createWinEditEstado();
        editEstadoWin.show();

        var form = editEstadoWin.down('form');
        form.setLoading(true, false);
        var paisStore = Ext.ComponentManager.get('win-edit-estado-combo-pais').store;

        if (paisStore.isLoading()) {
            paisStore.on('load', function () {
                form.loadRecord(record);
                form.setLoading(false, false);
            });
        }
        else {
            form.loadRecord(record);
            form.setLoading(false, false);
        }
    },

    onAddEstadoClick: function (thisButton, eventObject, options) {
        thisButton.scope.createWinAddEstado().show();
    },

    onEditEstadoClick: function(btn, eventObject, options) {
        var grid = btn.scope.grid;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelEstadoClick: function (thisButton, eventObject, options) {
        var estadoView = thisButton.scope;
        var sm = estadoView.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir Estado',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Estado: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    estadoView.grid.getStore().remove(sm.getSelection());
                    estadoView.grid.getStore().sync();
                    estadoView.grid.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
                    if (estadoView.grid.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: thisButton.id,
            icon: Ext.Msg.QUESTION
        });
    },

    inserirEstado: function (button) {
        var win = button.up('window'),
            form = win.down('form'),
            values = form.getValues();

        if (values.codigoPais == null || (values.nome == null || Ext.String.trim(values.nome) == "")) {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de inserir o estado');
        } else {

            var estadosView = Ext.ComponentQuery.query('estadosview')[0];

            var paisStore = Ext.ComponentManager.get('win-add-estado-combo-pais').store;

            var r = Ext.ModelManager.create({
                codigo: 0,
                nome: values.nome,
                codigoPais: values.codigoPais,
                nomePais: paisStore.getAt(paisStore.find('codigo', values.codigoPais)).get('nome')
            }, 'App.model.enderecos.EstadoModel');
            estadosView.grid.getStore().insert(0, r);
            estadosView.grid.getStore().sync();
            estadosView.grid.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
            win.close();
        }
    },

    alterarEstado: function (button) {
        var win = button.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues();

        if (values.nome == null || Ext.String.trim(values.nome) == "") {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de alterar o estado');
        } else {

            var paisStore = Ext.ComponentManager.get('win-edit-estado-combo-pais').store;
            values.nomePais = paisStore.getAt(paisStore.find('codigo', values.codigoPais)).get('nome');

            record.set(values);
            record.store.sync();
            win.close();
        }
    }
});