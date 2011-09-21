
Ext.define('App.controller.ordensDeServico.OrdensDeServicoAddController', {
    extend: 'Ext.app.Controller',

    models: ['ordensDeServico.OrdemDeServicoModel','ordensDeServico.ItemModel'],

    views: ['ordensDeServico.OrdensDeServicoAddView'],

    stores: ['ordensDeServico.OrdensDeServicoStore','ordensDeServico.ItensStore'],

    init: function () {
        this.control({
            '#grid-itensOS': {
                itemdblclick: this.editItem
            },
            '#module-ordensDeServico-add_btnAddItemOS': {
                click: this.onAddItemClick
            },
            '#module-ordensDeServico-add_btnEditItemOS': {
                click: this.onEditItemClick
            },
            '#module-ordensDeServico-add_btnDelItemOS': {
                click: this.onDelItemClick
            },
            '#btnAddClienteOS': {
                click: this.onAddClienteOSClick
            },
            '#btnConfirmAddOS': {
                click: this.onConfirmAddOSClick
            }
        });
    },
    
    //----------------------------------------------------------------------------------------
    onAddItemClick: function (btn, event, options) {
        if(btn.scope.cliente == null) {
            Ext.Msg.show({
                title: 'Dados incompletos',
                msg: 'Para incluir um Item na Ordem de Servi&ccedil;o <b>&eacute; preciso informar antes o Cliente</b>, '
                   + 'pois os valores dos Itens são calculados com base no Tipo de Cliente selecionado.',
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.WARNING
            });
        } else {
            btn.scope.app.getModule("module-itensOS").createWindow({targetModule: btn.scope});
        }
    },

    //----------------------------------------------------------------------------------------
    editItem: function (grid, record) {
        grid.panel.module.app.getModule("module-itensOS").createWindow({record:record});
    },

    //----------------------------------------------------------------------------------------
    onEditItemClick: function(btn, event, options) {
        var grid = btn.scope.grid;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    //----------------------------------------------------------------------------------------
    onDelItemClick: function(btn, event, options) {
        var sm = btn.scope.gridMeiosDeContato.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir este meio de contato',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Meio de contato: ' + sm.getSelection()[0].data.nomeTipoDeContato + '<br />Contato: ' + sm.getSelection()[0].data.contato,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    btn.scope.gridMeiosDeContato.getStore().remove(sm.getSelection());
                    if (btn.scope.gridMeiosDeContato.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: btn.id,
            icon: Ext.Msg.QUESTION
        });
    },

    //----------------------------------------------------------------------------------------
    onAddClienteOSClick: function(btn, event, options) {
        btn.scope.app.getModule("module-ordensDeServico-clientesSearch").createWindow(btn.scope);
    },

    //----------------------------------------------------------------------------------------
    onAddClienteClick: function(btn, event, options) {
        if (!btn.scope.formDadosPrimarios.getForm().isValid()) {
            btn.scope.tabPanel.setActiveTab(0);
            genericErrorAlert("Erro", "Dados prim&aacute;rios inv&aacute;lidos, passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        }

        var values = btn.scope.formDadosPrimarios.getValues();
        var tipoDeCliente = btn.scope.tiposDeClientesStore.getAt(btn.scope.tiposDeClientesStore.find('codigo', values.codigoTipoDeCliente));
        
        var meiosDeContato = [];
        btn.scope.gridMeiosDeContato.getStore().each(function(record){
            meiosDeContato.push(record.data);
        });

        var enderecos = [];
        btn.scope.gridEnderecos.getStore().each(function(record){
            enderecos.push(record.data);
        });

        var r = Ext.ModelManager.create({
            codigo: 0,
            ativo: true,
            nome: values.nome,
            conjuge: values.conjuge,
            codigoTipoDeCliente: tipoDeCliente.data.codigo,
            nomeTipoDeCliente: tipoDeCliente.data.nome,
            ativoTipoDeCliente: tipoDeCliente.data.ativo,
            sexo: values.sexo,
            dataDeNascimento: values.dataDeNascimento,
            rg: values.rg,
            cpf: values.cpf,
            observacoes: values.observacoes,
            dataDeCadastro: '',
            dataDeAtualizacao: '',
            meiosDeContato: meiosDeContato,
            enderecos: enderecos
        }, 'App.model.clientes.ClienteModel');

        btn.scope.mainPanel.setLoading( "Cadastrando...", true );

        var storeClientes = null;
        try { storeClientes = btn.scope.app.getModule("module-clientes-search").clientesStore; }catch(e){}

        if(storeClientes) {
            storeClientes.insert(0, r);
            storeClientes.sync();
            storeClientes.module.gridClientes.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
            btn.up('window').close();
        }
        else {
            r.setProxy( Ext.create('App.store.clientes.ClientesStore',{}).getProxy() );
            r.save({
                success: function(ed) {
                    btn.up('window').close();
                    Ext.notification.msg('A&ccedil;&atilde;o Conclu&iacute;da', 'Cliente cadastrado!');
                },
                failure: function(record, operation) {
                    btn.scope.mainPanel.setLoading( false, true );
                    genericErrorAlert('Erro ao cadastrar', 'Erro inesperado, contate o fornecedor');
                }
            });
        }
    }
});