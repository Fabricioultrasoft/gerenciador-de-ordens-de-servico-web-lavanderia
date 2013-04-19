
Ext.define('App.controller.clientes.ClientesAddController', {
    extend: 'Ext.app.Controller',

    models: ['clientes.ClienteModel','clientes.MeioDeContatoModel','enderecos.EnderecoModel'],

    views: ['clientes.ClientesAddView'],

    stores: ['clientes.ClientesStore','clientes.TiposDeClientesStore','clientes.MeiosDeContatoStore','enderecos.EnderecosStore'],

    init: function () {
        this.control({
            '#btnAddMeioDeContato': {
                click: this.onAddMeioDeContatoClick
            },
            '#btnDelMeioDeContato': {
                click: this.onDelMeioDeContatoClick
            },
            '#btnAddEndereco': {
                click: this.onAddEnderecoClick
            },
            '#btnDelEndereco': {
                click: this.onDelEnderecoClick
            },
            '#btnLimparMeioDeContato': {
                click: this.onLimparMeioDeContatoClick
            },
            '#btnLimparEndereco': {
                click: this.onLimparEnderecoClick
            },
            '#btn-add-cliente': {
                click: this.onAddClienteClick
            }
        });
    },
    //----------------------------------------------------------------------------------------
    onAddMeioDeContatoClick: function (btn, event, options) {
        if (!btn.scope.formMeiosDeContato.getForm().isValid()) {
            genericErrorAlert("Erro", "Dados inv&aacute;lidos, passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        }

        var values = btn.scope.formMeiosDeContato.getValues();
        var tiposDeContatoStore = btn.scope.formMeiosDeContato.items.getAt(0).store;
         
        var r = Ext.ModelManager.create({
            codigo: 0,
            codigoTipoDeContato: values.codigoTipoDeContato,
            nomeTipoDeContato: tiposDeContatoStore.getAt(tiposDeContatoStore.find('codigo', values.codigoTipoDeContato)).get('nome'),
            contato: values.contato,
            descricao: values.descricao
        }, 'App.model.clientes.MeioDeContatoModel');

        btn.scope.gridMeiosDeContato.getStore().insert(0, r);
        btn.scope.formMeiosDeContato.getForm().reset();
    },
    //----------------------------------------------------------------------------------------
    onDelMeioDeContatoClick: function(btn, eventObject, options) {
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
    onAddEnderecoClick: function (btn, event, options) {
        if (!btn.scope.formEnderecos.getForm().isValid()) {
            genericErrorAlert("Erro", "Dados inv&aacute;lidos, passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        }

        var values = btn.scope.formEnderecos.getValues();
        
        var endereco = btn.scope.LogradourosStore.getAt(btn.scope.LogradourosStore.find('codigo', values.codigoLogradouro));

        var r = Ext.ModelManager.create({
            codigo: 0,
            complemento: values.complemento,
            pontoDeReferencia: values.pontoDeReferencia,
            numero: values.numero,
            cep: endereco.data.cep,
            codigoLogradouro: endereco.data.codigo,
            codigoTipoDeLogradouro: endereco.data.codigoTipoDeLogradouro,
            codigoBairro: endereco.data.codigoBairro,
            codigoCidade: endereco.data.codigoCidade,
            codigoEstado: endereco.data.codigoEstado,
            codigoPais: endereco.data.codigoPais,
            nomeLogradouro: endereco.data.nome,
            nomeTipoDeLogradouro: endereco.data.nomeTipoDeLogradouro,
            nomeBairro: endereco.data.nomeBairro,
            nomeCidade: endereco.data.nomeCidade,
            nomeEstado: endereco.data.nomeEstado,
            nomePais: endereco.data.nomePais
        }, 'App.model.enderecos.EnderecoModel');

        btn.scope.gridEnderecos.getStore().insert(0, r);
        btn.scope.formEnderecos.getForm().reset();
    },
    //----------------------------------------------------------------------------------------
    onDelEnderecoClick: function(btn, eventObject, options) {
        var sm = btn.scope.gridEnderecos.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir este endere&ccedil;o',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Endere&ccedil;o: ' + sm.getSelection()[0].data.nomeTipoDeLogradouro + ' ' + sm.getSelection()[0].data.nomeLogradouro 
               + ', n&ordm; ' + sm.getSelection()[0].data.numero
               + '<br />Bairro: ' + sm.getSelection()[0].data.nomeBairro
               + '<br />Cidade: ' + sm.getSelection()[0].data.nomeCidade
               + '<br />Estado: ' + sm.getSelection()[0].data.nomeEstado
               + '<br />Pa&iacute;s: ' + sm.getSelection()[0].data.nomePais,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    btn.scope.gridEnderecos.getStore().remove(sm.getSelection());
                    if (btn.scope.gridEnderecos.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: btn.id,
            icon: Ext.Msg.QUESTION
        });
    },
    //----------------------------------------------------------------------------------------
    onLimparMeioDeContatoClick: function (btn, event, options) {
        btn.scope.formMeiosDeContato.getForm().reset();
    },
    //----------------------------------------------------------------------------------------
    onLimparEnderecoClick: function (btn, event, options) {
        btn.scope.formEnderecos.getForm().reset();

        btn.scope.comboEstado.clearValue(); btn.scope.comboEstado.disable();
        btn.scope.comboCidade.clearValue(); btn.scope.comboCidade.disable();
        btn.scope.comboBairro.clearValue(); btn.scope.comboBairro.disable();
        btn.scope.comboLogradouro.clearValue(); btn.scope.comboLogradouro.disable();
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
                    genericErrorAlert('Erro ao cadastrar', (operation.error) ? operation.error : 'Erro inesperado, contate o fornecedor');
                }
            });
        }
    }
});