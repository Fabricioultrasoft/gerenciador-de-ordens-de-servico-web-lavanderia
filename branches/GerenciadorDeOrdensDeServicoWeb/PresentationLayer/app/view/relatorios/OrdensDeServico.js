
Ext.define('App.view.relatorios.OrdensDeServico', {
    extend: 'App.webDesktop.Module',
    id: 'module-report-os',
    init: function () {
    },

    createWindow: function () {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-report-os');
        if (!win) {
            var panel = this.createPanel();
            win = desktop.createWindow({
                id: 'win-report-os',
                title: 'Relatório de Ordens de Serviço',
                width: 640,
                height: 340,
                iconCls: 'report',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                items: [panel]
            });
        }
        console.log(win);
        win.show();

        return win;
    },

    createPanel: function () {

        var statusStore = Ext.create('App.store.ordensDeServico.StatusStore', { pageSize: 0 });
        statusStore.load();    

        var form = Ext.create('Ext.form.Panel', {
            border: false,
            bodyPadding: 5,
            layout: 'anchor',
            defaults: {
                anchor: '100%',
                labelWidth: 50
            },
            items: [
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top', margins: '0 4 0 0' },
                    items: [
                        { xtype: 'numberfield', name: 'numero', flex : 1, fieldLabel: 'Numero', emptyText: 'Numero da OS', minValue: 0, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'combobox', name: 'codigoStatus', flex : 1, fieldLabel: 'Status', store: statusStore, queryMode: 'local', valueField: 'codigo', displayField: 'nome', emptyText: 'Selecione o status', selectOnFocus: true, forceSelection: true },
                        { xtype: 'numberfield', flex : 1, name: 'valorOriginal', fieldLabel: 'Valor Original', emptyText: 'R$ 0.00', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'numberfield', flex : 1, name: 'valorFinal', fieldLabel: 'Valor Final/Com Desconto', emptyText: 'R$ 0.00', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, margins: '0' }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'datefield', flex : 1, name: 'dataDeAbertura', fieldLabel: 'Abertura', emptyText: 'Data de Abertura', format: 'd/m/Y' },
                        { xtype: 'datefield', flex : 1, name: 'previsaoDeConclusao', fieldLabel: 'Prev. Conclus&atilde;o', emptyText: 'Previsão de Conclusão', format: 'd/m/Y', margins: '0 4' },
                        { xtype: 'datefield', flex : 1, name: 'dataDeFechamento', fieldLabel: 'Fechamento', emptyText: 'Fechamento', format: 'd/m/Y' }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: 'Cliente', layout: 'hbox', defaults: { hideLabel: true },
                    items: [
                        { xtype: 'numberfield', itemId:'codigoClienteReportOS', width: 60, name: 'codigoCliente', emptyText: 'Codigo', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'textfield', itemId:'nomeClienteReportOS', flex : 1, name : 'nomeCliente', emptyText: 'Nome do Cliente', margins: '0 4' },
                        { xtype: 'button', text: 'Add', itemId: 'btnClienteReportOS', iconCls: 'clientes-add-thumb', handler: this.onClienteSearchReportClick, scope: this }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: 'Qtd Registros', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'numberfield', name: 'start', minValue: 1, value: 1, flex: 1, fieldLabel: 'Inicio', emptyText: 'Digite o indice inicial', margins: '0 4 0 0' },
                        { xtype: 'numberfield', name: 'limit', minValue: 0, value: 0, flex: 1, fieldLabel: 'Limite', emptyText: 'Digite o indice final' }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'combobox', fieldLabel: 'Ordenar por', name: 'sort', flex : 1, queryMode: 'local', valueField: 'value', displayField: 'label', emptyText: 'Selecione o a coluna que será ordenada', selectOnFocus: true, forceSelection: true, margins: '0 4 0 0', 
                            store: Ext.create('Ext.data.Store', {
                                fields: ['label', 'value'],
                                data : [
                                    {"label":"Numero", "value":"numero"},
                                    {"label":"Codigo do Cliente", "value":"codigoCliente"},
                                    {"label":"Nome do Cliente", "value":"nomeCliente"},
                                    {"label":"Valor Original", "value":"valorOriginal"},
                                    {"label":"Valor Final", "value":"valorFinal"},
                                    {"label":"Status", "value":"nomeStatus"},
                                    {"label":"Data de Abertura", "value":"dataDeAbertura"},
                                    {"label":"Previsão de conclusão", "value":"previsaoDeConclusao"},
                                    {"label":"Data de Fechamento", "value":"dataDeFechamento"}
                                ]
                            })
                        },
                        { xtype: 'combobox', fieldLabel: 'Dire&ccedil;&atilde;o', name: 'direction', flex : 1, queryMode: 'local', valueField: 'value', displayField: 'label', emptyText: 'Selecione a direção da ordem', selectOnFocus: true, forceSelection: true,
                            store: Ext.create('Ext.data.Store', {
                                fields: ['label', 'value'],
                                data : [
                                    {"label":"Crescente", "value":"ASC"},
                                    {"label":"Decrescente", "value":"DESC"}
                                ]
                            })
                        }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: 'Colunas', defaults: { xtype: 'checkboxfield', name: 'colunas', checked: true, margins: '0 15 0 0' },
                    items: [
                        { boxLabel: 'Numero', inputValue : 'numero' }, 
                        { boxLabel: 'Codigo do Cliente', inputValue: 'codigoCliente' },
                        { boxLabel: 'Nome do Cliente', inputValue: 'nomeCliente' }, 
                        { boxLabel: 'Valor Original', inputValue: 'valorOriginal' },
                        { boxLabel: 'Valor Final', inputValue: 'valorFinal' },
                        { boxLabel: 'Status', inputValue: 'status' },
                        { boxLabel: 'Data de Abertura', inputValue: 'dataAbertura' },
                        { boxLabel: 'Previs&atilde;o de conclus&atilde;o', inputValue: 'previsaoConclusao' },
                        { boxLabel: 'Data de Fechamento', inputValue: 'dataFechamento' },
                        { boxLabel: 'Tapetes', inputValue: 'tapetes', checked: false },
                        { boxLabel: 'Itens', inputValue: 'itens', checked: false },
                        { boxLabel: 'Obs.', inputValue: 'observacoes', checked: false }
                    ]
                }
            ]
        });
        this.form = form;

        var panel = Ext.create('Ext.panel.Panel', {
            border: false,
            autoScroll: true,
            layout: 'auto',
            items: [form],
            buttonAlign: 'center',
            buttons: [
                { text: 'HTML', iconCls: 'html', reportView: 'html', handler: this.gerarRelatorio, scope: this },
                { text: 'Excel', iconCls: 'xls', reportView: 'xls', handler: this.gerarRelatorio, scope: this }
            ]
        });

        return panel;
    },

    gerarRelatorio: function (btn, eventObject, options) {
        console.log(this);
        if (!this.form.getForm().isValid()) {
            genericErrorAlert("Erro", "Preencha os campos corretamente");
            return false; 
        }

        var values = this.form.getValues();
        var arrFilters = new Array();
        var sort = new Array();

        for(var member in values) {
            if(member != "colunas" && member != "start" && member != "limit")
                arrFilters.push(new Ext.util.Filter({ property: member, value: values[member] }));
        }

        sort.push(new Ext.util.Sorter({ property : (values.sort) ? values.sort : 'numero' , direction: (values.direction) ? values.direction : 'ASC' }));

        var param = Ext.String.format('filter={0}&colunas={1}&start={2}&limit={3}&sort={4}&reportView={5}', Ext.encode(arrFilters), values.colunas.join(','), values.start, values.limit, Ext.encode(sort), btn.reportView);
        var newWindow = window.open('/PresentationLayer/app/view/relatorios/OrdensDeServicoTpl.ashx?' + param);
	    if (window.focus) {newWindow.focus()}
    },

    onClienteSearchReportClick: function(btn, event, options) {
        btn.scope.app.getModule("module-ordensDeServico-clientesSearch").createWindow(btn.scope);
    },

    setCliente: function(cliente) {
        this.form.down('#codigoClienteReportOS').setValue(cliente.codigo);
        this.form.down('#nomeClienteReportOS').setValue(cliente.nome);
    }
});
