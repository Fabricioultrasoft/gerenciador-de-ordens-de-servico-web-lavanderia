
Ext.define('App.store.usuarios.UsuariosStore', {
    extend: 'Ext.data.Store',
    model: 'App.model.usuarios.UsuarioModel',
    storeId: 'usuariosStore',
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