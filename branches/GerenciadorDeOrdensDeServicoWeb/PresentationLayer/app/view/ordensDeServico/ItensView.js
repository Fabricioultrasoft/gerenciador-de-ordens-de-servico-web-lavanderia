
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
            win.module = this;
        }
        win.show();
        return win;
    },

    createPanel: function (options) {
        this.options = options;

        // usado para controlar os servicos do item
        this.servicosEspecificosStore = Ext.create('App.store.servicos.ServicosEspecificosStore', { pageSize: 0 });

        var tapetesStore = Ext.create('App.store.tapetes.TapetesStore', { pageSize: 0 });
        this.tapetesStore = tapetesStore;
        tapetesStore.load();

        this.comprimento = Ext.create('Ext.form.field.Number', { enableKeyEvents: true, flex : 1, itemId: 'module-itensOS-comprimento', name: 'comprimento', fieldLabel: 'Comprimento', emptyText: 'Comprimento', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, labelAlign: 'top',allowBlank: false, blankText: 'Este campo &eacute; obrigat&oacute;rio', margins: '0 4 0 0' });
        this.largura = Ext.create('Ext.form.field.Number', { enableKeyEvents: true, flex : 1, itemId: 'module-itensOS-largura', name: 'largura', fieldLabel: 'Largura', emptyText: 'Largura', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, labelAlign: 'top',allowBlank: false, blankText: 'Este campo &eacute; obrigat&oacute;rio', margins: '0 4 0 0' });
        this.area = Ext.create('Ext.form.field.Number', { flex : 1, itemId: 'module-itensOS-area', name: 'area', fieldLabel: 'Area (m&sup2;)', emptyText: 'Area', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, editable: false, labelAlign: 'top',allowBlank: false, blankText: 'Este campo &eacute; obrigat&oacute;rio', margins: '0', cls: 'inputDisabled' });
        
        this.comprimento.module = this;
        this.largura.module = this;

        var form = Ext.create('Ext.form.Panel', {
            border: false,
            region: 'north',
            bodyPadding: 5,
            fieldDefaults: { labelAlign: 'left', labelWidth: 80, anchor: '100%', allowBlank: false, blankText: 'Este campo &eacute; obrigat&oacute;rio' },
            items: [
                { xtype: 'combobox', itemId:'cboTapetes-itensOS', store: tapetesStore, name: 'codigoTapete', fieldLabel: 'Tapete', emptyText: 'Selecione o tapete', displayField: 'nome', valueField: 'codigo', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this, listConfig: { getInnerTpl: function () { return '<div>{codigo} - {nome}</div>'; } } },
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
                { text: 'Valor', dataIndex: 'valor', renderer: Ext.util.Format.brMoney }
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
        this.grid.module = this;

        var mainPanel = Ext.create('Ext.panel.Panel', {
            border: true,
            layout: 'border',
            items: [form,grid],
            buttonAlign: 'center',
            buttons: [{ text: 'Confirmar', itemId: 'btnConfirmItemOS', iconCls: 'confirm', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        if(options.edit) {
            tapetesStore.on('load', function( store, records, isSuccessful, operation, opts) {
                form.loadRecord(options.record);
                var combo = form.down('#cboTapetes-itensOS');
                combo.fireEvent('select',combo,combo.findRecordByValue( options.record.data.tapete.codigo ),{});
                this.desabilitaDadosTapete();

                for(var i = 0; i < options.record.data.servicosDoItem.length; i++ ) {
                    grid.getStore().add(options.record.data.servicosDoItem[i]);
                }
            },this);
        }

        return mainPanel;
    },

    //--------------------------------------------------------------------------------------------------
    createServicoWindow: function (options) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-servicoNoItemOS');
        if (!win) {
            var panel = this.createServicoPanel(options);
            win = desktop.createWindow({
                id: 'win-servicoNoItemOS',
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
            win.module = this;
        }
        win.show();
        return win;
    },

    createServicoPanel: function (options) {
        this.optionsServicoPanel = options;

        var formServicos = Ext.create('Ext.form.Panel', {
            region: 'north',
            bodyPadding: 5,
            fieldDefaults: { labelAlign: 'left', labelWidth: 90, anchor: '100%', allowBlank: false },
            items: [
                { xtype: 'textfield', fieldLabel: 'Tapete', value: options.nomeTapete, readOnly: true, cls: 'inputDisabled' },
                { xtype: 'textfield', fieldLabel: 'Tipo de Cliente', value: options.nomeTipoDeCliente, readOnly: true, cls: 'inputDisabled' },
                { xtype: 'combobox', itemId: 'cboValoresEspecificos-itemOS', store: this.servicosEspecificosStore, name: 'codigoServico', fieldLabel: 'Servi&ccedil;o', emptyText: 'Selecione o serviço', displayField: 'nome', valueField: 'codigo', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this, listConfig: { getInnerTpl: function () { return '<div>{codigo} - {nome} (para: {nomeTipoDeCliente})</div>'; } } },
                { xtype: 'fieldcontainer', fieldLabel: 'Valores', layout: 'hbox', defaults: { labelAlign: 'top', margins: '0 4 0 0',editable: false },
                    items: [
                        { xtype: 'numberfield', flex : 1, itemId: 'servicoValInicial-itemOS', fieldLabel: 'Valor R$', disabled: true, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, cls: 'inputDisabled' },
                        { xtype: 'numberfield', flex : 1, itemId: 'servicoValAcima10m2-itemOS', fieldLabel: 'Acima de 10 m&sup2; R$', disabled: true, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, margins: '0', cls: 'inputDisabled' }
                    ]
                },
                { xtype: 'textfield', itemId: 'txtCobradoPor-itemOS', fieldLabel: 'Cobrado Por', disabled: true, readOnly: true, cls: 'inputDisabled' },
                { xtype: 'numberfield', itemId: 'qtdMm2-itemOS', name: 'quantidade_m_m2', fieldLabel: 'Qtd M/M&sup2;', disabled: true, enableKeyEvents: true, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, module: this },
                { xtype: 'numberfield', itemId: 'servicoValFinal-itemOS', name: 'valor', fieldLabel: 'Valor Final R$', disabled: true, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false }
            ],
            buttonAlign: 'center',
            buttons: [{ text: 'Confirmar', itemId: 'btnConfirmServicoOS', iconCls: 'confirm', padding: '10', scope: this}]
        });
        this.formServicos = formServicos;

        if(options.edit) {
            var combo = formServicos.down('#cboValoresEspecificos-itemOS');
            combo.select( options.record.data.servico.codigo );
            combo.fireEvent('select',combo,combo.findRecordByValue( options.record.data.servico.codigo ),{});
            formServicos.loadRecord(options.record);
        }

        return formServicos;
    },

    calcularValorServico: function(servico) {
        var qtdMm2 = this.formServicos.down('#qtdMm2-itemOS').getValue();
        if(qtdMm2 > 0) {
            var valorTabela = (servico.codigoCobradoPor == 2 && qtdMm2 > 10) ? servico.valorAcima10m2 : servico.valor;
            this.formServicos.down('#servicoValFinal-itemOS').setValue(qtdMm2 * valorTabela);
        } else {
            this.formServicos.down('#servicoValFinal-itemOS').setValue(0);
        }
    },

    habilitaDadosTapete: function() {
        // habilita os campos para poder recuperar os valores
        this.form.down('#cboTapetes-itensOS').enable();
        this.comprimento.enable();
        this.largura.enable();
        this.area.enable();
    },

    desabilitaDadosTapete: function() {
        this.form.down('#cboTapetes-itensOS').disable();
        this.comprimento.disable();
        this.largura.disable();
        this.area.disable();
    }
});
