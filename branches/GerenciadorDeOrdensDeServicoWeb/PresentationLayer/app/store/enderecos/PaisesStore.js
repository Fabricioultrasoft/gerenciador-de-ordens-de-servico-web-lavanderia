
Ext.define('App.store.enderecos.PaisesStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.enderecos.PaisModel',
    storeId: 'paisesStore',
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/enderecos/PaisesHandler.ashx?action=create',
            read: 'app/handlers/enderecos/PaisesHandler.ashx?action=read',
            update: 'app/handlers/enderecos/PaisesHandler.ashx?action=update',
            destroy: 'app/handlers/enderecos/PaisesHandler.ashx?action=destroy'
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