
Ext.define('App.controller.usuarios.UsuariosSearchController', {
    extend: 'Ext.app.Controller',

    models: ['usuarios.UsuarioModel'],

    views: ['usuarios.UsuariosSearchView'],

    stores: ['usuarios.UsuariosStore'],

    init: function () {
        this.control({
            '#grid-usuarios': {
                itemdblclick: this.editUsuario
            },
            '#btnEditUsuario': {
                click: this.onEditUsuarioClick
            },
            '#btnDelUsuario': {
                click: this.onDelUsuarioClick
            }
        });
    },

    editUsuario: function (grid, record) {
        grid.panel.module.app.getModule("module-usuarios-edit").createWindow({record:record});
    },

    onEditUsuarioClick: function(btn, eventObject, options) {
        var grid = btn.scope.gridUsuarios;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelUsuarioClick: function (btn, eventObject, options) {
        var sm = btn.scope.gridUsuarios.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir usu&aacute;rio',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Usu&aacute;rio: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    btn.scope.gridUsuarios.getStore().remove(sm.getSelection());
                    btn.scope.gridUsuarios.getStore().sync();
                    btn.scope.gridUsuarios.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
                    if (btn.scope.gridUsuarios.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            icon: Ext.Msg.QUESTION
        });
    }
});