﻿
Ext.define('App.store.servicos.ServicosStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.servicos.ServicoModel',
    storeId: 'servicosStore',
    listeners: {
        load: function( store, records, successful, eOpts ) {
            if (records.length == 0) {
                Ext.notification.msg("Consulta de Servi&ccedil;os", "Nenhum registro encontrado!");
            }
        },
        write: function(proxy, operation){
            if (operation.action == 'destroy') {
                if(operation.resultSet.success) {
                    Ext.notification.msg("Exclus&atilde;o de Servi&ccedil;os", "Os registros foram exclu&iacute;dos com sucesso");
                } else {
                    Ext.notification.msg("Exclus&atilde;o de Servi&ccedil;os", operation.resultSet.message.join("<br />"));
                }
            }
        }
    },
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/servicos/ServicosHandler.ashx?action=create',
            read: 'app/handlers/servicos/ServicosHandler.ashx?action=read',
            update: 'app/handlers/servicos/ServicosHandler.ashx?action=update',
            destroy: 'app/handlers/servicos/ServicosHandler.ashx?action=destroy'
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