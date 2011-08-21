
Ext.define('App.store.enderecos.LogradourosStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.enderecos.LogradouroModel',
    storeId: 'logradourosStore',
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/enderecos/LogradourosHandler.ashx?action=create',
            read: 'app/handlers/enderecos/LogradourosHandler.ashx?action=read',
            update: 'app/handlers/enderecos/LogradourosHandler.ashx?action=update',
            destroy: 'app/handlers/enderecos/LogradourosHandler.ashx?action=destroy'
        },
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