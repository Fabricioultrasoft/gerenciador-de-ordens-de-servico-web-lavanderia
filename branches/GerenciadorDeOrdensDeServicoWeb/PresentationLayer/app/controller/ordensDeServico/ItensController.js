
Ext.define('App.controller.ordensDeServico.ItensController', {
    extend: 'Ext.app.Controller',

    models: ['ordensDeServico.ItemModel','ordensDeServico.ItemServicoModel','servicos.ServicoEspecificoModel'],

    views: ['ordensDeServico.ItensView'],

    stores: ['ordensDeServico.ItensStore','ordensDeServico.ItensServicosStore','servicos.ServicosEspecificosStore'],

    init: function () {
        this.control({
            '#btnAddServicos-ItemOS': {
                click: this.onAddServicoClick
            },
            '#module-itensOS-comprimento': {
                keyup: this.calcularArea
            },
            '#module-itensOS-largura': {
                keyup: this.calcularArea
            }
        });
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
            var values = btn.scope.form.getValues();
            var tapetesStore = btn.scope.tapetesStore;
            btn.scope.createServicoWindow({
                codigoTapete: values.codigoTapete,
                nomeTapete: tapetesStore.getAt(tapetesStore.find('codigo', values.codigoTapete)).get('nome'),
                comprimento: values.comprimento,
                largura: values.largura,
                codigoTipoDeCliente: btn.scope.options.targetModule.cliente.codigoTipoDeCliente,
                nomeTipoDeCliente: btn.scope.options.targetModule.cliente.nomeTipoDeCliente
            });
        }
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
    }
});