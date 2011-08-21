
Ext.define('App.store.enderecos.EstadosStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.enderecos.EstadoModel',
    storeId: 'estadosStore',
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/enderecos/EstadosHandler.ashx?action=create',
            read: 'app/handlers/enderecos/EstadosHandler.ashx?action=read',
            update: 'app/handlers/enderecos/EstadosHandler.ashx?action=update',
            destroy: 'app/handlers/enderecos/EstadosHandler.ashx?action=destroy'
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