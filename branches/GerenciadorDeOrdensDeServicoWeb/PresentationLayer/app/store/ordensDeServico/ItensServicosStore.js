
Ext.define('App.store.ordensDeServico.ItensServicosStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.ordensDeServico.ItemServicoModel',
    storeId: 'itensServicosStore'
});