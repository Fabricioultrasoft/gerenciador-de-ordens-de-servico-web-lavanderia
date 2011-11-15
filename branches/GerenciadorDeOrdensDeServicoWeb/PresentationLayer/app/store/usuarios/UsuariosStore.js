
Ext.define('App.store.usuarios.UsuariosStore', {
    extend: 'Ext.data.Store',
    model: 'App.model.usuarios.UsuarioModel',
    storeId: 'usuariosStore',
    listeners: {
        load: function( store, records, successful, eOpts ) {
            if (records.length == 0) {
                Ext.notification.msg("Consulta de Usu&aacute;rios", "Nenhum registro encontrado!");
            }
        },
        write: function(proxy, operation){
            if (operation.action == 'destroy') {
                if(operation.resultSet.success) {
                    Ext.notification.msg("Exclus&atilde;o de Usu&aacute;rios", "Os registros foram exclu&iacute;dos com sucesso");
                } else {
                    Ext.notification.msg("Exclus&atilde;o de Usu&aacute;rios", operation.resultSet.message.join("<br />"));
                }
            }
        }
    },
    proxy: {
        type: 'ajax',
        api: {
            create: 'app/handlers/usuarios/UsuariosHandler.ashx?action=create',
            read: 'app/handlers/usuarios/UsuariosHandler.ashx?action=read',
            update: 'app/handlers/usuarios/UsuariosHandler.ashx?action=update',
            destroy: 'app/handlers/usuarios/UsuariosHandler.ashx?action=destroy'
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