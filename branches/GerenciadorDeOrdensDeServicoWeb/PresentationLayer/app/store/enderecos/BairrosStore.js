﻿
Ext.define('App.store.enderecos.BairrosStore', {
    extend: 'Ext.data.Store',
    autoDestroy: false,
    model: 'App.model.enderecos.BairroModel',
    storeId: 'bairrosStore',
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