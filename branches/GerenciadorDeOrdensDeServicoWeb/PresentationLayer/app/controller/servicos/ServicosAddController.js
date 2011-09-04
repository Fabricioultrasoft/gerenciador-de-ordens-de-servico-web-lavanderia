
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
            '#btnAddServico-delCondicaoEspecial': {
                click: this.onDelCondicaoEspecialClick
            },
            '#win-addServico-editValores button[action=save]': {
                click: this.alterarValores
            },
            '#win-addServico-addCondicaoEspecial button[action=save]': {
                click: this.addCondicaoEspecial
            },
            '#btn-add-servico': {
                click: this.onAddServicoClick
            }
        });
    },

    onEditValoresClick: function(btn, eventObject, options) {
        var grid = btn.scope.gridValoresServico;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    editValores: function(view, record, item, index, event, eOpts) {
        
        var winEditValores = view.panel.module.createWinEditValores();
        var store = winEditValores.down('combobox').store;
        if(store.getAt(store.find('codigo', record.data.codigoTipoDeCliente)) == null ) {
            store.add({codigo: record.data.codigoTipoDeCliente, nome: record.data.nomeTipoDeCliente});
            winEditValores.down('combobox').disable();
        }
        winEditValores.show();
        winEditValores.down('form').loadRecord(record);
    },

    onAddCondicaoEspecialClick: function(btn, eventObject, options) {
        btn.scope.createWinAddCondicaoEspecial( btn.scope.gridValoresServico.getSelectionModel().getSelection()[0]).show();
    },

    onDelCondicaoEspecialClick: function(btn, eventObject, options) {
        var sm = btn.scope.gridValoresServico.getSelectionModel();
        var record = sm.getSelection()[0];

        Ext.Msg.show({
            title: 'Remover Condi&ccedil;&atilde;o Especial',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Tapete: ' + record.data.nomeTapete + '<br />Tipo de cliente: ' + record.data.nomeTipoDeCliente,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    record.remove();
                    try {sm.select(0);} catch(e){}
                }
            },
            icon: Ext.Msg.QUESTION
        });

    },

    alterarValores: function (btn) {
        var win = btn.up('window'),
        form = win.down('form'),
        record = form.getRecord(),
        values = form.getValues(),
        store = form.down('combobox').store;

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

        if(values.codigoTipoDeCliente == null ) {
            values.codigoTipoDeCliente = form.down('combobox').value;
        }

        Ext.apply(values, {
            nomeTipoDeCliente: store.getAt(store.find('codigo', values.codigoTipoDeCliente)).get('nome')
        });

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
        var recordPai = btn.scope.gridValoresServico.getSelectionModel().getSelection()[0];

        Ext.apply(values, {
            codigoPai: recordPai.data.codigo,
            codigoTapete: recordPai.data.codigoTapete,
            iconCls: 'tapete-estrela-thumb',
            leaf: true,
            allowDrag: false,
            nomeTipoDeCliente: store.getAt(store.find('codigo', values.codigoTipoDeCliente)).get('nome')
        });
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
        if(count > 0) {
            genericErrorAlert("Erro", "J&aacute; existe uma condi&ccedil;&atilde;o especial para esse tipo de cliente");
            return false;
        }

        recordPai.appendChild(record);
        recordPai.expand();
        win.close();
    },

    onAddServicoClick: function(btn, event, options) {
        if (!btn.scope.formServico.getForm().isValid()) {
            genericErrorAlert("Erro", "Dados inv&aacute;lidos, passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        }
        var values = btn.scope.formServico.getValues();
        var cobradoPorStore = btn.scope.formServico.down('combobox').store;
        var valoresServico = this.preencherValores(btn.scope.gridValoresServico.getStore().getRootNode());
        var r = Ext.ModelManager.create({
            codigo: 0,
            nome: values.nome,
            codigoCobradoPor: values.codigoCobradoPor,
            nomeCobradoPor: cobradoPorStore.getAt(cobradoPorStore.find('codigo',values.codigoCobradoPor)).get('nome'),
            descricao: values.descricao,
            valores: valoresServico
        }, 'App.model.servicos.ServicoModel');

        btn.scope.mainPanel.setLoading( "Cadastrando...", true );

        var servicosStore = null;
        try { servicosStore = btn.scope.app.getModule("module-servicos-search").servicosStore; }catch(e){}

        if(servicosStore) {
            servicosStore.insert(0, r);
            servicosStore.sync();
            btn.up('window').close();
        }
        else {
            r.setProxy( Ext.create('App.store.servicos.ServicosStore',{}).getProxy() );
            r.save({
                success: function(ed) {
                    btn.up('window').close();
                    Ext.notification.msg('A&ccedil;&atilde;o Conclu&iacute;da', 'Servi&ccedil;o cadastrado!');
                },
                failure: function(record, operation) {
                    btn.scope.mainPanel.setLoading( false, true );
                }
            });
        }
    },

    preencherValores: function(recordPai) {
        var arrValores = new Array();
        recordPai.eachChild( function(record){
            var r = { 
                codigo: record.data.codigo,
                codigoPai: record.data.codigoPai,
                codigoServico: record.data.codigoServico,
                codigoTapete: record.data.codigoTapete,
                nomeTapete: record.data.nomeTapete,
                codigoTipoDeCliente: record.data.codigoTipoDeCliente,
                nomeTipoDeCliente: record.data.nomeTipoDeCliente,
                valor: record.data.valor,
                valorAcima10m2: record.data.valorAcima10m2
            };
            r.valoresAdicionais = this.preencherValores(record);
            arrValores.push(r);
        }, this);
        return arrValores;
    }
});