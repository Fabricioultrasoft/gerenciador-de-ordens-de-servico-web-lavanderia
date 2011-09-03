
Ext.define('App.controller.servicos.ServicosAddController', {
    extend: 'Ext.app.Controller',

    models: ['servicos.ServicoModel','servicos.ValorServicoModel'],

    views: ['servicos.ServicosAddView'],

    stores: ['servicos.ServicosStore'],

    init: function () {
        this.control({
            '#gridAddServico': {
                itemdblclick: this.editValores
            },
            '#btnAddServico-editValores': {
                click: this.onEditValoresClick
            },
            '#btnAddServico-addCondicaoEspecial': {
                click: this.onAddCondicaoEspecialClick
            },
            '#win-addServico-editValores button[action=save]': {
                click: this.alterarValores
            },
            '#win-addServico-addCondicaoEspecial button[action=save]': {
                click: this.addCondicaoEspecial
            }
        });
    },

    onEditValoresClick: function(btn, eventObject, options) {
        var grid = btn.scope.gridValoresServico;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    editValores: function(view, record, item, index, event, eOpts) {
        var store = view.panel.module.tiposDeClientesStore;
        var winEditValores = view.panel.module.createWinEditValores();
        if(store.getAt(store.find('codigo', record.codigoTipoDeCliente)) == null ) {
            store.add({codigo: record.data.codigoTipoDeCliente, nome: record.data.nomeTipoDeCliente});
            winEditValores.down('combobox').disable();
        }
        winEditValores.show();
        winEditValores.down('form').loadRecord(record);
    },

    onAddCondicaoEspecialClick: function(btn, eventObject, options) {
        btn.scope.createWinAddCondicaoEspecial( btn.scope.gridValoresServico.getSelectionModel().getSelection()[0]).show();
    },

    alterarValores: function (btn) {
        var win = btn.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues();
        if (!form.getForm().isValid()) {
            genericErrorAlert("Erro", "Preencha os campos corretamente antes de alterar os valores");
            return false;
        }

        var count = 0, i;
        for(i = 0; i < record.parentNode.childNodes.length; i++ ) {
            if(record.parentNode.childNodes[i].data.codigoTipoDeCliente != 0
                && record.parentNode.childNodes[i].data.codigoTipoDeCliente == values.codigoTipoDeCliente) {
                count++;
            }
        }
        if(count > 1) {
            genericErrorAlert("Erro", "J&aacute; existe uma condi&ccedil;&atilde;o especial para esse tipo de cliente");
            return false;
        }

        record.set(values);
        win.close();
    },

    addCondicaoEspecial: function (btn) {
        var win = btn.up('window'),
        form = win.down('form'),
        values = form.getValues();

        if (!form.getForm().isValid()) {
            genericErrorAlert("Erro", "Preencha os campos corretamente antes de alterar os valores");
            return false;
        }

        var store = btn.scope.tiposDeClientesStore;
        
        Ext.apply(values, { 
            allowDrag: false,
            nomeTipoDeCliente: store.getAt(store.find('codigo', values.codigoTipoDeCliente)).get('nome')
        });

        var recordPai = btn.scope.gridValoresServico.getSelectionModel().getSelection()[0];
        var record = Ext.create('App.model.servicos.ValorServicoModel',values);

        Ext.ModelManager.create(record, btn.scope.gridValoresServico.getStore().model);
        Ext.data.NodeInterface.decorate(record); 
        
        var count = 0, i;
        for(i = 0; i < recordPai.childNodes.length; i++ ) {
            if(recordPai.childNodes[i].data.codigoTipoDeCliente != 0
                && recordPai.childNodes[i].data.codigoTipoDeCliente == values.codigoTipoDeCliente) {
                count++;
            }
        }
        if(count > 1) {
            genericErrorAlert("Erro", "J&aacute; existe uma condi&ccedil;&atilde;o especial para esse tipo de cliente");
            return false;
        }

        recordPai.appendChild(record);
        btn.scope.gridValoresServico.getView().refreshNode(recordPai.index);
        win.close();
    }
});