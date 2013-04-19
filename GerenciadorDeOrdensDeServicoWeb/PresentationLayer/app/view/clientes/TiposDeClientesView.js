
Ext.define('App.view.clientes.TiposDeClientesView', {
    extend: 'App.webDesktop.Module',
    id: 'module-tiposDeClientes',
    init: function () {

        this.launcher = {
            text: 'Tipos de Clientes',
            iconCls: 'clientes-tipos',
            handler: this.createWindow,
            scope: this
        };
    },

    createWindow: function (options) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-tiposDeClientes');
        if (!win) {
            var panel = this.createPanel(options);
            win = desktop.createWindow({
                id: 'win-tiposDeClientes',
                title: 'Tipos de Clientes',
                width: 420,
                height: 300,
                iconCls: 'clientes-tipos-search',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [panel]
            });
        }
        win.show();
        return win;
    },

    createPanel: function (options) {
        this.options = options;
        var me = this;
        var tiposDeClientesStore = Ext.create('App.store.clientes.TiposDeClientesStore',{});
        tiposDeClientesStore.load();

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-tiposDeClientes',
            store: tiposDeClientesStore,
            columns: [
                { xtype: 'numbercolumn', header: 'Cod', dataIndex: 'codigo', format: '0' }, 
                { header: 'Nome', dataIndex: 'nome', flex: 1 },
                { header: 'Ativo', dataIndex: 'ativo', xtype: 'booleancolumn'}
            ],
            tbar: [{
                itemId: 'btnAddTipoDeCliente',
                text: 'Adicionar',
                iconCls: 'clientes-tipos-add',
                scope: me
            }, {
                itemId: 'btnEditTipoDeCliente',
                text: 'Editar',
                iconCls: 'clientes-tipos-edit',
                disabled: true,
                scope: me
            }, {
                itemId: 'btnDelTipoDeCliente',
                text: 'Remover',
                iconCls: 'clientes-tipos-del',
                disabled: true,
                scope: me
            }],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: tiposDeClientesStore,
                displayInfo: true,
                displayMsg: 'Tipos de Clientes {0} - {1} de {2}',
                emptyMsg: "Nenhum Tipo de cliente"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditTipoDeCliente').setDisabled(!records.length);
                    grid.down('#btnDelTipoDeCliente').setDisabled(!records.length);
                }
            }
        });
        grid.module = me;
        me.grid = grid;

        return grid;
    },

    createWinAddTipoDeCliente: function () {
        var win = Ext.ComponentManager.get('win-tiposDeClientes-add');
        if (!win) {
            win = Ext.create('widget.window', {
                title: 'Adicionar Tipo de Cliente',
                layout: 'fit',
                id: 'win-tiposDeClientes-add',
                iconCls: 'clientes-tipos-add',
                width: 230,
                modal: true,
                resizable: false,
                items: [{
                    xtype: 'form',
                    border: false,
                    bodyPadding: 5,
                    fieldDefaults: {
                        labelAlign: 'left',
                        labelWidth: 50
                    },
                    defaults: {
                        allowBlank: false, 
                        blankText: 'Este campo é obrigatório'
                    },
                    items: [ { xtype: 'textfield', name: 'nome', fieldLabel: 'Nome' } ]
                }],
                buttons: [
                    { text: 'Salvar', action: 'save', scope: this },
                    { text: 'Cancelar', scope: win, handler: function () { win.close(); } }
                ]
            });
        }

        return win;
    },

    createWinEditTipoDeCliente: function () {
        var win = Ext.ComponentManager.get('win-tiposDeClientes-edit');
        if (!win) {
            win = Ext.create('widget.window', {
                title: 'Editar Tipo de Cliente',
                layout: 'fit',
                id: 'win-tiposDeClientes-edit',
                iconCls: 'clientes-tipos-edit',
                width: 230,
                height: 150,
                modal: true,
                resizable: false,
                items: [{
                    xtype: 'form',
                    border: false,
                    bodyPadding: 5,
                    fieldDefaults: {
                        labelAlign: 'left',
                        labelWidth: 50
                    },
                    items: [
                        { xtype: 'textfield', name: 'codigo', fieldLabel: 'C&oacute;digo', readOnly: true },
                        { xtype: 'textfield', name: 'nome', fieldLabel: 'Nome', allowBlank: false, blankText: 'Este campo é obrigatório' },
                        { xtype: 'checkbox', name: 'ativo', fieldLabel: 'Ativo', inputValue: 1}
                    ]
                }],
                buttons: [
                    { text: 'Salvar', action: 'save', scope: this },
                    { text: 'Cancelar', scope: win, handler: function () { win.close(); } }
                ]
            });
        }

        return win;
    }
});