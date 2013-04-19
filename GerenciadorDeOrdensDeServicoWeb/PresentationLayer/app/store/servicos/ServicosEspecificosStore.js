
Ext.define('App.store.servicos.ServicosEspecificosStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.servicos.ServicoEspecificoModel',
    storeId: 'servicosEspecificosStore',
    proxy: {
        type: 'ajax',
        api: { read: 'app/handlers/servicos/ServicosHandler.ashx?action=readEspecificos' },
        reader: {
            type: 'json',
            root: 'data',
            successProperty: 'success',
            messageProperty: 'message',
            totalProperty: 'total'
        },
        writer: {
            allowSingle: false,
            encode: true,
            root: 'records'
        },
        listeners: {
            exception: {
                element: this,
                fn: function(thisProxy, response, operation, options) {
                    genericExceptionHandler(thisProxy, response, operation, options);
                }
            }
        }
    }
});