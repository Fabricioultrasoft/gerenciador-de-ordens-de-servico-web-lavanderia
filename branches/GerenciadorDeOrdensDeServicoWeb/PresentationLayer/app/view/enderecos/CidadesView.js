
Ext.define('App.view.enderecos.CidadesView', {
    extend: 'Ext.panel.Panel',
    requires: ['Ext.grid.*', 'Ext.data.*', 'Ext.util.*', 'Ext.state.*', 'Ext.form.*'],
    alias: 'widget.cidadesview',
    border: false,
    layout: 'fit',
    initComponent: function () {
        var me = this;

        var cidadesStore = Ext.create('App.store.enderecos.CidadesStore',{});
        this.cidadesStore = cidadesStore;
        this.cidadesStore.module = this;

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-cidades',
            border: false,
            autoScroll: true,
            store: cidadesStore,
            columns: [{
                xtype: 'numbercolumn',
                header: 'Cod',
                dataIndex: 'codigo',
                format: '0'
            }, {
                header: 'Cidade',
                dataIndex: 'nome',
                flex: 1
            }, {
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
            tbar: [
                { itemId: 'btnAddCidade', text: 'Adicionar', iconCls: 'btn-add', scope: me }, 
                { itemId: 'btnEditCidade', text: 'Editar', iconCls: 'edit', scope: this, disabled: true },
                { itemId: 'btnDelCidade', text: 'Remover', iconCls: 'btn-del', disabled: true, scope: me }
            ],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: cidadesStore,
                displayInfo: true,
                displayMsg: 'Cidades {0} - {1} de {2}',
                emptyMsg: "Nenhuma cidade"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditCidade').setDisabled(!records.length);
                    grid.down('#btnDelCidade').setDisabled(!records.length);
                }
            }
        });

        grid.getStore().load();

        grid.parent = me;
        me.grid = grid;

        this.callParent(arguments);

        this.add(grid);
    },

    createWinAddCidade: function () {
        var addCidadeWin = Ext.ComponentManager.get('win-add-cidade');
        if (!addCidadeWin) {

            var paisesStore = Ext.create('App.store.enderecos.PaisesStore', {pageSize:0});
            var estadosStore = Ext.create('App.store.enderecos.EstadosStore', {pageSize:0});
            
            paisesStore.load();

            addCidadeWin = Ext.create('widget.window', {
                title: 'Adicionar Cidade',
                layout: 'fit',
                id: 'win-add-cidade',
                width: 310,
                modal: true,
                resizable: false,
                items: [{
                    xtype: 'form',
                    border: false,
                    layout: 'anchor',
                    fieldDefaults: {
                        anchor: '100%',
                        labelAlign: 'left',
                        labelWidth: 60,
                        margin: '2 2 2 2'
                    },
                    items: [
                        {
                            xtype: 'combo',
                            id: 'win-add-cidade-combo-pais',
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
                            id: 'win-add-cidade-combo-estado',
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
                            xtype: 'textfield',
                            name: 'nome',
                            fieldLabel: 'Cidade'
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
                        scope: addCidadeWin,
                        handler: function () { addCidadeWin.close(); }
                    }
                ]
            });
        }

        return addCidadeWin;
    },

    createWinEditCidade: function () {
        var editCidadeWin = Ext.ComponentManager.get('win-edit-cidade');
        if (!editCidadeWin) {

            var estadosStore = Ext.create('App.store.enderecos.EstadosStore', {pageSize:0});

            editCidadeWin = Ext.create('widget.window', {
                title: 'Editar Cidade',
                layout: 'fit',
                id: 'win-edit-cidade',
                width: 310,
                modal: true,
                resizable: false,
                items: [{
                    xtype: 'form',
                    border: false,
                    layout: 'anchor',
                    fieldDefaults: {
                        anchor: '100%',
                        labelAlign: 'left',
                        labelWidth: 60,
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
                            xtype: 'combo',
                            id: 'win-edit-cidade-combo-estado',
                            name: 'codigoEstado',
                            autoSelect: true,
                            store: estadosStore,
                            fieldLabel: 'Estado',
                            emptyText: 'Selecione o estado',
                            displayField: 'nome',
                            valueField: 'codigo',
                            allowBlank: false,
                            blankText: 'Um Estado deve ser selecionado',
                            typeAhead: true,
                            queryMode: 'local',
                            selectOnFocus: true,
                            forceSelection: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'nome',
                            fieldLabel: 'Cidade'
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
                        scope: editCidadeWin,
                        handler: function () { editCidadeWin.close(); }
                    }
                ]
            });
        }

        return editCidadeWin;
    }
});