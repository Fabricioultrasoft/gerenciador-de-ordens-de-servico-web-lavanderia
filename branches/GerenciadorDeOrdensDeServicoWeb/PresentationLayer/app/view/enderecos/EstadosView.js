
Ext.define('App.view.enderecos.EstadosView', {
    extend: 'Ext.panel.Panel',
    requires: ['Ext.grid.*', 'Ext.data.*', 'Ext.util.*', 'Ext.state.*', 'Ext.form.*'],
    alias: 'widget.estadosview',
    border: false,
    layout: 'fit',
    initComponent: function () {
        var me = this;

        var estadosStore = Ext.create('App.store.enderecos.EstadosStore',{});

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-estados',
            border: false,
            autoScroll: true,
            store: estadosStore,
            columns: [{
                xtype: 'numbercolumn',
                header: 'Cod',
                dataIndex: 'codigo',
                format: '0'
            }, {
                header: 'Estado',
                dataIndex: 'nome',
                flex: 1
            }, {
                header: 'Cod Pa&iacute;s',
                dataIndex: 'codigoPais',
                hidden: true
            }, {
                header: 'Pa&iacute;s',
                dataIndex: 'nomePais'
            }],
            tbar: [
                { itemId: 'btnAddEstado', text: 'Adicionar', iconCls: 'btn-add', scope: me }, 
                { itemId: 'btnEditEstado', text: 'Editar', iconCls: 'edit', scope: this, disabled: true },
                { itemId: 'btnDelEstado', text: 'Remover', iconCls: 'btn-del', disabled: true, scope: me }
            ],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: estadosStore,
                displayInfo: true,
                displayMsg: 'Estados {0} - {1} de {2}',
                emptyMsg: "Nenhum estado"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditEstado').setDisabled(!records.length);
                    grid.down('#btnDelEstado').setDisabled(!records.length);
                }
            }
        });

        grid.getStore().load();

        grid.parent = me;
        me.grid = grid;

        this.callParent(arguments);

        this.add(grid);
    },

    createWinAddEstado: function () {
        var addEstadoWin = Ext.ComponentManager.get('win-add-estado');
        if (!addEstadoWin) {

            var paisStore = Ext.create('App.store.enderecos.PaisesStore', {pageSize:0});
            paisStore.load();

            addEstadoWin = Ext.create('widget.window', {
                title: 'Adicionar Estado',
                layout: 'fit',
                id: 'win-add-estado',
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
                            xtype: 'combo',
                            id: 'win-add-estado-combo-pais',
                            name: 'codigoPais',
                            store: paisStore,
                            fieldLabel: 'Pa&iacute;s',
                            emptyText: 'Selecione o país',
                            displayField: 'nome',
                            valueField: 'codigo',
                            allowBlank: false,
                            blankText: 'Um Pa&iacute;s deve ser selecionado',
                            typeAhead: true,
                            queryMode: 'local',
                            triggerAction: 'all',
                            selectOnFocus: true,
                            forceSelection: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'nome',
                            fieldLabel: 'Estado'
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
                        scope: addEstadoWin,
                        handler: function () { addEstadoWin.close(); }
                    }
                ]
            });
        }

        return addEstadoWin;
    },

    createWinEditEstado: function () {
        var editEstadoWin = Ext.ComponentManager.get('win-edit-estado');
        if (!editEstadoWin) {

            var paisStore = Ext.create('App.store.enderecos.PaisesStore', {pageSize:0});
            paisStore.load();

            editEstadoWin = Ext.create('widget.window', {
                title: 'Editar Estado',
                layout: 'fit',
                id: 'win-edit-estado',
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
                            xtype: 'combo',
                            id: 'win-edit-estado-combo-pais',
                            name: 'codigoPais',
                            autoSelect: true,
                            store: paisStore,
                            fieldLabel: 'Pa&iacute;s',
                            emptyText: 'Selecione o país',
                            displayField: 'nome',
                            valueField: 'codigo',
                            allowBlank: false,
                            blankText: 'Um Pa&iacute;s deve ser selecionado',
                            typeAhead: true,
                            queryMode: 'local',
                            selectOnFocus: true,
                            forceSelection: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'nome',
                            fieldLabel: 'Estado'
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
                        scope: editEstadoWin,
                        handler: function () { editEstadoWin.close(); }
                    }
                ]
            });
        }

        return editEstadoWin;
    }
});