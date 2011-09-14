
Ext.define('App.view.enderecos.PaisesView', {
    extend: 'Ext.panel.Panel',
    requires: ['Ext.grid.*', 'Ext.data.*', 'Ext.util.*', 'Ext.state.*', 'Ext.form.*'],
    alias: 'widget.paisesview',
    border: false,
    layout: 'fit',
    initComponent: function () {
        var me = this;

        var paisesStore = Ext.create('App.store.enderecos.PaisesStore',{});
        this.paisesStore = paisesStore;
        this.paisesStore.module = this;

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-paises',
            border: false,
            store: paisesStore,
            columns: [{
                xtype: 'numbercolumn',
                header: 'Cod',
                dataIndex: 'codigo',
                format: '0'
            }, {
                header: 'Pa&iacute;s',
                dataIndex: 'nome',
                flex: 1
            }],
            tbar: [
                { itemId: 'btnAddPais', text: 'Adicionar', iconCls: 'btn-add', scope: me },
                { itemId: 'btnEditPais', text: 'Editar', iconCls: 'edit', scope: this, disabled: true },
                { itemId: 'btnDelPais', text: 'Remover', iconCls: 'btn-del', disabled: true, scope: me }
            ],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: paisesStore,
                displayInfo: true,
                displayMsg: 'pa&iacute;ses {0} - {1} de {2}',
                emptyMsg: "Nenhum pa&iacute;s"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditPais').setDisabled(!records.length);
                    grid.down('#btnDelPais').setDisabled(!records.length);
                }
            }
        });

        grid.getStore().load();
        grid.parent = me;
        me.grid = grid;

        this.callParent(arguments);

        this.add(grid);
    },

    createWinAddPais: function () {
        var addPaisWin = Ext.ComponentManager.get('win-add-pais');
        if (!addPaisWin) {
            addPaisWin = Ext.create('widget.window', {
                title: 'Adicionar Pa&iacute;s',
                layout: 'fit',
                id: 'win-add-pais',
                modal: true,
                resizable: false,
                items: [{
                    xtype: 'form',
                    border: false,
                    fieldDefaults: {
                        labelAlign: 'left',
                        labelWidth: 40
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'nome',
                            fieldLabel: 'Nome',
                            margin: '2 2 2 2'
                        }
                    ]
                }],
                buttons: [
                    {
                        text: 'Salvar',
                        action: 'save'
                    },
                    {
                        text: 'Cancelar',
                        scope: addPaisWin,
                        handler: function () { addPaisWin.close(); }
                    }
                ]
            });
        }

        return addPaisWin;
    },

    createWinEditPais: function () {
        var editPaisWin = Ext.ComponentManager.get('win-edit-pais');
        if (!editPaisWin) {
            editPaisWin = Ext.create('widget.window', {
                title: 'Editar Pa&iacute;s',
                layout: 'fit',
                id: 'win-edit-pais',
                modal: true,
                resizable: false,
                items: [{
                    xtype: 'form',
                    border: false,
                    fieldDefaults: {
                        labelAlign: 'left',
                        labelWidth: 40,
                        margin: '2 2 2 2'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'codigo',
                            fieldLabel: 'Código',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'nome',
                            fieldLabel: 'Nome'
                        }
                    ]
                }],
                buttons: [
                    {
                        text: 'Salvar',
                        action: 'save'
                    },
                    {
                        text: 'Cancelar',
                        scope: editPaisWin,
                        handler: function () { editPaisWin.close(); }
                    }
                ]
            });
        }

        return editPaisWin;
    }
});