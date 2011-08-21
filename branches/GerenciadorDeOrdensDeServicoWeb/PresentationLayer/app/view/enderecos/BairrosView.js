
Ext.define('App.view.enderecos.BairrosView', {
    extend: 'Ext.panel.Panel',
    requires: ['Ext.grid.*', 'Ext.data.*', 'Ext.util.*', 'Ext.state.*', 'Ext.form.*'],
    alias: 'widget.bairrosview',
    border: false,
    layout: 'fit',
    initComponent: function () {
        var me = this;

        var bairrosStore = Ext.create('App.store.enderecos.BairrosStore',{});

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-bairros',
            border: false,
            autoScroll: true,
            store: bairrosStore,
            columns: [{
                xtype: 'numbercolumn',
                header: 'Cod',
                dataIndex: 'codigo',
                format: '0'
            }, {
                header: 'Bairro',
                dataIndex: 'nome',
                flex: 1
            }, {
                header: 'Cod Cidade',
                dataIndex: 'codigoCidade',
                hidden: true
            }, {
                header: 'Cidade',
                dataIndex: 'nomeCidade'
            },
            {
                header: 'Cod Estado',
                dataIndex: 'codigoEstado',
                hidden: true
            }, {
                header: 'Estado',
                dataIndex: 'nomeEstado'
            },
            {
                header: 'Cod Pa&iacute;s',
                dataIndex: 'codigoPais',
                hidden: true
            }, {
                header: 'Pa&iacute;s',
                dataIndex: 'nomePais'
            }],
            tbar: [{
                itemId: 'btnAddBairro',
                text: 'Adicionar',
                iconCls: 'btn-add',
                scope: me
            }, {
                itemId: 'btnDelBairro',
                text: 'Remover',
                iconCls: 'btn-del',
                disabled: true,
                scope: me
            }],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: bairrosStore,
                displayInfo: true,
                displayMsg: 'Bairros {0} - {1} of {2}',
                emptyMsg: "Nenhum bairro"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnDelBairro').setDisabled(!records.length);
                }
            }
        });

        grid.getStore().load();

        grid.parent = me;
        me.grid = grid;

        this.callParent(arguments);

        this.add(grid);
    },

    createWinAddBairro: function () {
        var addBairroWin = Ext.ComponentManager.get('win-add-bairro');
        if (!addBairroWin) {

            var paisesStore = Ext.create('App.store.enderecos.PaisesStore', {pageSize:0});
            var estadosStore = Ext.create('App.store.enderecos.EstadosStore', {pageSize:0});
            var cidadesStore = Ext.create('App.store.enderecos.CidadesStore', {pageSize:0});
            
            paisesStore.load();

            addBairroWin = Ext.create('widget.window', {
                title: 'Adicionar Bairro',
                layout: 'fit',
                id: 'win-add-bairro',
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
                            id: 'win-add-bairro-combo-pais',
                            name: 'codigoPais',
                            store: paisesStore,
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
                            xtype: 'combo',
                            id: 'win-add-bairro-combo-estado',
                            name: 'codigoEstado',
                            store: estadosStore,
                            fieldLabel: 'Estado',
                            emptyText: 'Selecione o estado',
                            displayField: 'nome',
                            valueField: 'codigo',
                            allowBlank: false,
                            blankText: 'Um Estado deve ser selecionado',
                            typeAhead: true,
                            queryMode: 'local',
                            triggerAction: 'all',
                            selectOnFocus: true,
                            forceSelection: true
                        },
                        {
                            xtype: 'combo',
                            id: 'win-add-bairro-combo-cidade',
                            name: 'codigoCidade',
                            store: cidadesStore,
                            fieldLabel: 'Cidade',
                            emptyText: 'Selecione a cidade',
                            displayField: 'nome',
                            valueField: 'codigo',
                            allowBlank: false,
                            blankText: 'Uma Cidade deve ser selecionada',
                            typeAhead: true,
                            queryMode: 'local',
                            triggerAction: 'all',
                            selectOnFocus: true,
                            forceSelection: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'nome',
                            fieldLabel: 'Bairro'
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
                        scope: addBairroWin,
                        handler: function () { addBairroWin.close(); }
                    }
                ]
            });
        }

        return addBairroWin;
    },

    createWinEditBairro: function () {
        var editBairroWin = Ext.ComponentManager.get('win-edit-bairro');
        if (!editBairroWin) {

            var cidadesStore = Ext.create('App.store.enderecos.CidadesStore', {pageSize:0});
            
            editBairroWin = Ext.create('widget.window', {
                title: 'Editar Bairro',
                layout: 'fit',
                id: 'win-edit-bairro',
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
                            name: 'nomePais',
                            fieldLabel: 'Pa&iacute;s',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'nomeEstado',
                            fieldLabel: 'Estado',
                            readOnly: true
                        },
                        {
                            xtype: 'combo',
                            id: 'win-edit-bairro-combo-cidade',
                            name: 'codigoCidade',
                            autoSelect: true,
                            store: cidadesStore,
                            fieldLabel: 'Cidade',
                            emptyText: 'Selecione a cidade',
                            displayField: 'nome',
                            valueField: 'codigo',
                            allowBlank: false,
                            blankText: 'Uma Cidade deve ser selecionada',
                            typeAhead: true,
                            queryMode: 'local',
                            selectOnFocus: true,
                            forceSelection: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'nome',
                            fieldLabel: 'Bairro'
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
                        scope: editBairroWin,
                        handler: function () { editBairroWin.close(); }
                    }
                ]
            });
        }

        return editBairroWin;
    }
});