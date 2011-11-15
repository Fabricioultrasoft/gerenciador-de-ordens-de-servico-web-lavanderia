
Ext.define('App.controller.enderecos.LogradourosController', {
    extend: 'Ext.app.Controller',

    models: ['enderecos.LogradouroModel'],

    views: ['enderecos.LogradourosView'],

    stores: ['enderecos.LogradourosStore'],

    init: function () {
        this.control({
            '#grid-logradouros': {
                itemdblclick: this.editLogradouro
            },
            '#btnAddLogradouro': {
                click: this.onAddLogradouroClick
            },
            '#btnEditLogradouro': {
                click: this.onEditLogradouroClick
            },
            '#btnDelLogradouro': {
                click: this.onDelLogradouroClick
            },
            '#win-add-logradouro button[action=save]': {
                click: this.inserirLogradouro
            },
            '#win-edit-logradouro button[action=save]': {
                click: this.alterarLogradouro
            },
            '#win-add-logradouro-combo-pais': {
                select: this.carregarEstados
            },
            '#win-add-logradouro-combo-estado': {
                select: this.carregarCidades
            },
            '#win-add-logradouro-combo-cidade': {
                select: this.carregarBairros
            }
        });
    },

    editLogradouro: function (grid, record) {
        var editLogradouroWin = grid.panel.module.createWinEditLogradouro();
        editLogradouroWin.show();

        var tiposDeLogradourosStore = editLogradouroWin.tiposDeLogradourosStore;
        tiposDeLogradourosStore.load();

        var bairrosStore = editLogradouroWin.bairrosStore;
        bairrosStore.load({ params: { codigoCidade: record.data.codigoCidade} });

        var form = editLogradouroWin.down('form');
        form.setLoading(true, false);

        if (bairrosStore.isLoading()) {
            bairrosStore.on('load', function () {
                form.loadRecord(record);
                form.setLoading(false, false);
            });
        }
        else {
            form.loadRecord(record);
            form.setLoading(false, false);
        }
    },

    onAddLogradouroClick: function (btn, eventObject, options) {
        btn.scope.createWinAddLogradouro().show();
    },

    onEditLogradouroClick: function(btn, eventObject, options) {
        var grid = btn.scope.gridLogradouros;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelLogradouroClick: function (btn, eventObject, options) {
        var grid = btn.scope.gridLogradouros;
        var sm = grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir Logradouro',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Logradouro: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    grid.getStore().remove(sm.getSelection());
                    grid.getStore().sync();
                    grid.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
                    if (grid.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: btn.id,
            icon: Ext.Msg.QUESTION
        });
    },

    inserirLogradouro: function (btn) {
        var win = btn.up('window'),
            form = win.down('form'),
            values = form.getValues();

        if (values.codigoPais == null 
            || values.codigoEstado == null 
            || values.codigoCidade == null
            || values.codigoBairro == null 
            || values.codigoTipoDeLogradouro == null
            || (values.nome == null || Ext.String.trim(values.nome) == "")) {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de inserir o logradouro');
        } else {

            var paisesStore = win.paisesStore;
            var estadosStore = win.estadosStore;
            var cidadesStore = win.cidadesStore;
            var bairrosStore = win.bairrosStore;
            var tiposDeLogradourosStore = win.tiposDeLogradourosStore;

            var r = Ext.ModelManager.create({
                codigo: 0,
                nome: values.nome,
                cep: values.cep,
                codigoTipoDeLogradouro: values.codigoTipoDeLogradouro,
                nomeTipoDeLogradouro: tiposDeLogradourosStore.getAt(tiposDeLogradourosStore.find('codigo', values.codigoTipoDeLogradouro)).get('nome'),
                codigoBairro: values.codigoBairro,
                nomeBairro: bairrosStore.getAt(bairrosStore.find('codigo', values.codigoBairro)).get('nome'),
                codigoCidade: values.codigoCidade,
                nomeCidade: cidadesStore.getAt(cidadesStore.find('codigo', values.codigoCidade)).get('nome'),
                codigoEstado: values.codigoEstado,
                nomeEstado: estadosStore.getAt(estadosStore.find('codigo', values.codigoEstado)).get('nome'),
                codigoPais: values.codigoPais,
                nomePais: paisesStore.getAt(paisesStore.find('codigo', values.codigoPais)).get('nome')
            }, 'App.model.enderecos.LogradouroModel');
            btn.scope.gridLogradouros.getStore().insert(0, r);
            btn.scope.gridLogradouros.getStore().sync();
            btn.scope.gridLogradouros.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
            win.close();
        }
    },

    alterarLogradouro: function (btn) {
        var win = btn.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues();
        if (values.nome == null || Ext.String.trim(values.nome) == "") {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de alterar o logradouro');
        } else {

            var bairrosStore = win.bairrosStore;
            values.nomeBairro = bairrosStore.getAt(bairrosStore.find('codigo', values.codigoBairro)).get('nome');

            var tiposDeLogradourosStore = win.tiposDeLogradourosStore;
            values.nomeTipoDeLogradouro = tiposDeLogradourosStore.getAt(tiposDeLogradourosStore.find('codigo', values.codigoTipoDeLogradouro)).get('nome');

            record.set(values);
            record.store.sync();
            win.close();
        }
    },

    carregarEstados: function (combo, model, options) {
        var estadosStore = Ext.ComponentManager.get('win-add-logradouro-combo-estado').store;
        estadosStore.load({ params: { codigoPais: model[0].data.codigo} });
    },

    carregarCidades: function (combo, model, options) {
        var cidadesStore = Ext.ComponentManager.get('win-add-logradouro-combo-cidade').store;
        cidadesStore.load({ params: { codigoEstado: model[0].data.codigo} });
    },

    carregarBairros: function (combo, model, options) {
        var bairrosStore = Ext.ComponentManager.get('win-add-logradouro-combo-bairro').store;
        bairrosStore.load({ params: { codigoCidade: model[0].data.codigo} });
    }
});