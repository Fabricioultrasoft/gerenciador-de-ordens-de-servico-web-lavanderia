
Ext.define('App.controller.tapetes.TapetesController', {
    extend: 'Ext.app.Controller',

    models: ['tapetes.TapeteModel'],

    views: ['tapetes.TapetesView'],

    stores: ['tapetes.TapetesStore'],

    init: function () {
        this.control({
            '#grid-tapetes': {
                itemdblclick: this.editTapete
            },
            '#btnAddTapete': {
                click: this.onAddTapeteClick
            },
            '#btnEditTapete': {
                click: this.onEditTapeteClick
            },
            '#btnDelTapete': {
                click: this.onDelTapeteClick
            },
            '#btnLimparTapete': {
                click: this.onLimparTapeteClick
            },
            '#btnShowDescricaoTapete': {
                click: this.onShowDescricaoTapeteClick
            },
            '#win-edit-tapete button[action=save]': {
                click: this.alterarTapete
            }
        });
    },

    editTapete: function (grid, record) {
        var editTapeteWin = grid.panel.tapetesView.createWinEditTapete();
        editTapeteWin.show();
        var form = editTapeteWin.down('form');
        form.loadRecord(record);
    },

    onAddTapeteClick: function (btn, event, options) {
        if (!btn.scope.formTapetes.getForm().isValid()) {
            genericErrorAlert("Erro", "Dados inv&aacute;lidos");
            return false;
        }

        var win = btn.up('window'),
            form = win.down('form'),
            values = form.getValues();

        var r = Ext.ModelManager.create({
            codigo: 0,
            nome: values.nome,
            descricao: values.descricao,
            ativo: ((values.ativo == 1) ? true : false)
        }, 'App.model.tapetes.TapeteModel');
        btn.scope.gridTapetes.getStore().insert(0, r);
        btn.scope.gridTapetes.getStore().sync();
        btn.scope.formTapetes.getForm().reset();
    },

    onEditTapeteClick: function(btn, eventObject, options) {
        var grid = btn.scope.gridTapetes;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelTapeteClick: function (btn, eventObject, options) {
        var sm = btn.scope.gridTapetes.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir tapete',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Tapete: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    btn.scope.gridTapetes.getStore().remove(sm.getSelection());
                    btn.scope.gridTapetes.getStore().sync();
                    Ext.notification.msg('A&ccedil;&atilde;o Conclu&iacute;da', 'O tapete selecionado foi excluido');
                    if (btn.scope.gridTapetes.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: btn.id,
            icon: Ext.Msg.QUESTION
        });
    },

    onLimparTapeteClick: function (btn, event, options) {
        btn.scope.formTapetes.getForm().reset();
    },

    onShowDescricaoTapeteClick: function (btn, event, options) {
        btn.scope.gridTapetes.getComponent('view').getPlugin('preview').toggleExpanded(btn.pressed);
    },

    alterarTapete: function (btn) {
        var win = btn.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues();
        
        values.ativo = ((values.ativo == 1) ? true : false);

        if (!form.getForm().isValid()) {
            genericErrorAlert("Erro", "Preencha os campos corretamente antes de alterar o tapete");
            return false;
        }

        record.set(values);
        record.store.sync();
        win.close();
    }
});