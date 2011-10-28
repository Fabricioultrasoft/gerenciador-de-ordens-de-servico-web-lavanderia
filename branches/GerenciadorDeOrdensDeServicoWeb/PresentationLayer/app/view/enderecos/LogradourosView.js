Ext.define('App.view.enderecos.LogradourosView', {
    extend: 'App.webDesktop.Module',
    id: 'module-logradouros',
    init: function () {
    },

    createWindow: function () {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-logradouros');
        if (!win) {
            var logradourosPanel = this.createPanel();
            win = desktop.createWindow({
                id: 'win-logradouros',
                title: 'Logradouros',
                width: 700,
                height: 400,
                iconCls: 'location-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [logradourosPanel]
            });
        }
        win.show();
        return win;
    },


    createPanel: function () {

        var logradourosStore = Ext.create('App.store.enderecos.LogradourosStore',{});
        this.logradourosStore = logradourosStore;
        this.logradourosStore.module = this;
        
        logradourosStore.load();

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-logradouros',
            border: false,
            autoScroll: true,
            store: logradourosStore,
            columns: [
                { xtype: 'numbercolumn', header: 'Cod', dataIndex: 'codigo', format: '0' }, 
                { header: 'Cod Tipo de Logradouro', dataIndex: 'codigoTipoDeLogradouro', hidden: true }, 
                { header: 'Tipo', dataIndex: 'nomeTipoDeLogradouro' }, 
                { header: 'Logradouro', dataIndex: 'nome', width: 200 }, 
                { header: 'CEP', dataIndex: 'cep' }, 
                { header: 'Cod Bairro', dataIndex: 'codigoBairro', hidden: true }, 
                { header: 'Bairro', dataIndex: 'nomeBairro' }, 
                { header: 'Cod Cidade', dataIndex: 'codigoCidade', hidden: true }, 
                { header: 'Cidade', dataIndex: 'nomeCidade' },
                { header: 'Cod Estado', dataIndex: 'codigoEstado', hidden: true },
                { header: 'Estado', dataIndex: 'nomeEstado' },
                { header: 'Cod Pa&iacute;s', dataIndex: 'codigoPais', hidden: true }, 
                { header: 'Pa&iacute;s', dataIndex: 'nomePais' }
            ],
            tbar: [
                { itemId: 'btnAddLogradouro', text: 'Adicionar', iconCls: 'btn-add', scope: this },
                { itemId: 'btnEditLogradouro', text: 'Editar', iconCls: 'edit', scope: this, disabled: true },
                { itemId: 'btnDelLogradouro', text: 'Remover', iconCls: 'btn-del', disabled: true, scope: this }
            ],
            bbar: Ext.create('Ext.PagingToolbar', {
                store: logradourosStore,
                displayInfo: true,
                displayMsg: 'Logradouros {0} - {1} de {2}',
                emptyMsg: "Nenhum logradouro"
            }),
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditLogradouro').setDisabled(!records.length);
                    grid.down('#btnDelLogradouro').setDisabled(!records.length);
                }
            }
        });
        this.gridLogradouros = grid;
        this.gridLogradouros.module = this;

        var mainPanel = Ext.create('Ext.panel.Panel', {
            layout: 'fit',
            items: [grid]
        });
        this.mainPanel = mainPanel;

        return mainPanel;

    },

    createWinAddLogradouro: function () {
        var addLogradouroWin = Ext.ComponentManager.get('win-add-logradouro');
        if (!addLogradouroWin) {

            var paisesStore = Ext.create('App.store.enderecos.PaisesStore', {pageSize:0});
            var estadosStore = Ext.create('App.store.enderecos.EstadosStore', {pageSize:0});
            var cidadesStore = Ext.create('App.store.enderecos.CidadesStore', {pageSize:0});
            var bairrosStore = Ext.create('App.store.enderecos.BairrosStore', {pageSize:0});
            var tiposDeLogradourosStore = Ext.create('App.store.enderecos.TiposDeLogradourosStore', {pageSize:0});
            
            paisesStore.load();
            tiposDeLogradourosStore.load();

            addLogradouroWin = Ext.create('widget.window', {
                title: 'Adicionar Logradouro',
                layout: 'fit',
                id: 'win-add-logradouro',
                modal: true,
                resizable: false,
                width: 300,
                items: [{
                    xtype: 'form',
                    border: false,
                    fieldDefaults: {
                        labelAlign: 'right',
                        labelWidth: 65,
                        anchor: '100%',
                        margin: '2 2 2 2'
                    },
                    items: [
                        { xtype: 'combo', name: 'codigoPais', store: paisesStore, id: 'win-add-logradouro-combo-pais', fieldLabel: 'Pa&iacute;s', emptyText: 'Selecione o país', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um Pa&iacute;s deve ser selecionado', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true },
                        { xtype: 'combo', name: 'codigoEstado', store: estadosStore, id: 'win-add-logradouro-combo-estado', fieldLabel: 'Estado', emptyText: 'Selecione o estado', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um Estado deve ser selecionado', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true },
                        { xtype: 'combo', name: 'codigoCidade', store: cidadesStore, id: 'win-add-logradouro-combo-cidade', fieldLabel: 'Cidade', emptyText: 'Selecione a cidade', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Uma Cidade deve ser selecionada', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true },
                        { xtype: 'combo', name: 'codigoBairro', store: bairrosStore, id: 'win-add-logradouro-combo-bairro', fieldLabel: 'Bairro', emptyText: 'Selecione o bairro', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um Bairro deve ser selecionado', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true },
                        { xtype: 'combo', name: 'codigoTipoDeLogradouro', store: tiposDeLogradourosStore, fieldLabel: 'Tipo', emptyText: 'Selecione o tipo de logradouro', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um tipo de logradouro deve ser selecionado', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true },
                        { xtype: 'textfield', name: 'cep', fieldLabel: 'CEP', emptyText: '00000-000', maxLength:10 },
                        { xtype: 'textfield', name: 'nome', fieldLabel: 'Logradouro' }
                    ]
                }],
                buttons: [
                    { text: 'Salvar', action: 'save', scope: this },
                    { text: 'Cancelar', scope: addLogradouroWin, handler: function () { addLogradouroWin.close(); } }
                ]
            });

            addLogradouroWin.paisesStore = paisesStore;
            addLogradouroWin.estadosStore = estadosStore;
            addLogradouroWin.cidadesStore = cidadesStore;
            addLogradouroWin.bairrosStore = bairrosStore;
            addLogradouroWin.tiposDeLogradourosStore = tiposDeLogradourosStore;
        }

        return addLogradouroWin;
    },

    createWinEditLogradouro: function () {
        var editLogradouroWin = Ext.ComponentManager.get('win-edit-logradouro');
        if (!editLogradouroWin) {

            var bairrosStore = Ext.create('App.store.enderecos.BairrosStore', {pageSize:0});
            var tiposDeLogradourosStore = Ext.create('App.store.enderecos.TiposDeLogradourosStore', {pageSize:0});

            editLogradouroWin = Ext.create('widget.window', {
                title: 'Editar Logradouro',
                layout: 'fit',
                id: 'win-edit-logradouro',
                modal: true,
                resizable: false,
                width: 300,
                items: [{
                    xtype: 'form',
                    border: false,
                    fieldDefaults: {
                        labelAlign: 'right',
                        labelWidth: 65,
                        anchor: '100%',
                        margin: '2 2 2 2'
                    },
                    items: [
                        { xtype: 'textfield', name: 'codigo', fieldLabel: 'Código', readOnly: true },
                        { xtype: 'textfield', name: 'nomePais', fieldLabel: 'Pa&iacute;s', readOnly: true },
                        { xtype: 'textfield', name: 'nomeEstado', fieldLabel: 'Estado', readOnly: true },
                        { xtype: 'textfield', name: 'nomeCidade', fieldLabel: 'Cidade', readOnly: true },
                        { xtype: 'combo', name: 'codigoBairro', autoSelect: true, store: bairrosStore, fieldLabel: 'Bairro', emptyText: 'Selecione o bairro', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um Bairro deve ser selecionado', typeAhead: true, queryMode: 'local', selectOnFocus: true, forceSelection: true },
                        { xtype: 'combo', name: 'codigoTipoDeLogradouro', store: tiposDeLogradourosStore, fieldLabel: 'Tipo', emptyText: 'Selecione o tipo de logradouro', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um tipo de logradouro deve ser selecionado', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true },
                        { xtype: 'textfield', name: 'cep', fieldLabel: 'CEP', emptyText: '00000-000' },
                        { xtype: 'textfield', name: 'nome', fieldLabel: 'Logradouro' }
                    ]
                }],
                buttons: [
                    { text: 'Salvar', action: 'save' },
                    { text: 'Cancelar', scope: editLogradouroWin, handler: function () { editLogradouroWin.close(); } }
                ]
            });

            editLogradouroWin.bairrosStore = bairrosStore;
            editLogradouroWin.tiposDeLogradourosStore = tiposDeLogradourosStore;
        }

        return editLogradouroWin;
    }
});