
Ext.define('App.store.ordensDeServico.OrdensDeServicoStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.ordensDeServico.OrdemDeServicoModel',
    storeId: 'ordensDeServicoStore',
    listeners: {
        write: function(proxy, operation){
            if (operation.action == 'destroy') {
                if(operation.resultSet.success) {
                    Ext.notification.msg("Exclus&atilde;o de Ordem de Servi&ccedil;o", "Os registros foram exclu&iacute;dos com sucesso");
                } else {
                    Ext.notification.msg("Exclus&atilde;o de Ordem de Servi&ccedil;o", operation.resultSet.message.join("<br />"));
                }
            }
            try {
                this.onWriteCallback(proxy, operation);
            } catch(e){}
        }
    },
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/ordensDeServico/OrdensDeServicoHandler.ashx?action=create',
            read: 'app/handlers/ordensDeServico/OrdensDeServicoHandler.ashx?action=read',
            update: 'app/handlers/ordensDeServico/OrdensDeServicoHandler.ashx?action=update',
            destroy: 'app/handlers/ordensDeServico/OrdensDeServicoHandler.ashx?action=destroy'
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
                    try {
                        this.onRequestFailureCallback(thisProxy, response, operation, options);
                    } catch(e){}
                }
            }
        }
    }
});