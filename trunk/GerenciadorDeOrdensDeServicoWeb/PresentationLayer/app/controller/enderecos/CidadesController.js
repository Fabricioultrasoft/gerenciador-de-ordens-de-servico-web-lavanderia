
Ext.define('App.controller.enderecos.CidadesController', {
    extend: 'Ext.app.Controller',

    models: ['enderecos.CidadeModel'],

    views: ['enderecos.CidadesView'],

    stores: ['enderecos.CidadesStore'],

    init: function () {
        this.control({
            '#grid-cidades': {
                itemdblclick: this.editCidade
            },
            '#btnAddCidade': {
                click: this.onAddCidadeClick
            },
            '#btnEditCidade': {
                click: this.onEditCidadeClick
            },
            '#btnDelCidade': {
                click: this.onDelCidadeClick
            },
            '#win-add-cidade button[action=save]': {
                click: this.inserirCidade
            },
            '#win-edit-cidade button[action=save]': {
                click: this.alterarCidade
            },
            '#win-add-cidade-combo-pais': {
                select: this.carregarEstados
            }
        });
    },

    editCidade: function (grid, record) {
        var editCidadeWin = grid.up('cidadesview').createWinEditCidade();
        editCidadeWin.show();

        var estadosStore = Ext.ComponentManager.get('win-edit-cidade-combo-estado').store;
        estadosStore.load({ params: { codigoPais: record.data.codigoPais} });

        var form = editCidadeWin.down('form');
        form.setLoading(true, false);

        if (estadosStore.isLoading()) {
            estadosStore.on('load', function () {
                form.loadRecord(record);
                form.setLoading(false, false);
            });
        }
        else {
            form.loadRecord(record);
            form.setLoading(false, false);
        }
    },

    onAddCidadeClick: function (thisButton, eventObject, options) {
        thisButton.scope.createWinAddCidade().show();
    },

    onEditCidadeClick: function(btn, eventObject, options) {
        var grid = btn.scope.grid;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelCidadeClick: function (thisButton, eventObject, options) {
        var cidadesView = thisButton.scope;
        var sm = cidadesView.grid.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir Cidade',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Cidade: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    cidadesView.grid.getStore().remove(sm.getSelection());
                    cidadesView.grid.getStore().sync();
                    cidadesView.grid.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
                    if (cidadesView.grid.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: thisButton.id,
            icon: Ext.Msg.QUESTION
        });
    },

    inserirCidade: function (button) {
        var win = button.up('window'),
            form = win.down('form'),
            values = form.getValues();

        if (values.codigoPais == null || values.codigoEstado == null || (values.nome == null || Ext.String.trim(values.nome) == "")) {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de inserir a cidade');
        } else {

            var cidadesView = Ext.ComponentQuery.query('cidadesview')[0];

            var paisesStore = Ext.ComponentManager.get('win-add-cidade-combo-pais').store;
            var estadosStore = Ext.ComponentManager.get('win-add-cidade-combo-estado').store;

            var r = Ext.ModelManager.create({
                codigo: 0,
                nome: values.nome,
                codigoEstado: values.codigoEstado,
                nomeEstado: estadosStore.getAt(estadosStore.find('codigo', values.codigoEstado)).get('nome'),
                codigoPais: values.codigoPais,
                nomePais: paisesStore.getAt(paisesStore.find('codigo', values.codigoPais)).get('nome')
            }, 'App.model.enderecos.CidadeModel');
            cidadesView.grid.getStore().insert(0, r);
            cidadesView.grid.getStore().sync();
            cidadesView.grid.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
            win.close();
        }
    },

    alterarCidade: function (button) {
        var win = button.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues();

        if (values.nome == null || Ext.String.trim(values.nome) == "") {
            Ext.Msg.alert('Aten&ccedil;&atilde;o', 'Preencha todos os campos antes de alterar a cidade');
        } else {

            var estadosStore = Ext.ComponentManager.get('win-edit-cidade-combo-estado').store;
            values.nomeEstado = estadosStore.getAt(estadosStore.find('codigo', values.codigoEstado)).get('nome');

            record.set(values);
            record.store.sync();
            win.close();
        }
    },

    carregarEstados: function (combo, model, options) {
        var estadosStore = Ext.ComponentManager.get('win-add-cidade-combo-estado').store;
        estadosStore.load({ params: { codigoPais: model[0].data.codigo} });
    }
});