
Ext.define('App.controller.ordensDeServico.ItensController', {
    extend: 'Ext.app.Controller',

    models: ['ordensDeServico.ItemModel','ordensDeServico.ItemServicoModel','servicos.ServicoEspecificoModel'],

    views: ['ordensDeServico.ItensView'],

    stores: ['ordensDeServico.ItensStore','ordensDeServico.ItensServicosStore','servicos.ServicosEspecificosStore'],

    init: function () {
        this.control({
            '#': {

            }
        });
    },
    

});