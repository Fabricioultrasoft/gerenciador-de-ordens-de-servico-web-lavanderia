
Ext.define('App.store.tapetes.TapetesStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.tapetes.TapeteModel',
    storeId: 'tapetesStore',
    listeners: {
        load: function( store, records, successful, eOpts ) {
            if (records.length == 0) {
                Ext.notification.msg("Consulta de Tapetes", "Nenhum registro encontrado!");
            }
        },
        write: function(proxy, operation){
            if (operation.action == 'destroy') {
                if(operation.resultSet.success) {
                    Ext.notification.msg("Exclus&atilde;o de Tapetes", "Os registros foram exclu&iacute;dos com sucesso");
                } else {
                    Ext.notification.msg("Exclus&atilde;o de Tapetes", operation.resultSet.message.join("<br />"));
                }
            }
        }
    },
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