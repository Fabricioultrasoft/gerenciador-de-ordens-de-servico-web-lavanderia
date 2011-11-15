
Ext.define('App.store.enderecos.CidadesStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.enderecos.CidadeModel',
    storeId: 'cidadesStore',
    listeners: {
        load: function( store, records, successful, eOpts ) {
            if (records.length == 0) {
                Ext.notification.msg("Consulta de Cidades", "Nenhum registro encontrado!");
            }
        },
        write: function(proxy, operation){
            if (operation.action == 'destroy') {
                if(operation.resultSet.success) {
                    Ext.notification.msg("Exclus&atilde;o de Cidades", "Os registros foram exclu&iacute;dos com sucesso");
                } else {
                    Ext.notification.msg("Exclus&atilde;o de Cidades", operation.resultSet.message.join("<br />"));
                }
            }
        }
    },
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