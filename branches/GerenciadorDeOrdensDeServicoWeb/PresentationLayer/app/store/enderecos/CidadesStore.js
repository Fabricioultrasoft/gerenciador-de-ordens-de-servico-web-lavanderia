
Ext.define('App.store.enderecos.CidadesStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.enderecos.CidadeModel',
    storeId: 'cidadesStore',
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/enderecos/CidadesHandler.ashx?action=create',
            read: 'app/handlers/enderecos/CidadesHandler.ashx?action=read',
            update: 'app/handlers/enderecos/CidadesHandler.ashx?action=update',
            destroy: 'app/handlers/enderecos/CidadesHandler.ashx?action=destroy'
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