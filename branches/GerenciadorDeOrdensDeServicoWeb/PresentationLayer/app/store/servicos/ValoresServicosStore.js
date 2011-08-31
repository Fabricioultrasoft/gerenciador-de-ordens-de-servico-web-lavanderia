
Ext.define('App.store.servicos.ValoresServicosStore', {
    extend: 'Ext.data.TreeStore',
    autoDestroy: false,
    model: 'App.model.servicos.ValorServicoModel',
    storeId: 'servicosStore',
    listeners: {
        write: function(proxy, operation){
            if (operation.action == 'destroy') {
                if(operation.resultSet.success) {
                    Ext.notification.msg("Exclus&atilde;o de Servi&ccedil;o", "Os registros foram exclu&iacute;dos com sucesso");
                } else {
                    Ext.notification.msg("Exclus&atilde;o de Servi&ccedil;o", operation.resultSet.message.join("<br />"));
                }
            }
        }
    },
    proxy: {
        type: 'ajax',
        api: {
            read: 'app/handlers/servicos/ServicosHandler.ashx?action=readServico'
        },
        reader: {
            type: 'json',
            root: 'valores',
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