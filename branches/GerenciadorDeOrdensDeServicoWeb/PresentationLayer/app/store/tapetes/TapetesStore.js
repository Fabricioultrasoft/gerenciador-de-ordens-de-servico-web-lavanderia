
Ext.define('App.store.tapetes.TapetesStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.tapetes.TapeteModel',
    storeId: 'tapetesStore',
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/tapetes/TapetesHandler.ashx?action=create',
            read: 'app/handlers/tapetes/TapetesHandler.ashx?action=read',
            update: 'app/handlers/tapetes/TapetesHandler.ashx?action=update',
            destroy: 'app/handlers/tapetes/TapetesHandler.ashx?action=destroy'
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