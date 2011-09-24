﻿
Ext.define('App.controller.ordensDeServico.ItensController', {
    extend: 'Ext.app.Controller',

    models: ['ordensDeServico.ItemModel','ordensDeServico.ItemServicoModel','servicos.ServicoEspecificoModel'],

    views: ['ordensDeServico.ItensView'],

    stores: ['ordensDeServico.ItensStore','ordensDeServico.ItensServicosStore','servicos.ServicosEspecificosStore'],

    init: function () {
        this.control({
            '#cboTapetes-itensOS': {
                select: this.onTapeteSelect
            },
            '#grid-servicosDoItemOS': {
                itemdblclick: this.editServico
            },
            '#btnAddServicos-ItemOS': {
                click: this.onAddServicoClick
            },
            '#btnEditServicos-ItemOS': {
                click: this.onEditServicoClick
            },
            '#btnDelServicos-ItemOS': {
                click: this.onDelServicoClick
            },
            '#module-itensOS-comprimento': {
                keyup: this.calcularArea
            },
            '#module-itensOS-largura': {
                keyup: this.calcularArea
            },
            //--------------------------------
            '#cboValoresEspecificos-itemOS': {
                select: this.onValorEspecificoSelect   
            },
            '#qtdMm2-itemOS': {
                keyup: this.onQtdMm2KeyUp
            },
            '#btnConfirmServicoOS':{
                click: this.confirmServicoOSClick
            },
            '#win-servicoNoItemOS': {
                destroy: this.onServicosWindowDestroy
            }
        });
    },

    onTapeteSelect: function( combo, records, opts ) {
        combo.scope.servicosEspecificosStore.load({ params:{ codigoTapete: combo.getValue(), codigoTipoDeCliente: combo.scope.options.targetModule.cliente.codigoTipoDeCliente } });
    },
    
    onAddServicoClick: function(btn, event, options) {
        if(btn.scope.form.getForm().isValid() == false) {
            Ext.Msg.show({
                title: 'Dados incompletos',
                msg: 'Para incluir um Servi&ccedil;o ao tapete <b>&eacute; preciso informar antes o Tapete e suas Dimens&otilde;es</b>, '
                   + 'pois os valores do servi&ccedil;o são calculados com base no Tapete selecionado.',
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.WARNING
            });
        } else {
            // habilita os campos para poder recuperar os valores
            btn.scope.habilitaDadosTapete();

            var values = btn.scope.form.getValues();
            var tapetesStore = btn.scope.tapetesStore;
            btn.scope.createServicoWindow({
                servicosEspecificosStore: btn.scope.servicosEspecificosStore,
                codigoTapete: values.codigoTapete,
                nomeTapete: tapetesStore.getAt(tapetesStore.find('codigo', values.codigoTapete)).get('nome'),
                comprimento: values.comprimento,
                largura: values.largura,
                codigoTipoDeCliente: btn.scope.options.targetModule.cliente.codigoTipoDeCliente,
                nomeTipoDeCliente: btn.scope.options.targetModule.cliente.nomeTipoDeCliente
            });

            // desabilita os campos para o usuario nao alterar os valores
            btn.scope.desabilitaDadosTapete();
        }
    },

    editServico: function (grid, record) {
        // habilita os campos para poder recuperar os valores
        grid.panel.module.habilitaDadosTapete();

        var values = grid.panel.module.form.getValues();
        var tapetesStore = grid.panel.module.tapetesStore;
            
        grid.panel.module.createServicoWindow({
            record:record, 
            edit: true,
            servicosEspecificosStore: grid.panel.module.servicosEspecificosStore,
            codigoTapete: values.codigoTapete,
            nomeTapete: tapetesStore.getAt(tapetesStore.find('codigo', values.codigoTapete)).get('nome'),
            comprimento: values.comprimento,
            largura: values.largura,
            codigoTipoDeCliente: grid.panel.module.options.targetModule.cliente.codigoTipoDeCliente,
            nomeTipoDeCliente: grid.panel.module.options.targetModule.cliente.nomeTipoDeCliente
        });

        // desabilita os campos para o usuario nao alterar os valores
        grid.panel.module.desabilitaDadosTapete();
    },

    onEditServicoClick: function(btn, eventObject, options) {
        var grid = btn.scope.grid;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelServicoClick: function (btn, event, options) {
        var sm = btn.scope.grid.getSelectionModel();
        var servico = sm.getSelection()[0].data;
        var controller = this;

        Ext.Msg.show({
            title: 'Excluir servi&ccedil;o',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Servi&ccedil;o: ' + servico.nomeServico + '<br />Valor: ' + servico.valor,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    btn.scope.grid.getStore().remove(sm.getSelection());
                    controller.onServicosWindowDestroy(btn.up('window'));
                }
            },
            icon: Ext.Msg.QUESTION
        });
    },

    calcularArea: function( field, event, opts ) {
        var comprimento = field.module.comprimento.getValue();
        var largura = field.module.largura.getValue();

        if( comprimento == null ) {
            comprimento = 0;
        }
        if( largura == null ) {
            largura = 0;
        }
        field.module.area.setValue(comprimento * largura);
    },

    onValorEspecificoSelect: function( combo, records, opts ) {
        var store = combo.scope.servicosEspecificosStore;
        var servico = store.getAt(store.find('codigo', combo.getValue() )).data;
        combo.scope.optionsServicoPanel.servicoSelecionado = servico;

        var valorInicial = combo.scope.formServicos.down('#servicoValInicial-itemOS');
        var valorAcima10m2 = combo.scope.formServicos.down('#servicoValAcima10m2-itemOS');
        var cobradoPor = combo.scope.formServicos.down('#txtCobradoPor-itemOS');
        var qtdMm2 = combo.scope.formServicos.down('#qtdMm2-itemOS');
        var valorFinal = combo.scope.formServicos.down('#servicoValFinal-itemOS');


        valorInicial.enable();
        valorInicial.setValue(servico.valor);

        cobradoPor.enable();
        cobradoPor.setValue(servico.nomeCobradoPor);
        
        qtdMm2.enable();
        valorFinal.enable();

        // se servico cobrado por metroQuadrado
        if(servico.codigoCobradoPor == 2) {
            var comp = combo.scope.optionsServicoPanel.comprimento;
            var larg = combo.scope.optionsServicoPanel.largura;
            
            valorAcima10m2.enable();
            valorAcima10m2.setValue(servico.valorAcima10m2);
            qtdMm2.setValue(comp * larg);
            combo.scope.calcularValorServico(servico);
        } else {
            valorAcima10m2.setValue(0);
            valorAcima10m2.disable();

            qtdMm2.reset();
            valorFinal.reset();
        }
    },

    onQtdMm2KeyUp: function( field, event, opts ) {
        field.module.calcularValorServico(field.module.optionsServicoPanel.servicoSelecionado);
    },

    confirmServicoOSClick: function( btn, event, options ) {
        
        if(!btn.scope.formServicos.getForm().isValid() ) {
            genericErrorAlert("Erro", "Dados inv&aacute;lidos, passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        }

        var values = btn.scope.formServicos.getValues();
        var store = btn.scope.servicosEspecificosStore;
        var servico = store.getAt(store.find('codigo', values.codigoServico )).data;
        
        
        if(btn.scope.optionsServicoPanel.edit) {

            btn.scope.optionsServicoPanel.record.set({
                codigoServico: servico.codigo,
                nomeServico: servico.nome,
                quantidade_m_m2: values.quantidade_m_m2,
                valor: values.valor,
                servico: servico
            });
        
        } else {
            var record = Ext.ModelManager.create({
                codigo: 0,
                codigoItem: 0,
                codigoServico: servico.codigo,
                nomeServico: servico.nome,
                quantidade_m_m2: values.quantidade_m_m2,
                valor: values.valor,
                servico: servico
            }, 'App.model.ordensDeServico.ItemServicoModel');

            btn.scope.grid.getStore().add(record);
        }
        
        btn.up('window').close();
    },

    onServicosWindowDestroy: function( component, opts ) {
        // habilita os campos para poder recuperar os valores
        if(component.module.grid.getStore().getCount() <= 0) {
            component.module.form.down('#cboTapetes-itensOS').enable();
            component.module.comprimento.enable();
            component.module.largura.enable();
            component.module.area.enable();
        }
    }
});