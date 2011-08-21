
Ext.define('App.controller.enderecos.BairrosController', {
    extend: 'Ext.app.Controller',

    models: ['enderecos.BairroModel'],

    views: ['enderecos.BairrosView'],

    stores: ['enderecos.BairrosStore'],

    init: function () {
        this.control({
            '#grid-bairros': {
                itemdblclick: this.editBairro
            },
            '#btnAddBairro': {
                click: this.onAddBairroClick
            },
            '#btnDelBairro': {
                click: this.onDelBairroClick
            },
            '#win-add-bairro button[action=save]': {
                click: this.inserirBairro
            },
            '#win-edit-bairro button[action=save]': {
                click: this.alterarBairro
            },
            '#win-add-bairro-combo-pais': {
                select: this.carregarEstados
            },
            '#win-add-bairro-combo-estado': {
                select: this.carregarCidades
            }
        });
    },

    editBairro: function (grid, record) {
        var editBairroWin = grid.up('bairrosview').createWinEditBairro();
        editBairroWin.show();

        var cidadesStore = Ext.ComponentManager.get('win-edit-bairro-combo-cidade').store;
        cidadesStore.load({ params: { codigoEstado: record.data.codigoEstado} });

        var form = editBairroWin.down('form');
        form.setLoading(true, false);

        if (cidadesStore.isLoading()) {
            cidadesStore.on('load', function () {
                form.loadRecord(record);
                form.setLoading(false, false);
            });
        }
        else {
            form.loadRecord(record);
            form.setLoading(false, false);
        }
    },

    onAddBairroClick: function (thisButton, eventObject, options) {
        thisButton.scope.createWinAddBairro().show();
    },

    onDelBairroClick: function (thisButton, eventObject, options) {
        var bairrosView = thisButton.scope;
        var sm = bairrosView.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir Bairro',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Bairro: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    bairrosView.grid.getStore().remove(sm.getSelection());
                    bairrosView.grid.getStore().sync();
                    Ext.notification.msg('A&ccedil;&atilde;o Conclu&iacute;da', 'O Bairro selecionado foi excluido');
                    if (bairrosView.grid.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: thisButton.id,
            icon: Ext.Msg.QUESTION
        });
    },

    inserirBairro: function (button) {
        var win = button.up('window'),
            form = win.down('form'),
            values = form.getValues();

        if (values.codigoPais == null || values.codigoEstado == null || values.codigoCidade == null || (values.nome == null || Ext.String.trim(values.nome) == "")) {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de inserir o bairro');
        } else {

            var bairrosView = Ext.ComponentQuery.query('bairrosview')[0];

            var paisesStore = Ext.ComponentManager.get('win-add-bairro-combo-pais').store;
            var estadosStore = Ext.ComponentManager.get('win-add-bairro-combo-estado').store;
            var cidadesStore = Ext.ComponentManager.get('win-add-bairro-combo-cidade').store;

            var r = Ext.ModelManager.create({
                codigo: 0,
                nome: values.nome,
                codigoCidade: values.codigoCidade,
                nomeCidade: cidadesStore.getAt(cidadesStore.find('codigo', values.codigoCidade)).get('nome'),
                codigoEstado: values.codigoEstado,
                nomeEstado: estadosStore.getAt(estadosStore.find('codigo', values.codigoEstado)).get('nome'),
                codigoPais: values.codigoPais,
                nomePais: paisesStore.getAt(paisesStore.find('codigo', values.codigoPais)).get('nome')
            }, 'App.model.enderecos.BairroModel');
            bairrosView.grid.getStore().insert(0, r);
            bairrosView.grid.getStore().sync();
            win.close();
        }
    },

    alterarBairro: function (button) {
        var win = button.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues();
        if (values.nome == null || Ext.String.trim(values.nome) == "") {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de alterar o bairro');
        } else {

            var cidadesStore = Ext.ComponentManager.get('win-edit-bairro-combo-cidade').store;
            values.nomeCidade = cidadesStore.getAt(cidadesStore.find('codigo', values.codigoCidade)).get('nome');

            record.set(values);
            record.store.sync();
            win.close();
        }
    },

    carregarEstados: function (combo, model, options) {
        var estadosStore = Ext.ComponentManager.get('win-add-bairro-combo-estado').store;
        estadosStore.load({ params: { codigoPais: model[0].data.codigo} });
    },

    carregarCidades: function (combo, model, options) {
        var cidadesStore = Ext.ComponentManager.get('win-add-bairro-combo-cidade').store;
        cidadesStore.load({ params: { codigoEstado: model[0].data.codigo} });
    }
});