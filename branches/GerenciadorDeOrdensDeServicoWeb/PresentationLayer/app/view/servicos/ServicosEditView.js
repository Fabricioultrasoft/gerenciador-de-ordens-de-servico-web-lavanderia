
Ext.define('App.view.servicos.ServicosEditView', {
    extend: 'App.webDesktop.Module',
    id: 'module-servicos-edit',
    
    createWindow: function (record) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-servicos-edit');
        if (!win) {
            var servicosPanel = this.createPanel(record);
            win = desktop.createWindow({
                id: 'win-servicos-edit',
                title: 'Editar Serviço',
                width: 600,
                height: 540,
                iconCls: 'servicos-edit-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [servicosPanel]
            });
        }
        win.show();
        return win;
    },

    createPanel: function (record) {

        var valoresStore = Ext.create('App.store.servicos.ValoresServicosStore',{ pageSize:0 });
        valoresStore.getProxy().api.read = "app/handlers/servicos/ServicosHandler.ashx?action=readServico&codigoServico=" + record.data.codigo;

        var tiposDeClientesStore = Ext.create('App.store.clientes.TiposDeClientesStore', { pageSize: 0 });
        tiposDeClientesStore.load({ params: { ativo: true} });
        
        this.tiposDeClientesStore = tiposDeClientesStore;

        var formServico = Ext.create('Ext.form.Panel',{
            title: 'Dados do servi&ccedil;o',
            collapsible: true,
            iconCls: 'servicos-thumb',
            bodyPadding: 5,
            height: 190,
            region: 'north',
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [
                { xtype: 'numberfield', name: 'codigo', fieldLabel: 'Codigo', editable: false, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                { xtype: 'textfield', name: 'nome', fieldLabel: 'Nome', emptyText: 'Digite o nome do serviço', maxLength: 255, allowBlank: false, blankText: 'O NOME do servi&ccedil;o &eacute; obrigat&oacute;rio' },
                {
                    xtype: 'combobox', name: 'codigoCobradoPor', fieldLabel: 'Cobrado por', emptyText: 'Selecione como o serviço é cobrado',
                    store: Ext.create('Ext.data.Store', { fields: ['codigo', 'nome'], data: [ { 'codigo': 1, 'nome': "Metro (M)" }, { 'codigo': 2, 'nome': "Metro quadrado (m²)" } ] }),
                    queryMode: 'local', displayField: 'nome', valueField: 'codigo', selectOnFocus: true, forceSelection: true, allowBlank: false, blankText: 'Uma op&ccedil;&atilde;o deve ser selecionada'
                },
                { xtype: 'textarea', name: 'descricao', fieldLabel: 'Descri&ccedil;&atilde;o', emptyText: 'Descrição do serviço', height: 70 }
            ]
        });
        formServico.loadRecord(record);
        this.formServico = formServico;
        
        var gridValoresServico =  Ext.create('Ext.tree.Panel', {
            id: 'gridEditServico',
            title: 'Valores do servi&ccedil;o',
            iconCls: 'cifrao-thumb',
            region: 'center',
            rootVisible: false,
            animate: false,
            store: valoresStore,
            autoScroll: false,
            tbar: [
                { xtype: 'button', itemId: 'btnEditServico-editValores', text: 'Editar valores', iconCls: 'edit', tooltip: 'Editar valores do servi&ccedil;o para o tapete selecionado na listagem abaixo', scope: this, disabled: true },
                { xtype: 'button', itemId: 'btnEditServico-addCondicaoEspecial', text: 'Adicionar Condi&ccedil;&atilde;o Especial', iconCls: 'favorite-add', tooltip: 'Adicionar uma condi&ccedil;&atilde;o de valor especial para um tipo de cliente', scope: this, disabled: true },
                { xtype: 'button', itemId: 'btnEditServico-delCondicaoEspecial', text: 'Remover Condi&ccedil;&atilde;o Especial', iconCls: 'favorite-del', tooltip: 'Remover condi&ccedil;&atilde;o de valor especial', scope: this, disabled: true }
            ],
            bbar: [
                { text: 'Expandir', iconCls: 'toggle-expand', handler: function(){ gridValoresServico.expandAll(); } }, 
                { text: 'Contrair', iconCls: 'toggle-collapse', handler: function(){ gridValoresServico.collapseAll(); } }
            ],
            columns: [
                { xtype: 'treecolumn', dataIndex: 'nomeTapete', text: 'Tapete', flex: 2, sortable: true },
                { xtype: 'templatecolumn', dataIndex: 'nomeTipoDeCliente', text: 'Tipo de cliente', flex: 1, align: 'center', sortable: false, tpl: Ext.create('Ext.XTemplate', '{nomeTipoDeCliente:this.formatCondEspec}', { formatCondEspec: function(v) { if (v != 'Todos') { return '<b>' + v + '</b>'; } else { return v; } } }) },
                { xtype: 'numbercolumn', dataIndex: 'valor', text: 'Valor', align: 'center', width: 70, sortable: false },
                { xtype: 'numbercolumn', dataIndex: 'valorAcima10m2', text: 'Acima 10m&sup2;', align: 'center', width: 70, sortable: false },
            ],
            listeners: {
                'selectionchange': function (treeModel, selectedRecords) {
                    
                    if( selectedRecords[0].data.codigoTipoDeCliente == 0 ) {
                        gridValoresServico.down('#btnEditServico-addCondicaoEspecial').enable();
                        gridValoresServico.down('#btnEditServico-delCondicaoEspecial').disable();
                    } else {
                        gridValoresServico.down('#btnEditServico-addCondicaoEspecial').disable();
                        gridValoresServico.down('#btnEditServico-delCondicaoEspecial').enable();
                    }
                    gridValoresServico.down('#btnEditServico-editValores').setDisabled(!selectedRecords.length);
                }
            }
        });
        this.gridValoresServico = gridValoresServico;
        this.gridValoresServico.module = this;

        var mainPanel = Ext.create('Ext.panel.Panel', {
            xtype: 'panel',
            layout: 'border',
            border: false,
            items: [formServico,gridValoresServico],
            buttonAlign: 'center',
            buttons: [{ text: 'Salvar altera&ccedil;&otilde;es', itemId: 'btn-edit-servico', iconCls: 'servicos-edit-thumb', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        valoresStore.on('load',function(){
            gridValoresServico.view.refresh();
        });

        return mainPanel;
    },

    createWinEditValores: function () {
        var winEditValores = Ext.ComponentManager.get('win-editServico-editValores');
        if (!winEditValores) {
            
            var arrTipoClientes = new Array(), i;
            for(i = 0; i < this.tiposDeClientesStore.data.items.length; i++) {
                arrTipoClientes.push(this.tiposDeClientesStore.data.items[i].data);
            }

            var tempStore = Ext.create('App.store.clientes.TiposDeClientesStore', {
                data: arrTipoClientes
            });

            winEditValores = Ext.create('widget.window', {
                title: 'Alterar Valores do Servi&ccedil;o',
                iconCls: 'edit',
                layout: 'fit',
                id: 'win-editServico-editValores',
                modal: true,
                resizable: false,
                width: 400,
                items: [{
                    xtype: 'form',
                    border: false,
                    fieldDefaults: {
                        labelAlign: 'right',
                        labelWidth: 90,
                        anchor: '100%',
                        margin: '2 2 2 2'
                    },
                    items: [
                        { xtype: 'textfield', name: 'nomeTapete', fieldLabel: 'Tapete', readOnly: true },
                        { xtype: 'combo', name: 'codigoTipoDeCliente', store: tempStore, fieldLabel: 'Tipo de cliente', emptyText: 'Selecione o tipo de cliente', displayField: 'nome', valueField: 'codigo', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, allowBlank: false, blankText: 'O tipo de cliente é obrigatório' },
                        { xtype: 'numberfield', name: 'valor', fieldLabel: 'Valor', emptyText: 'Valor inicial do serviço', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, allowBlank: false, blankText: 'O valor inicial é obrigatório' },
                        { xtype: 'numberfield', name: 'valorAcima10m2', fieldLabel: 'Acima 10m&sup2;', emptyText: 'Valor do serviço acima de 10m2', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, allowBlank: false, blankText: 'O valor acima de 10m&sup2; é obrigatório' }
                    ]
                }],
                buttons: [
                    { text: 'Salvar', action: 'save', scope: this },
                    { text: 'Cancelar', scope: winEditValores, handler: function () { winEditValores.close(); } }
                ]
            });
        }

        return winEditValores;
    },

    createWinAddCondicaoEspecial: function (recordPai) {
        var win = Ext.ComponentManager.get('win-editServico-addCondicaoEspecial');
        if (!win) {

            win = Ext.create('widget.window', {
                title: 'Adicionar Condi&ccedil;&atilde;o Especial de valores',
                iconCls: 'favorite-add',
                layout: 'fit',
                id: 'win-editServico-addCondicaoEspecial',
                modal: true,
                resizable: false,
                width: 400,
                items: [{
                    xtype: 'form',
                    border: false,
                    fieldDefaults: {
                        labelAlign: 'right',
                        labelWidth: 90,
                        anchor: '100%',
                        margin: '2 2 2 2'
                    },
                    items: [
                        { xtype: 'textfield', name: 'nomeTapete', fieldLabel: 'Tapete', readOnly: true, value: recordPai.data.nomeTapete },
                        { xtype: 'combo', name: 'codigoTipoDeCliente', store: this.tiposDeClientesStore, fieldLabel: 'Tipo de cliente', emptyText: 'Selecione o tipo de cliente', displayField: 'nome', valueField: 'codigo', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, allowBlank: false, blankText: 'O tipo de cliente é obrigatório' },
                        { xtype: 'numberfield', name: 'valor', fieldLabel: 'Valor', emptyText: 'Valor inicial do serviço', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, allowBlank: false, blankText: 'O valor inicial é obrigatório' },
                        { xtype: 'numberfield', name: 'valorAcima10m2', fieldLabel: 'Acima 10m&sup2;', emptyText: 'Valor do serviço acima de 10m2', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, allowBlank: false, blankText: 'O valor acima de 10m&sup2; é obrigatório' }
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
