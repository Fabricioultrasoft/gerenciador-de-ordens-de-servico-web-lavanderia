
Ext.define('App.view.ordensDeServico.ItensView', {
    extend: 'App.webDesktop.Module',
    id: 'module-itensOS',
    init: function () {
    },

    createWindow: function (options) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-itensOS');
        if (!win) {
            var panel = this.createPanel(options);
            win = desktop.createWindow({
                id: 'win-itensOS',
                title: 'Item da Ordem de Serviço',
                width: 500,
                height: 380,
                modal: true,
                minimizable: false,
                iconCls: 'itens',
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

        var tapetesStore = Ext.create('App.store.tapetes.TapetesStore', { pageSize: 0 });
        this.tapetesStore = tapetesStore;
        tapetesStore.load();

        this.comprimento = Ext.create('Ext.form.field.Number', { enableKeyEvents: true, flex : 1, itemId: 'module-itensOS-comprimento', name: 'comprimento', fieldLabel: 'Comprimento', emptyText: 'Comprimento', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, labelAlign: 'top',allowBlank: false, blankText: 'Este campo &eacute; obrigat&oacute;rio', margins: '0 4 0 0' });
        this.largura = Ext.create('Ext.form.field.Number', { enableKeyEvents: true, flex : 1, itemId: 'module-itensOS-largura', name: 'largura', fieldLabel: 'Largura', emptyText: 'Largura', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, labelAlign: 'top',allowBlank: false, blankText: 'Este campo &eacute; obrigat&oacute;rio', margins: '0 4 0 0' });
        this.area = Ext.create('Ext.form.field.Number', { flex : 1, itemId: 'module-itensOS-area', name: 'area', fieldLabel: 'Area (m&sup2;)', emptyText: 'Area', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, editable: false, labelAlign: 'top',allowBlank: false, blankText: 'Este campo &eacute; obrigat&oacute;rio', margins: '0' });
        
        this.comprimento.module = this;
        this.largura.module = this;

        var form = Ext.create('Ext.form.Panel', {
            border: false,
            region: 'north',
            bodyPadding: 5,
            fieldDefaults: { labelAlign: 'left', labelWidth: 80, anchor: '100%', allowBlank: false, blankText: 'Este campo &eacute; obrigat&oacute;rio' },
            items: [
                { xtype: 'combobox', store: tapetesStore, name: 'codigoTapete', fieldLabel: 'Tapete', emptyText: 'Selecione o tapete', displayField: 'nome', valueField: 'codigo', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this, listConfig: { getInnerTpl: function () { return '<div>{codigo} - {nome}</div>'; } } },
                { xtype: 'fieldcontainer', fieldLabel: 'Dimens&otilde;es em metros', layout: 'hbox', items: [ this.comprimento, this.largura, this.area ] },
                { xtype: 'textarea', name: 'observacoes', fieldLabel: 'Observa&ccedil;&otilde;es', emptyText: 'Observações referentes aos serviços para este tapete', height: 50, allowBlank: true }
            ]
        });
        this.form = form;

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-servicosDoItemOS',
            border: false,
            title: 'Servi&ccedil;os que ser&atilde;o realizados no Tapete',
            iconCls: 'servicos-thumb',
            region: 'center',
            store: Ext.create('App.store.ordensDeServico.ItensServicosStore', {}),
            style: { borderTop: '1px solid #99BCE8' },
            columns: [
                { text: 'Cod. Item', dataIndex: 'codigoitem', hidden: true },
                { text: 'Cod. Servi&ccedil;o', dataIndex: 'codigoServico', hidden: true },
                { text: 'Servi&ccedil;o', dataIndex: 'nomeServico', flex: 1 },
                { text: 'Qtd. M/M&sup2;', dataIndex: 'quantidade_m_m2' },
                { text: 'Valor', dataIndex: 'valor' }
            ],
            tbar: [
                { itemId: 'btnAddServicos-ItemOS', text: 'Adicionar', iconCls: 'servicos-add-thumb', scope: this },
                { itemId: 'btnEditServicos-ItemOS', text: 'Editar', iconCls: 'servicos-edit-thumb', scope: this, disabled: true },
                { itemId: 'btnDelServicos-ItemOS', text: 'Remover', iconCls: 'servicos-del-thumb', scope: this, disabled: true }
            ],
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#btnEditServicos-ItemOS').setDisabled(!records.length);
                    grid.down('#btnDelServicos-ItemOS').setDisabled(!records.length);
                }
            }
        });
        this.grid = grid;


        var mainPanel = Ext.create('Ext.panel.Panel', {
            border: true,
            layout: 'border',
            items: [form,grid],
            buttonAlign: 'center',
            buttons: [{ text: 'Adicionar Item', itemId: 'btn-add-itemOS', iconCls: 'itens-add', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    },

    //--------------------------------------------------------------------------------------------------
    createServicoWindow: function (options) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-addServicoNoItemOS');
        if (!win) {
            var panel = this.createServicoPanel(options);
            win = desktop.createWindow({
                id: 'win-addServicoNoItemOS',
                title: 'Servi&ccedil;o a realizar no Tapete',
                width: 400,
                height: 300,
                modal: true,
                minimizable: false,
                iconCls: 'servicos-thumb',
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

    createServicoPanel: function (options) {
        this.optionsServicoPanel = options;

        var servicosEspecificosStore = Ext.create('App.store.servicos.ServicosEspecificosStore', { pageSize: 0 });
        servicosEspecificosStore.load({ params:{ codigoTapete: options.codigoTapete,codigoTipoDeCliente: options.codigoTipoDeCliente} });

        var form = Ext.create('Ext.form.Panel', {
            region: 'north',
            bodyPadding: 5,
            fieldDefaults: { labelAlign: 'left', labelWidth: 90, anchor: '100%', allowBlank: false },
            items: [
                { xtype: 'combobox', itemId: '', store: servicosEspecificosStore, name: 'codigoServico', fieldLabel: 'Servi&ccedil;o', emptyText: 'Selecione o serviço', displayField: 'nome', valueField: 'codigo', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this, listConfig: { getInnerTpl: function () { return '<div>{codigo} - {nome} (para: {nomeTipoDeCliente})</div>'; } } },
                { xtype: 'textfield', itemId: '', fieldLabel: 'Cobrado Por', readOnly: true },
                { xtype: 'textfield', itemId: '', fieldLabel: 'Tapete', value: options.nomeTapete, readOnly: true },
                { xtype: 'textfield', itemId: '', fieldLabel: 'Tipo de Cliente', value: options.nomeTipoDeCliente, readOnly: true },
                { xtype: 'fieldcontainer', fieldLabel: 'Valores', layout: 'hbox', defaults: { labelAlign: 'top', margins: '0 4 0 0',editable: false, },
                    items: [
                        { xtype: 'numberfield', flex : 1, fieldLabel: 'Valor at&eacute; 10 m&sup2;', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'numberfield', flex : 1, fieldLabel: 'Valor acima de 10 m&sup2;', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, margins: '0' }
                    ]
                },
                { xtype: 'numberfield', fieldLabel: 'Qtd M/M&sup2;', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                { xtype: 'numberfield', fieldLabel: 'Valor Final', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false }
            ],
            buttonAlign: 'center',
            buttons: [{ text: 'Confirmar', itemId: 'btnConfirmServicoOS', iconCls: 'confirm', padding: '10', scope: this}]
        });
        this.form = form;

        return form;
    }
});
