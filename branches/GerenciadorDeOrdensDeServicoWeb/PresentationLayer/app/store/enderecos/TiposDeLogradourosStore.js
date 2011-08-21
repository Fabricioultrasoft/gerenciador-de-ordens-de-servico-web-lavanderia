
Ext.define('App.store.enderecos.TiposDeLogradourosStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.enderecos.TipoDeLogradouroModel',
    storeId: 'tiposDeLogradourosStore',
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/enderecos/TiposDeLogradourosHandler.ashx?action=create',
            read: 'app/handlers/enderecos/TiposDeLogradourosHandler.ashx?action=read',
            update: 'app/handlers/enderecos/TiposDeLogradourosHandler.ashx?action=update',
            destroy: 'app/handlers/enderecos/TiposDeLogradourosHandler.ashx?action=destroy'
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