
Ext.define('App.view.clientes.ClientesAddView', {
    extend: 'App.webDesktop.Module',
    id: 'win-clientes-add',
    init: function () {
//        this.launcher = {
//            text: 'Adicionar Cliente',
//            iconCls: 'clientes-add-thumb',
//            handler: this.createWindow,
//            scope: this
//        };
    },

    createWindow: function (options) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-clientes-add');
        if (!win) {
            var clientesPanel = this.createPanel(options);
            win = desktop.createWindow({
                id: 'win-clientes-add',
                title: 'Adicionar Novo Cliente',
                width: 600,
                height: 480,
                iconCls: 'clientes-add-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [clientesPanel]
            });
        }
        win.show();
        return win;
    },

    createPanel: function (options) {
        this.options = options;

        var tiposDeClientesStore = Ext.create('App.store.clientes.TiposDeClientesStore', { pageSize: 0 });
        tiposDeClientesStore.load({ params: { ativo: true} });
        
        this.tiposDeClientesStore = tiposDeClientesStore;

        //--------------------------------------------------------------------
        var formDadosPrimarios = Ext.create('Ext.form.Panel', {
            title: 'Dados Prim&aacute;rios',
            iconCls: 'clientes-dados',
            bodyPadding: 5,
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [
                { xtype: 'textfield', name: 'nome', fieldLabel: 'Nome', emptyText: 'Digite o nome do cliente', maxLength: 100, allowBlank: false, blankText: 'O NOME do cliente é obrigatório' },
                { xtype: 'textfield', name: 'conjuge', fieldLabel: 'Conjuge', emptyText: 'Digite o nome do cônjuge', maxLength: 100 },
                { xtype: 'combobox', name: 'codigoTipoDeCliente', fieldLabel: 'Tipo de cliente', store: tiposDeClientesStore, queryMode: 'local', valueField: 'codigo', displayField: 'nome', emptyText: 'Selecione o tipo de cliente', selectOnFocus: true, forceSelection: true, allowBlank: false, blankText: 'O TIPO do cliente é obrigatório' },
                { xtype: 'datefield', name: 'dataDeNascimento', fieldLabel: 'Nascimento', emptyText: 'dd/mm/aaaa', format: 'd/m/Y' },
                { xtype: 'radiogroup', fieldLabel: 'Sexo', items: [{ xtype: 'radio', boxLabel: 'Masculino', inputValue: 1, name: 'sexo', checked: true }, { xtype: 'radio', boxLabel: 'Feminino', inputValue: 2, name: 'sexo'}] },
                { xtype: 'textfield', name: 'rg', fieldLabel: 'RG', emptyText: 'Digite o RG', maxLength: 12 },
                { xtype: 'textfield', name: 'cpf', fieldLabel: 'CPF', emptyText: 'Digite o CPF', maxLength: 14 },
                { xtype: 'textarea', name: 'observacoes', fieldLabel: 'Observa&ccedil;&otilde;es', emptyText: 'Observações gerais', height: 100 }
            ]
        });
        this.formDadosPrimarios = formDadosPrimarios;

        //--------------------------------------------------------------------
        var formMeiosDeContato = Ext.create('Ext.form.Panel', {
            bodyPadding: 5,
            border: false,
            layout: 'anchor',
            height: 120,
            region: 'north',
            defaults: {
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'combobox',
                    name: 'codigoTipoDeContato',
                    fieldLabel: 'Tipo de Contato',
                    emptyText: 'Selecione o meio de contato',
                    store: Ext.create('Ext.data.Store', {
                        fields: ['codigo', 'nome'],
                        data: [
                            { 'codigo': 1, 'nome': "Tel. Residencial" },
                            { 'codigo': 2, 'nome': "Tel. Comercial" },
                            { 'codigo': 3, 'nome': "Celular" },
                            { 'codigo': 4, 'nome': "E-mail" },
                            { 'codigo': 5, 'nome': "Radio" },
                            { 'codigo': 6, 'nome': "Outros" }
                        ]
                    }),
                    queryMode: 'local',
                    displayField: 'nome',
                    valueField: 'codigo',
                    selectOnFocus: true,
                    forceSelection: true,
                    allowBlank: false, blankText: 'Um Tipo de Contato deve ser selecionado'
                },
                { xtype: 'textfield', name: 'contato', fieldLabel: 'Contato', emptyText: 'Telefone, e-mail, etc...', allowBlank: false, blankText: 'Um contato deve ser informado' },
                { xtype: 'textfield', name: 'descricao', fieldLabel: 'Descri&ccedil;&atilde;o', emptyText: '(Ex.: Telefone para recados, etc...)' }
            ],
            buttonAlign: 'center',
            buttons: [{
                itemId: 'btnAddMeioDeContato',
                text: 'Adicionar meio de contato',
                iconCls: 'btn-add',
                scope: this
            }, {
                itemId: 'btnLimparMeioDeContato',
                text: 'Limpar',
                iconCls: 'btn-limpar',
                scope: this
            }]
        });
        this.formMeiosDeContato = formMeiosDeContato;

        //--------------------------------------------------------------------
        var gridMeiosDeContato = Ext.create('Ext.grid.Panel', {
            border: false,
            region: 'center',
            store: Ext.create('App.store.clientes.MeiosDeContatoStore', {}),
            style: {
                borderTop: '1px solid #99BCE8'
            },
            columns: [
                { text: 'Tipo de Contato', dataIndex: 'nomeTipoDeContato' },
                { text: 'Contato', dataIndex: 'contato', width: 160 },
                { text: 'Descri&ccedil;&atilde;o', dataIndex: 'descricao', flex: 1 }
            ],
            tbar: [{
                itemId: 'btnDelMeioDeContato',
                text: 'Remover',
                tooltip: 'Remover o contato selecionado na lista',
                iconCls: 'btn-del',
                disabled: true,
                scope: this
            }],
            listeners: {
                'selectionchange': function (view, records) {
                    gridMeiosDeContato.down('#btnDelMeioDeContato').setDisabled(!records.length);
                }
            }
        });
        this.gridMeiosDeContato = gridMeiosDeContato;


        //--------------------------------------------------------------------
        var paisesStore = Ext.create('App.store.enderecos.PaisesStore', { pageSize: 0 });
        var estadosStore = Ext.create('App.store.enderecos.EstadosStore', { pageSize: 0 });
        var cidadesStore = Ext.create('App.store.enderecos.CidadesStore', { pageSize: 0 });
        var bairrosStore = Ext.create('App.store.enderecos.BairrosStore', { pageSize: 0 });
        var LogradourosStore = Ext.create('App.store.enderecos.LogradourosStore', { pageSize: 0 });

        this.paisesStore = paisesStore;
        this.estadosStore = estadosStore;
        this.cidadesStore = cidadesStore;
        this.bairrosStore = bairrosStore;
        this.LogradourosStore = LogradourosStore;

        paisesStore.load();

        //------------------------
        var comboPais = Ext.create('Ext.form.ComboBox', { store: paisesStore, name: 'codigoPais', fieldLabel: 'Pa&iacute;s', emptyText: 'Selecione o país', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um Pa&iacute;s deve ser selecionado', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this });
        var comboEstado = Ext.create('Ext.form.ComboBox', { store: estadosStore, name: 'codigoEstado', fieldLabel: 'Estado', emptyText: 'Selecione o estado', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um Estado deve ser selecionado', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this, disabled: true });
        var comboCidade = Ext.create('Ext.form.ComboBox', { store: cidadesStore, name: 'codigoCidade', fieldLabel: 'Cidade', emptyText: 'Selecione a cidade', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Uma Cidade deve ser selecionada', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this, disabled: true });
        var comboBairro = Ext.create('Ext.form.ComboBox', { store: bairrosStore, name: 'codigoBairro', fieldLabel: 'Bairro', emptyText: 'Selecione o bairro', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um Bairro deve ser selecionado', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this, disabled: true });
        var comboLogradouro = Ext.create('Ext.form.ComboBox', { store: LogradourosStore, name: 'codigoLogradouro', fieldLabel: 'Logradouro', emptyText: 'Selecione o logradouro', displayField: 'nome', valueField: 'codigo', allowBlank: false, blankText: 'Um logradouro deve ser selecionado', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this, disabled: true, listConfig: {getInnerTpl: function () { return '<div>{nomeTipoDeLogradouro} {nome} {cep}</div>'; }} });


        this.comboPais = comboPais;
        this.comboEstado = comboEstado;
        this.comboCidade = comboCidade;
        this.comboBairro = comboBairro;
        this.comboLogradouro = comboLogradouro;
        //------------------------

        comboPais.on('select', function (combo, record, index) {
            this.comboEstado.clearValue(); this.comboEstado.enable();
            this.comboCidade.clearValue(); this.comboCidade.disable();
            this.comboBairro.clearValue(); this.comboBairro.disable();
            this.comboLogradouro.clearValue(); this.comboLogradouro.disable();
            this.estadosStore.load({ params: { codigoPais: combo.getValue()} });
        }, this);

        comboEstado.on('select', function (combo, record, index) {
            this.comboCidade.clearValue(); this.comboCidade.enable();
            this.comboBairro.clearValue(); this.comboBairro.disable();
            this.comboLogradouro.clearValue(); this.comboLogradouro.disable();
            this.cidadesStore.load({ params: { codigoEstado: combo.getValue()} });
        }, this);

        comboCidade.on('select', function (combo, record, index) {
            this.comboBairro.clearValue(); this.comboBairro.enable();
            this.comboLogradouro.clearValue(); this.comboLogradouro.disable();
            this.bairrosStore.load({ params: { codigoCidade: combo.getValue()} });
        }, this);

        comboBairro.on('select', function (combo, record, index) {
            this.comboLogradouro.clearValue(); this.comboLogradouro.enable();
            this.LogradourosStore.load({ params: { codigoBairro: combo.getValue()} });
        }, this);

        //--------------------------


        var formEnderecos = Ext.create('Ext.form.Panel', {
            bodyPadding: 5,
            border: false,
            layout: 'anchor',
            height: 250,
            region: 'north',
            defaults: {
                anchor: '100%'
            },
            items: [comboPais, comboEstado, comboCidade, comboBairro, comboLogradouro,
                {
                    xtype: 'numberfield',
                    name: 'numero',
                    fieldLabel: 'N&uacute;mero',
                    emptyText: 'Numero da residencia',
                    hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false
                },
                {
                    xtype: 'textfield',
                    name: 'complemento',
                    fieldLabel: 'Complemento',
                    emptyText: 'Complemento para o endereço'
                },
                {
                    xtype: 'textfield',
                    name: 'pontoDeReferencia',
                    fieldLabel: 'Ponto de Ref.',
                    emptyText: 'Ponto de Referencia'
                }
            ],
            buttonAlign: 'center',
            buttons: [{
                itemId: 'btnAddEndereco',
                text: 'Adicionar endere&ccedil;o',
                iconCls: 'btn-add',
                scope: this
            }, {
                itemId: 'btnLimparEndereco',
                text: 'Limpar',
                iconCls: 'btn-limpar',
                scope: this
            }]
        });
        this.formEnderecos = formEnderecos;

        //--------------------------------------------------------------------
        var gridEnderecos = Ext.create('Ext.grid.Panel', {
            border: false,
            region: 'center',
            style: {
                borderTop: '1px solid #99BCE8'
            },
            store: Ext.create('App.store.enderecos.EnderecosStore', {}),
            columns: [
                { text: 'tipo', dataIndex: 'nomeTipoDeLogradouro' },
                { text: 'Logradouro', dataIndex: 'nomeLogradouro' },
                { text: 'cep', dataIndex: 'cep' },
                { text: 'Numero', dataIndex: 'numero' },
                { text: 'Complemento', dataIndex: 'complemento' },
                { text: 'Ponto de referencia', dataIndex: 'pontoDeReferencia' },
                { text: 'Bairro', dataIndex: 'nomeBairro' },
                { text: 'Cidade', dataIndex: 'nomeCidade' },
                { text: 'Estado', dataIndex: 'nomeEstado' },
                { text: 'Pa&iacute;s', dataIndex: 'nomePais' }
            ],
            tbar: [{
                itemId: 'btnDelEndereco',
                text: 'Remover',
                tooltip: 'Remover o endere&ccedil;o selecionado na lista',
                iconCls: 'btn-del',
                scope: this,
                disabled: true
            }],
            listeners: {
                'selectionchange': function (view, records) {
                    gridEnderecos.down('#btnDelEndereco').setDisabled(!records.length);
                }
            }
        });
        this.gridEnderecos = gridEnderecos;

        //--------------------------------------------------------------------
        var tabPanel = Ext.create('Ext.tab.Panel', {
            resizeTabs: true,
            items: [
                formDadosPrimarios,
                {
                    title: 'Meios de Contato',
                    iconCls: 'talk',
                    layout: 'border',
                    border: false,
                    items: [formMeiosDeContato, gridMeiosDeContato]
                },
                {
                    title: 'Endere&ccedil;os',
                    iconCls: 'location-thumb',
                    layout: 'border',
                    border: false,
                    items: [formEnderecos, gridEnderecos]
                }]
        });
        this.tabPanel = tabPanel;

        //--------------------------------------------------------------------
        var mainPanel = Ext.create('Ext.panel.Panel', {
            border: false,
            layout: 'fit',
            items: [tabPanel],
            buttonAlign: 'center',
            buttons: [{ text: 'Adicionar Cliente', itemId: 'btn-add-cliente', iconCls: 'clientes-add-thumb', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    }
});
