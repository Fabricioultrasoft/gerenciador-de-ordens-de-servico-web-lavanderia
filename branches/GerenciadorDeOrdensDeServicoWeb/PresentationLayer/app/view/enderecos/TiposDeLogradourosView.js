
Ext.define('App.view.enderecos.TiposDeLogradourosView', {
    extend: 'Ext.panel.Panel',
    requires: ['Ext.grid.*', 'Ext.data.*', 'Ext.util.*', 'Ext.state.*', 'Ext.form.*'],
    alias: 'widget.tiposdelogradourosview',
    border: false,
    layout: 'fit',
    initComponent: function () {
        var me = this;

        var tiposDeLogradourosStore = Ext.create('App.store.enderecos.TiposDeLogradourosStore',{});
        this.tiposDeLogradourosStore = tiposDeLogradourosStore;
        this.tiposDeLogradourosStore.module = this;

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-tipos-de-logradouros',
            border: false,
            store: tiposDeLogradourosStore,
            columns: [{
                xtype: 'numbercolumn',
                header: 'Cod',
                dataIndex: 'codigo',
                format: '0'
            }, {
                header: 'Tipo de Logradouro',
                dataIndex: 'nome',
                flex: 1
            }],
            tbar: [
                { itemId: 'btnAddTipoDeLogradouro', text: 'Adicionar', iconCls: 'btn-add', scope: me }, 
                { itemId: 'btnEditTipoDeLogradouro', text: 'Editar', iconCls: 'edit', scope: this, disabled: true },
                { itemId: 'btnDelTipoDeLogradouro', text: 'Remover', iconCls: 'btn-del', disabled: true, scope: me }
            ],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: tiposDeLogradourosStore,
                displayInfo: true,
                displayMsg: 'Tipos de Logradouros {0} - {1} de {2}',
                emptyMsg: "Nenhum tipo de logradouro"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditTipoDeLogradouro').setDisabled(!records.length);
                    grid.down('#btnDelTipoDeLogradouro').setDisabled(!records.length);
                }
            }
        });

        grid.getStore().load();
        grid.parent = me;
        me.grid = grid;

        this.callParent(arguments);

        this.add(grid);
    },

    createWinAddTipoDeLogradouro: function () {
        var addTipoDelogradouroWin = Ext.ComponentManager.get('win-add-tipo-de-logradouro');
        if (!addTipoDelogradouroWin) {
            addTipoDelogradouroWin = Ext.create('widget.window', {
                title: 'Adicionar Tipo de Logradouro',
                layout: 'fit',
                id: 'win-add-tipo-de-logradouro',
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
                        scope: addTipoDelogradouroWin,
                        handler: function () { addTipoDelogradouroWin.close(); }
                    }
                ]
            });
        }

        return addTipoDelogradouroWin;
    },

    createWinEditTipoDeLogradouro: function () {
        var editTipoDelogradouroWin = Ext.ComponentManager.get('win-edit-tipo-de-logradouro');
        if (!editTipoDelogradouroWin) {
            editTipoDelogradouroWin = Ext.create('widget.window', {
                title: 'Editar Tipo de Logradouro',
                layout: 'fit',
                id: 'win-edit-tipo-de-logradouro',
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
                        scope: editTipoDelogradouroWin,
                        handler: function () { editTipoDelogradouroWin.close(); }
                    }
                ]
            });
        }

        return editTipoDelogradouroWin;
    }
});