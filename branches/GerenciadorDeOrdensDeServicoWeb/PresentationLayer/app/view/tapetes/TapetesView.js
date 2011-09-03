
Ext.define('App.view.tapetes.TapetesView', {
    extend: 'App.webDesktop.Module',
    requires: ['App.ux.PreviewPlugin','Ext.String'],
    id: 'module-tapetes',
    init: function () {
        this.launcher = {
            text: 'Tapetes',
            iconCls: 'tapete-thumb',
            handler: this.createWindow,
            scope: this
        };
    },

    createWindow: function () {
        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-tapetes');
        if (!win) {

            var tapetesStore = Ext.create('App.store.tapetes.TapetesStore', {});
            tapetesStore.load();

            var formTapetes = Ext.create('Ext.form.Panel',{
                id: 'form-tapetes',
                height: 150,
                region: 'north',
                buttonAlign: 'center',
                style: {
                    background:'#fff',
                    padding: '5px'
                },
                bodyStyle: {
                    background: '#D3E1F1',
                    padding: '5px'
                },
                fieldDefaults: {
                    labelAlign: 'right',
                    labelWidth: 60,
                    anchor: '100%'
                },
                items: [
                    {
                        xtype: 'hiddenfield',
                        name: 'codigo'
                    },
                    {
                        xtype: 'textfield',
                        name: 'nome',
                        fieldLabel: 'Tapete',
                        emptyText: 'Digite o tapete',
                        allowBlank: false,
                        blankText: 'Este campo &eacute; de preenchimento obrigat&oacute;rio',
                        maxLength: 100
                    }, {
                        xtype: 'textarea',
                        name: 'descricao',
                        fieldLabel: 'Descri&ccedil;&atilde;o',
                        emptyText: 'Digite uma descrição para o tapete',
                        height: 50
                    }, {
                        xtype: 'checkbox',
                        name: 'ativo',
                        fieldLabel: 'Ativo',
                        inputValue: 1,
                        checked: true
                    }
                ],
                buttons: [{
                    itemId: 'btnAddTapete',
                    text: 'Adicionar', 
                    iconCls: 'btn-add',
                    scope: this
                }, { 
                    itemId: 'btnLimparTapete',
                    text: 'Limpar',
                    iconCls: 'btn-limpar',
                    scope: this
                }]
            });
            this.formTapetes = formTapetes;

            var gridTapetes = Ext.create('Ext.grid.Panel', {
                id: 'grid-tapetes',
                border: false,
                region: 'center',
                store: tapetesStore,
                cls: 'grid-style-1',
                style: {
                    borderTop: '1px solid #99BCE8'
                },
                columns: [{
                    xtype: 'numbercolumn',
                    header: 'Cod',
                    dataIndex: 'codigo',
                    format: '0'
                }, {
                    header: 'Nome',
                    dataIndex: 'nome',
                    flex: 1,
                    renderer: Ext.String.htmlEncode
                }, {
                    header: 'Descricao',
                    dataIndex: 'descricao',
                    hidden:true,
                    renderer: Ext.String.htmlEncode
                }, {
                    xtype: 'booleancolumn',
                    header: 'Ativo',
                    dataIndex: 'ativo'
                }],
                tbar: [{
                    itemId: 'btnDelTapete',
                    text: 'Remover',
                    iconCls: 'btn-del',
                    scope: this,
                    disabled: true
                }, {
                    itemId: 'btnShowDescricaoTapete',
                    iconCls: 'btn-detalhes',
                    scope: this,
                    pressed: false,
                    enableToggle: true,
                    text: 'Descri&ccedil;&atilde;o',
                    tooltip: {
                        title: 'Descri&ccedil;&atilde;o dos tapetes',
                        text: 'Visualizar a descri&ccedil;&atilde;o de cada registro na listagem'
                    }
                }],
                bbar: Ext.create('Ext.PagingToolbar', {
                    store: tapetesStore,
                    displayInfo: true,
                    displayMsg: 'tapetes {0} - {1} of {2}',
                    emptyMsg: "Nenhum tapete"
                }),
                listeners: {
                    'selectionchange': function (view, records) {
                        gridTapetes.down('#btnDelTapete').setDisabled(!records.length);
                    }
                },
                viewConfig: {
                    itemId: 'view',
                    plugins: [{
                        pluginId: 'preview',
                        ptype: 'preview',
                        bodyField: 'descricao',
                        previewExpanded: false,
                        labelField: '<b>Descri&ccedil;&atilde;o:</b> '
                    }]
                }
            });
            this.gridTapetes = gridTapetes;
            this.gridTapetes.tapetesView = this;

            win = desktop.createWindow({
                id: 'win-tapetes',
                title: 'Tapetes',
                width: 600,
                height: 480,
                iconCls: 'tapete-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                items: [{
                    xtype: 'panel',
                    layout: 'border',
                    border: false,
                    items: [formTapetes,gridTapetes]
                }]
            });
        }
        win.show();
        return win;
    },

    createWinEditTapete: function () {
        var editTapeteWin = Ext.ComponentManager.get('win-edit-tapete');
        if (!editTapeteWin) {

            var tapetesStore = Ext.create('App.store.tapetes.TapetesStore', {pageSize:0});
            
            editTapeteWin = Ext.create('widget.window', {
                title: 'Editar Tapete',
                width: 600,
                height: 200,
                iconCls: 'tapete-thumb',
                layout: 'fit',
                id: 'win-edit-tapete',
                modal: true,
                resizable: false,
                items: [{
                    xtype: 'form',
                    border: false,
                    fieldDefaults: {
                        margin: '2 2 2 2',
                        labelAlign: 'right',
                        labelWidth: 60,
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'codigo',
                            fieldLabel: 'Código',
                            readOnly: true
                        }, {
                            xtype: 'textfield',
                            name: 'nome',
                            fieldLabel: 'Tapete',
                            emptyText: 'Digite o tapete',
                            allowBlank: false,
                            blankText: 'Este campo &eacute; de preenchimento obrigat&oacute;rio',
                            maxLength: 100
                        }, {
                            xtype: 'textarea',
                            name: 'descricao',
                            fieldLabel: 'Descri&ccedil;&atilde;o',
                            emptyText: 'Digite uma descrição para o tapete',
                            height: 50
                        }, {
                            xtype: 'checkbox',
                            name: 'ativo',
                            fieldLabel: 'Ativo',
                            inputValue: 1,
                            checked: true
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
                        scope: editTapeteWin,
                        handler: function () { editTapeteWin.close(); }
                    }
                ]
            });
        }

        return editTapeteWin;
    }
});