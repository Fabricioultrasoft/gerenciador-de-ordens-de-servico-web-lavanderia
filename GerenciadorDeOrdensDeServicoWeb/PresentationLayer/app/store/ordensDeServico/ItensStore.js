﻿
Ext.define('App.store.ordensDeServico.ItensStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.ordensDeServico.ItemModel',
    storeId: 'itensStore',
    listeners: {
        load: function( store, records, successful, eOpts ) {
            if (records.length == 0) {
                Ext.notification.msg("Consulta de Itens de OS", "Nenhum registro encontrado!");
            }
        },
        write: function(proxy, operation){
            if (operation.action == 'destroy') {
                if(operation.resultSet.success) {
                    Ext.notification.msg("Exclus&atilde;o de Itens de OS", "Os registros foram exclu&iacute;dos com sucesso");
                } else {
                    Ext.notification.msg("Exclus&atilde;o de itens de OS", operation.resultSet.message.join("<br />"));
                }
            }
        }
    },
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/ordensDeServico/ItensHandler.ashx?action=create',
            read: 'app/handlers/ordensDeServico/ItensHandler.ashx?action=read',
            update: 'app/handlers/ordensDeServico/ItensHandler.ashx?action=update',
            destroy: 'app/handlers/ordensDeServico/ItensHandler.ashx?action=destroy'
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