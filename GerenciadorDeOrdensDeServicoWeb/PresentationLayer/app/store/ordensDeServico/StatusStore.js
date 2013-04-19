
Ext.define('App.store.ordensDeServico.StatusStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.ordensDeServico.StatusModel',
    storeId: 'statusStore',
    proxy: {
        type: 'ajax',
        api: { read: 'app/handlers/ordensDeServico/OrdensDeServicoHandler.ashx?action=readStatus' },
        reader: {
            type: 'json',
            root: 'data',
            successProperty: 'success',
            messageProperty: 'message',
            totalProperty: 'total'
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