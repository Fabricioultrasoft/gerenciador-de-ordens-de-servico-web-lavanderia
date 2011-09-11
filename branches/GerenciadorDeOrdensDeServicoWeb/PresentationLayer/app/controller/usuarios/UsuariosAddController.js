
Ext.define('App.controller.usuarios.UsuariosAddController', {
    extend: 'Ext.app.Controller',

    models: ['usuarios.UsuarioModel'],

    views: ['usuarios.UsuariosAddView'],

    stores: ['usuarios.UsuariosStore'],

    init: function () {
        this.control({
            '#btn-add-usuario': {
                click: this.onAddUsuarioClick
            }
        });
    },


    onAddUsuarioClick: function(btn, event, options) {
        
        var values = btn.scope.form.getValues();
        
        if (!btn.scope.form.getForm().isValid()) {
            genericErrorAlert("Erro", "Dados inv&aacute;lidos, passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        } else if(values.senha != values.confirmSenha) {
            genericErrorAlert("Erro", "Senhas diferentes, digite novamente a senha nas duas caixas de texto e certifique-se de que sejam iguais");
            return false;
        }

        var r = Ext.ModelManager.create({
            codigo: 0,
            nome: values.nome,
            senha: values.senha
        }, 'App.model.usuarios.UsuarioModel');

        btn.scope.mainPanel.setLoading( "Cadastrando...", true );

        var storeUsuarios = null;
        try { storeUsuarios = btn.scope.app.getModule("module-usuarios-search").usuariosStore; }catch(e){}

        if(storeUsuarios) {
            storeUsuarios.insert(0, r);
            storeUsuarios.sync();
            btn.up('window').close();
        }
        else {
            r.setProxy( Ext.create('App.store.usuarios.UsuariosStore',{}).getProxy() );
            r.save({
                success: function(ed) {
                    btn.up('window').close();
                    Ext.notification.msg('A&ccedil;&atilde;o Conclu&iacute;da', 'Usuario cadastrado!');
                },
                failure: function(record, operation) {
                    btn.scope.mainPanel.setLoading( false, true );
                    genericErrorAlert('Erro ao cadastrar', 'Erro inesperado, contate o fornecedor');
                }
            });
        }
    }
});