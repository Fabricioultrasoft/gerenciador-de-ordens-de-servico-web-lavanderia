
Ext.define('App.store.clientes.TiposDeClientesStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    storeId: 'tiposDeClientesStore',
    fields: [{name: 'codigo', type: 'int'},{name: 'nome', type: 'string'},{name: 'ativo', type: 'boolean'}],
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/clientes/TiposDeClientesHandler.ashx?action=create',
            read: 'app/handlers/clientes/TiposDeClientesHandler.ashx?action=read',
            update: 'app/handlers/clientes/TiposDeClientesHandler.ashx?action=update',
            destroy: 'app/handlers/clientes/TiposDeClientesHandler.ashx?action=destroy'
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
