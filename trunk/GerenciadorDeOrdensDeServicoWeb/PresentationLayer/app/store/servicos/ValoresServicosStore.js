
Ext.define('App.store.servicos.ValoresServicosStore', {
    extend: 'Ext.data.TreeStore',
    autoDestroy: false,
    autoLoad: false,
    model: 'App.model.servicos.ValorServicoModel',
    storeId: 'valoresServicosStore',
    listeners: {
        load: function( store, records, successful, eOpts ) {
            if (records.length == 0) {
                Ext.notification.msg("Consulta de Valores de Servi&ccedil;os", "Nenhum registro encontrado!");
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
            root: 'children',
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