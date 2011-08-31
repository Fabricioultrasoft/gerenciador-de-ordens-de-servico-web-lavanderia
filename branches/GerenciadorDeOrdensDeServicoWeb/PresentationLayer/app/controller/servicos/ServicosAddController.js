
Ext.define('App.controller.servicos.ServicosAddController', {
    extend: 'Ext.app.Controller',

    models: ['servicos.ServicoModel','servicos.ValorServicoModel'],

    views: ['servicos.ServicosAddView'],

    stores: ['servicos.ServicosStore'],

    init: function () {
        this.control({
            '#grid-servicos': {
                itemdblclick: this.editServico
            },
            '#addServicoFlgValorUnico': {
                change: this.onCheckValorUnico
            }
        });
    },

    onCheckValorUnico: function ( checkbox, newValue, oldValue, eOpts ) {
        var valUnico = checkbox.scope.formServico.down("#addServicoValorUnico");

        if(newValue) {
            valUnico.enable();
            valUnico.setValue(0);
            checkbox.scope.gridValoresServico.disable();
        } else {
            valUnico.disable();
            valUnico.reset();
            checkbox.scope.gridValoresServico.enable();
        }
    },

});