
Ext.define('App.controller.usuarios.UsuariosEditController', {
    extend: 'Ext.app.Controller',

    models: ['usuarios.UsuarioModel'],

    views: ['usuarios.UsuariosEditView'],

    stores: ['usuarios.UsuariosStore'],

    init: function () {
        this.control({
            '#btn-atualizar-usuario': {
                click: this.onAtualizarUsuarioClick
            }
        });
    },


    onAtualizarUsuarioClick: function(btn, event, options) {
        
        var values = btn.scope.form.getValues();
        var record = btn.scope.form.getRecord();
        
        if (!btn.scope.form.getForm().isValid()) {
            genericErrorAlert("Erro", "Dados inv&aacute;lidos, passe o mouse sobre os campos em vermelho para mais detalhes");
            return false;
        } else if(values.senha != values.confirmSenha) {
            genericErrorAlert("Erro", "Senhas diferentes, digite novamente a senha nas duas caixas de texto e certifique-se de que sejam iguais");
            return false;
        }
        
        record.set(values);
        record.store.sync();
        btn.up('window').close();
    }
});