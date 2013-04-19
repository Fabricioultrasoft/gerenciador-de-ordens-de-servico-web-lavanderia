
Ext.define('App.store.clientes.MeiosDeContatoStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.clientes.MeioDeContatoModel',
    storeId: 'meiosDeContatoStore',
    proxy: {
        type: 'ajax',
//        api: {
//            read: 'app/handlers/clientes/MeiosDeContatoHandler.ashx?action=read'
//        },
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