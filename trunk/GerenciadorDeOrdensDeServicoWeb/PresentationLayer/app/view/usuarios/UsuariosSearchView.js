
Ext.define('App.view.usuarios.UsuariosSearchView', {
    extend: 'App.webDesktop.Module',
    requires: ['Ext.String'],
    id: 'module-usuarios-search',
    init: function () {
    },

    createWindow: function () {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-usuarios-search');
        if (!win) {
            var usuariosPanel = this.createPanel();
            win = desktop.createWindow({
                id: 'win-usuarios-search',
                title: 'Consultar Usuários',
                width: 400,
                height: 300,
                iconCls: 'user-search',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [usuariosPanel]
            });
        }
        win.show();
        return win;
    },

    createPanel: function () {

        var usuariosStore = Ext.create('App.store.usuarios.UsuariosStore', {});
        usuariosStore.load({ params: { ativo: true} });
        this.usuariosStore = usuariosStore;
        this.usuariosStore.module = this;

        var gridUsuarios = Ext.create('Ext.grid.Panel',{
            id: 'grid-usuarios',
            region: 'center',
            store: usuariosStore,
            columns: [
                { header: 'Cod', dataIndex: 'codigo', width: 50, xtype: 'numbercolumn', format: '0'}, 
                { header: 'Nome', dataIndex: 'nome', minWidth: 150, flex: 1, renderer: Ext.String.htmlEncode },
                { header: 'Senha', dataIndex: 'senha', minWidth: 150, flex: 2 }
            ],
            tbar: [
                { itemId: 'btnAddUsuario', text: 'Adicionar', iconCls: 'user-add', scope: this, handler : function() { this.app.getModule("module-usuarios-add").createWindow({store:usuariosStore}); } },
                { itemId: 'btnEditUsuario', text: 'Editar', iconCls: 'user-edit', scope: this, disabled: true },
                { itemId: 'btnDelUsuario', text: 'Remover', iconCls: 'user-del', scope: this, disabled: true } 
            ],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: usuariosStore,
                displayInfo: true,
                displayMsg: 'usu&aacute;rios {0} - {1} de {2}',
                emptyMsg: "Nenhum usu&aacute;rio"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    gridUsuarios.down('#btnEditUsuario').setDisabled(!records.length);
                    gridUsuarios.down('#btnDelUsuario').setDisabled(!records.length);
                }
            }
        });
        this.gridUsuarios = gridUsuarios;
        this.gridUsuarios.module = this;
        //--------------------------------------------------------------------
        var mainPanel = Ext.create('Ext.panel.Panel', {
            border: false,
            layout: 'border',
            items: [gridUsuarios]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    }
});
