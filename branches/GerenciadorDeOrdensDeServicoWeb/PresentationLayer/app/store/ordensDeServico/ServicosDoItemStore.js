
Ext.define('App.store.ordensDeServico.ServicosDoItemStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.ordensDeServico.ServicoDoItemModel',
    storeId: 'itensServicosStore'
});