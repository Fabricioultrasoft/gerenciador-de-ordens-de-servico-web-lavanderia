
Ext.define('App.store.clientes.ClientesStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.clientes.ClienteModel',
    storeId: 'clientesStore',
    listeners: {
        load: function( store, records, successful, eOpts ) {
            if (records.length == 0) {
                Ext.notification.msg("Consulta de Clientes", "Nenhum registro encontrado!");
            }
        },
        write: function(proxy, operation){
            if (operation.action == 'destroy') {
                if(operation.resultSet.success) {
                    Ext.notification.msg("Exclus&atilde;o de Clientes", "Os registros foram exclu&iacute;dos com sucesso");
                } else {
                    Ext.notification.msg("Exclus&atilde;o de Clientes", operation.resultSet.message.join("<br />"));
                }
            }
        }
    },
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/clientes/ClientesHandler.ashx?action=create',
            read: 'app/handlers/clientes/ClientesHandler.ashx?action=read',
            update: 'app/handlers/clientes/ClientesHandler.ashx?action=update',
            destroy: 'app/handlers/clientes/ClientesHandler.ashx?action=destroy'
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