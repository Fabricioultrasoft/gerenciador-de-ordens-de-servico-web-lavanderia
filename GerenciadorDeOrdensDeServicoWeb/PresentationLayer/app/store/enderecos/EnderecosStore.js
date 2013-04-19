
Ext.define('App.store.enderecos.EnderecosStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.enderecos.EnderecoModel',
    storeId: 'enderecosStore',
    proxy: {
        type: 'ajax',
        api: {
            read: 'app/handlers/enderecos/EnderecosHandler.ashx?action=read'
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