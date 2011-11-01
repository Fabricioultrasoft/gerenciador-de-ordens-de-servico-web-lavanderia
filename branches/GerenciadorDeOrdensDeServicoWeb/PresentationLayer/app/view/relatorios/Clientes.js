
Ext.define('App.view.relatorios.Clientes', {
    extend: 'App.webDesktop.Module',
    id: 'module-report-clientes',
    init: function () {
    },

    createWindow: function () {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-report-clientes');
        if (!win) {
            var panel = this.createPanel();
            win = desktop.createWindow({
                id: 'win-report-clientes',
                title: 'Relatório de Clientes',
                width: 600,
                height: 340,
                iconCls: 'report',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                items: [panel]
            });
        }
        win.show();

        return win;
    },

    createPanel: function () {

        var tiposDeClientesStore = Ext.create('App.store.clientes.TiposDeClientesStore', { pageSize: 0 });
        tiposDeClientesStore.load({ params: { ativo: true} });

        var form = Ext.create('Ext.form.Panel', {
            border: false,
            bodyPadding: 5,
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'checkbox', name: 'ativo', fieldLabel: 'Ativo', width: 50, inputValue: 1, checked: true },
                        { xtype: 'numberfield', name: 'codigo', minValue: 0, flex: 1, fieldLabel: 'Codigo', emptyText: '0000', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, margins: '0 4' },
                        { xtype: 'combobox', name: 'codigoTipoDeCliente', flex: 1, fieldLabel: 'Tipo de cliente', store: tiposDeClientesStore, queryMode: 'local', valueField: 'codigo', displayField: 'nome', emptyText: 'Selecione o tipo de cliente', selectOnFocus: true, forceSelection: true }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top' },
                    items: [
                        { xtype: 'textfield', name: 'nome', flex: 1, fieldLabel: 'Nome', emptyText: 'Digite o nome do cliente', maxLength: 50, margins: '0 4 0 0'},
                        { xtype: 'textfield', name: 'conjuge', flex: 1, fieldLabel: 'Conjuge', emptyText: 'Digite o nome do cônjuge', maxLength: 50 }
                    ]
                },
                { xtype: 'fieldcontainer', fieldLabel: '', layout: 'hbox', defaults: { labelAlign: 'top', margins: '0 4 0 0' },
                    items: [
                        { xtype: 'radiogroup', fieldLabel: 'Sexo', width: 200, items: [{ xtype: 'radio', boxLabel: 'Masculino', inputValue: 1, name: 'sexo', checked: true }, { xtype: 'radio', boxLabel: 'Feminino', inputValue: 2, name: 'sexo'}] },
                        { xtype: 'datefield', name: 'dataDeNascimento', flex: 1, fieldLabel: 'Nascimento', emptyText: 'dd/mm/aaaa', format: 'd/m/Y' },
                        { xtype: 'textfield', name: 'rg', flex: 1, fieldLabel: 'RG', emptyText: 'Digite o RG', maxLength: 12 },
                        { xtype: 'textfield', name: 'cpf', flex: 1, fieldLabel: 'CPF', emptyText: 'Digite o CPF', maxLength: 14, margins: '0' }
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
                                    {"label":"Ativo", "value":"ativo"},
                                    {"label":"Codigo", "value":"codigo"},
                                    {"label":"Nome", "value":"nome"},
                                    {"label":"Conjuge", "value":"conjuge"},
                                    {"label":"Tipo", "value":"nomeTipoDeCliente"},
                                    {"label":"Data de Nascimento", "value":"dataDeNascimento"},
                                    {"label":"Sexo", "value":"strSexo"},
                                    {"label":"RG", "value":"rg"},
                                    {"label":"CPF", "value":"cpf"}
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
                        { boxLabel: 'Ativo', inputValue : 'ativo' },
                        { boxLabel: 'Codigo', inputValue : 'codigo' },
                        { boxLabel: 'Nome', inputValue : 'nome' }, 
                        { boxLabel: 'Conjuge', inputValue: 'conjuge' }, 
                        { boxLabel: 'Tipo', inputValue: 'tipoCliente' },
                        { boxLabel: 'Data de Nascimento', inputValue: 'nascimento' },
                        { boxLabel: 'Sexo', inputValue: 'sexo' },
                        { boxLabel: 'RG', inputValue: 'rg' },
                        { boxLabel: 'CPF', inputValue: 'cpf' },
                        { boxLabel: 'Obs.', inputValue: 'observacoes', checked: false },
                        { boxLabel: 'Contatos', inputValue: 'meiosDeContato', checked: false },
                        { boxLabel: 'Endereços', inputValue: 'Endereco', checked: false }
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
                { itemId: 'btnGerarRelatorioDeClientes_html', text: 'HTML', iconCls: 'html', reportView: 'html', handler: this.gerarRelatorio, scope: this },
                { itemId: 'btnGerarRelatorioDeClientes_excel', text: 'Excel', iconCls: 'xls', reportView: 'xls', handler: this.gerarRelatorio, scope: this }
            ]
        });

        return panel;
    },

    gerarRelatorio: function (btn, eventObject, options) {
        if (!this.form.getForm().isValid()) {
            genericErrorAlert("Erro", "Preencha os campos corretamente");
            return false; 
        }

        var values = this.form.getValues();
        var arrFilters = new Array();
        var sort = new Array();

        for(var member in values) {
            if(member != "colunas" && member != "start" && member != "limit" && member != "sort" && member != "direction" )
                arrFilters.push(new Ext.util.Filter({ property: member, value: values[member] }));
        }

        sort.push(new Ext.util.Sorter({ property : (values.sort) ? values.sort : 'codigo' , direction: (values.direction) ? values.direction : 'ASC' }));

        var param = Ext.String.format('filter={0}&colunas={1}&start={2}&limit={3}&sort={4}&reportView={5}', Ext.encode(arrFilters), values.colunas.join(','), values.start, values.limit, Ext.encode(sort), btn.reportView);
        var newWindow = window.open('/PresentationLayer/app/view/relatorios/ClientesTpl.ashx?' + param);
	    if (window.focus) {newWindow.focus()}
    }
});
