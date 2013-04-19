
Ext.define('App.view.ordensDeServico.OrdensDeServicoEditView', {
    extend: 'App.webDesktop.Module',
    requires: ['App.ux.PreviewPlugin'],
    id: 'module-ordensDeServico-edit',
    init: function () {
    },

    createWindow: function (options) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-ordensDeServico-edit');
        if (!win) {
            var panel = this.createPanel(options);
            win = desktop.createWindow({
                id: 'win-ordensDeServico-edit',
                title: 'Editar Ordem de Serviço',
                width: 600,
                height: 480,
                iconCls: 'os-add',
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
        var record = options.record;
        this.options = options;
        this.cliente = record.data.cliente;
        var itens = record.data.itens;

        var itensStore = Ext.create('App.store.ordensDeServico.ItensStore', { data: itens });
        itensStore.on({ add: this.atualizarValorOS, update: this.atualizarValorOS, remove: this.atualizarValorOS, scope: this });
        this.itensStore = itensStore;

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
                { xtype: 'numberfield', name: 'numero', fieldLabel: 'Numero', emptyText: 'Numero da Ordem de Serviço', allowBlank: false, minValue: 0, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                { xtype: 'fieldcontainer', fieldLabel: 'Cliente', layout: 'hbox', defaults: { hideLabel: true, allowBlank: false, blankText: 'Para adicionar um Cliente, clique no bot&atilde;o [Add]' },
                    items: [
                        { xtype: 'numberfield', itemId:'moduleEditOS_codigoCliente', width: 60, name: 'codigoCliente', emptyText: 'Codigo', editable: false, cls: 'inputDisabled', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'textfield', itemId:'moduleEditOS_nomeCliente', flex : 1, name : 'nomeCliente', emptyText: 'Nome do Cliente', readOnly: true, cls: 'inputDisabled', margins: '0 4' },
                        { xtype: 'button', text: 'Add', itemId: 'btnEditClienteOS', iconCls: 'clientes-add-thumb', scope: this }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: 'Valor R$', layout: 'hbox', defaults: { labelAlign: 'top', allowBlank: false },
                    items: [
                        { xtype: 'numberfield', flex : 1, itemId: 'osEditValOriginal', name: 'valorOriginal', fieldLabel: 'Valor Original', emptyText: 'Valor Original', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, margins: '0 4 0 0', editable: false, cls: 'inputDisabled' },
                        { xtype: 'numberfield', flex : 1, itemId: 'osEditValFinal', name: 'valorFinal', fieldLabel: 'Valor Final/Com Desconto', emptyText: 'Final/Com Desconto', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: 'Datas', layout: 'hbox', defaults: { labelAlign: 'top',allowBlank: false },
                    items: [
                        { xtype: 'datefield', flex : 1, name: 'dataDeAbertura', fieldLabel: 'Data de Abertura', emptyText: 'Data de Abertura', emptyText: 'dd/mm/aaaa', format: 'd/m/Y', value: new Date(), margins: '0 4 0 0' },
                        { xtype: 'datefield', flex : 1, name: 'previsaoDeConclusao', fieldLabel: 'Previsão de Conclusão', emptyText: 'Previsão de Conclusão', emptyText: 'dd/mm/aaaa', format: 'd/m/Y', value: new Date() }
                    ]
                },
                { xtype: 'textarea', name: 'observacoes', fieldLabel: 'Observa&ccedil;&otilde;es', emptyText: 'Observações gerais', height: 50 }
            ]
        });
        form.loadRecord(record);
        this.form = form;


        var renderServicosDoItem = function(servicos) {
            var nomes = new Array(), i;
            for( i = 0; i < servicos.length; i++ ) { nomes.push(servicos[i].nomeServico); }
            return nomes.join(', ');
        };

        var grid = Ext.create('Ext.grid.Panel', {
            id: 'grid-itensEditOS',
            border: false,
            title: 'Itens da Ordem de Servi&ccedil;o',
            iconCls: 'itens',
            region: 'center',
            store: itensStore,
            cls: 'grid-style-1',
            style: {
                borderTop: '1px solid #99BCE8'
            },
            columns: [
                { text: 'Cod. Item', dataIndex: 'codigo', hidden: true },
                { text: 'Cod. Tapete', dataIndex: 'codigoTapete', hidden: true },
                { text: 'Tapete', dataIndex: 'nomeTapete', flex: 1 },
                { text: 'Comprimento', dataIndex: 'comprimento', width: 80, renderer: function(value){ return value + ' metro(s)';  } },
                { text: 'Largura', dataIndex: 'largura', width: 80, renderer: function(value){ return value + ' metro(s)';  } },
                { text: 'Area', dataIndex: 'area', width: 80, renderer: function(value){ return value + ' m&sup2;';  } },
                { text: 'Valor', dataIndex: 'valor', width: 80, renderer: Ext.util.Format.brMoney },
                { text: 'Servi&ccedil;os', dataIndex: 'servicosDoItem', flex: 1, renderer: renderServicosDoItem }
            ],
            tbar: [
                { itemId: 'module-ordensDeServico-edit_btnAddItemOS', text: 'Adicionar', iconCls: 'itens-add', scope: this },
                { itemId: 'module-ordensDeServico-edit_btnEditItemOS', text: 'Editar', iconCls: 'itens-edit', scope: this, disabled: true },
                { itemId: 'module-ordensDeServico-edit_btnDelItemOS', text: 'Remover', iconCls: 'itens-del', scope: this, disabled: true },
                {
                    itemId: 'btnShowDescricaoOrdensDeServicoEdit',
                    iconCls: 'btn-detalhes',
                    scope: this,
                    pressed: false,
                    enableToggle: true,
                    text: 'Observa&ccedil;&otilde;es',
                    tooltip: {
                        title: 'Observa&ccedil;&otilde;es da Ordem de Servi&ccedil;o',
                        text: 'Visualizar a descri&ccedil;&atilde;o de cada registro na listagem'
                    }
                }
            ],
            listeners: {
                'selectionchange': function (view, records) {
                    grid.down('#module-ordensDeServico-edit_btnEditItemOS').setDisabled(!records.length);
                    grid.down('#module-ordensDeServico-edit_btnDelItemOS').setDisabled(!records.length);
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
        this.grid.module = this;

        //--------------------------------------------------------------------
        var mainPanel = Ext.create('Ext.panel.Panel', {
            border: true,
            layout: 'border',
            items: [form,grid],
            buttonAlign: 'center',
            buttons: [{ text: 'Alterar OS', itemId: 'btnAlterarOS', iconCls: 'os-add', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    },

    setCliente: function(cliente) {
        if(this.cliente != null 
        && this.grid.getStore().getCount() > 0
        && this.cliente.codigoTipoDeCliente != cliente.codigoTipoDeCliente) {
            // alertar que o tipo de cliente é diferente e que os valores dos itens devem recalculados
            Ext.Msg.show({
                title: 'Tipo de Cliente &eacute; diferente',
                msg: 'O <b>Tipo de Cliente</b> selecionado &eacute; diferente do Cliente anterior, '
                   + 'devido a isto, podem haver diferen&ccedil;as nos valores dos itens desta Ordem de Servi&ccedil;o!<br />'
                   + 'Anterior: <b>' + this.cliente.nomeTipoDeCliente + '</b><br />'
                   + 'Novo: <b>' + cliente.nomeTipoDeCliente + '</b>.',
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.WARNING
            });
        }

        this.cliente = cliente;
        this.form.down('#moduleEditOS_codigoCliente').setValue(cliente.codigo);
        this.form.down('#moduleEditOS_nomeCliente').setValue(cliente.nome);
    },

    atualizarValorOS: function() {
        var valOriginal = this.form.down('#osEditValOriginal');
        var valFinal = this.form.down('#osEditValFinal');

        var valorOS = 0;
        this.itensStore.each(function(record){
            valorOS += record.data.valor;
        },this);

        valOriginal.setValue(valorOS);
        valFinal.setValue(valorOS);
    }
});
