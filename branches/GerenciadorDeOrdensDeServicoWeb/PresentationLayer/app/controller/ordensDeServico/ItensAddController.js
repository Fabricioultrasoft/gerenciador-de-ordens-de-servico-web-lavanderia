
Ext.define('App.controller.ordensDeServico.ItensAddController', {
    extend: 'Ext.app.Controller',

    models: ['ordensDeServico.ItemModel','ordensDeServico.ItemServicoModel'],

    views: ['ordensDeServico.ItensAddView'],

    stores: ['ordensDeServico.ItensStore','ordensDeServico.ItensServicosStore'],

    init: function () {
        this.control({
            '#grid-itensServicosOS': {
                itemdblclick: this.editItemServico
            },
            '#module-itensOS-add_btnAddItemServicosOS': {
                click: this.onAddItemServicoClick
            },
            '#module-itensOS-add_btnEditItemServicosOS': {
                click: this.onEditItemServicoClick
            },
            '#module-itensOS-add_btnDelItemServicosOS': {
                click: this.onDelItemServicoClick
            },
            '#btn-add-itemOS': {
                click: this.onAddItemClick
            }
        });
    },
    
    //----------------------------------------------------------------------------------------
    onAddItemClick: function (btn, event, options) {
        btn.scope.app.getModule("module-itensOS-add").createWindow();
    },

    //----------------------------------------------------------------------------------------
    editItem: function (grid, record) {
        grid.panel.module.app.getModule("module-itensOS-edit").createWindow({record:record});
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