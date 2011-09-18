
Ext.define('App.view.ordensDeServico.OrdensDeServicoAddView', {
    extend: 'App.webDesktop.Module',
    requires: ['App.ux.PreviewPlugin'],
    id: 'module-ordensDeServico-add',
    init: function () {
    },

    createWindow: function (options) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-ordensDeServico-add');
        if (!win) {
            var panel = this.createPanel(options);
            win = desktop.createWindow({
                id: 'win-ordensDeServico-add',
                title: 'Adicionar Nova Ordem de Serviço',
                width: 600,
                height: 480,
                iconCls: 'os-add',
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

        var form = Ext.create('Ext.form.Panel', {
            border: false,
            region: 'north',
            bodyPadding: 5,
            fieldDefaults: {
                labelAlign: 'left',
                labelWidth: 80,
                anchor: '100%'
            },
            items: [
                { xtype: 'numberfield', name: 'numero', fieldLabel: 'Numero', emptyText: 'Numero da Ordem de Serviço', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                { xtype: 'fieldcontainer', combineErrors: true, fieldLabel: 'Cliente', layout: 'hbox', defaults: { hideLabel: true },
                    items: [
                        { xtype: 'numberfield', width: 60, name: 'codigoCliente', emptyText: 'Codigo', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'textfield', flex : 1, name : 'nomeCliente', emptyText: 'Nome do Cliente', allowBlank: false, margins: '0 4' },
                        { xtype: 'button', text: 'Buscar', iconCls: 'lupa' }
                    ]
                },
                { xtype: 'fieldcontainer', combineErrors: true, fieldLabel: 'Valor', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'numberfield', flex : 1, name: 'valorOriginal', fieldLabel: 'Valor Original', emptyText: 'Valor Original', editable: false, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, margins: '0 4 0 0' },
                        { xtype: 'numberfield', flex : 1, name: 'valorFinal', fieldLabel: 'Final/Com Desconto', emptyText: 'Final/Com Desconto', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false }
                    ]
                },

                { xtype: 'fieldcontainer', combineErrors: true, fieldLabel: 'Datas', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'datefield', flex : 1, name: 'dataDeAbertura', fieldLabel: 'Data de Abertura', emptyText: 'Data de Abertura', emptyText: 'dd/mm/aaaa', format: 'd/m/Y', value: new Date(), margins: '0 4 0 0' },
                        { xtype: 'datefield', flex : 1, name: 'previsaoDeConclusao', fieldLabel: 'Previsão de Conclusão', emptyText: 'Previsão de Conclusão', emptyText: 'dd/mm/aaaa', format: 'd/m/Y', value: new Date() }
                    ]
                },
                { xtype: 'textarea', name: 'observacoes', fieldLabel: 'Observa&ccedil;&otilde;es', emptyText: 'Observações gerais', height: 50 }
            ]
        });
        this.form = form;

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-itensOS',
            border: false,
            title: 'Itens da Ordem de Servi&ccedil;o',
            iconCls: 'itens',
            region: 'center',
            store: Ext.create('App.store.ordensDeServico.ItensStore', {}),
            style: {
                borderTop: '1px solid #99BCE8'
            },
            columns: [
                { text: 'Cod. Item', dataIndex: 'codigo', hidden: true },
                { text: 'Cod. Tapete', dataIndex: 'codigoTapete', hidden: true },
                { text: 'Tapete', dataIndex: 'nomeTapete', flex: 1 },
                { text: 'Comprimento', dataIndex: 'comprimento' },
                { text: 'Largura', dataIndex: 'largura' },
                { text: 'Area', dataIndex: 'area' },
                { text: 'Valor', dataIndex: 'valor' }
            ],
            tbar: [
                { itemId: 'module-ordensDeServico-add_btnAddItemOS', text: 'Adicionar', iconCls: 'itens-add', scope: this },
                { itemId: 'module-ordensDeServico-add_btnEditItemOS', text: 'Editar', iconCls: 'itens-edit', scope: this, disabled: true },
                { itemId: 'module-ordensDeServico-add_btnDelItemOS', text: 'Remover', iconCls: 'itens-del', scope: this, disabled: true },
                {
                    itemId: 'btnShowDescricaoOrdensDeServicoAdd',
                    iconCls: 'btn-detalhes',
                    scope: this,
                    pressed: false,
                    enableToggle: true,
                    text: 'Observa&ccedil;&otilde;es',
                    tooltip: {
                        title: 'Observa&ccedil;&otilde;es da Ordem de Servi&ccedil;o',
                        text: 'Visializar a descri&ccedil;&atilde;o de cada registro na listagem'
                    }
                }
            ],
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#module-ordensDeServico-add_btnEditItemOS').setDisabled(!records.length);
                    grid.down('#module-ordensDeServico-add_btnDelItemOS').setDisabled(!records.length);
                }
            },
            viewConfig: {
                itemId: 'view',
                plugins: [{
                    pluginId: 'preview',
                    ptype: 'preview',
                    bodyField: 'observacoes',
                    previewExpanded: false,
                    labelField: '<b>Observa&ccedil;&otilde;es:</b> '
                }]
            }
        });
        this.grid = grid;

        //--------------------------------------------------------------------
        var mainPanel = Ext.create('Ext.panel.Panel', {
            border: true,
            layout: 'border',
            items: [form,grid],
            buttonAlign: 'center',
            buttons: [{ text: 'Adicionar OS', itemId: 'btn-add-ordemDeServico', iconCls: 'os-add', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    }
});
