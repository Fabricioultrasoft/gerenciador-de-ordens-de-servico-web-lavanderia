
Ext.define('App.store.enderecos.BairrosStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.enderecos.BairroModel',
    storeId: 'bairrosStore',
    listeners: {
        load: function( store, records, successful, eOpts ) {
            if (records.length == 0) {
                Ext.notification.msg("Consulta de Bairros", "Nenhum registro encontrado!");
            }
        },
        write: function(proxy, operation){
            if (operation.action == 'destroy') {
                if(operation.resultSet.success) {
                    Ext.notification.msg("Exclus&atilde;o de Bairros", "Os registros foram exclu&iacute;dos com sucesso");
                } else {
                    Ext.notification.msg("Exclus&atilde;o de Bairros", operation.resultSet.message.join("<br />"));
                }
            }
        }
    },
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/enderecos/BairrosHandler.ashx?action=create',
            read: 'app/handlers/enderecos/BairrosHandler.ashx?action=read',
            update: 'app/handlers/enderecos/BairrosHandler.ashx?action=update',
            destroy: 'app/handlers/enderecos/BairrosHandler.ashx?action=destroy'
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