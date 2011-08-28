
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
        grid.panel.module.app.getModule("win-servicos-add").createWindow({record:record});
    },

});