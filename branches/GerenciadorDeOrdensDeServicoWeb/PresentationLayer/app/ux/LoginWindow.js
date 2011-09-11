
Ext.define('App.ux.LoginWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.loginwindow',
    iconCls: 'lock',
    title: 'Autentica&ccedil;&atilde;o de Usu&aacute;rio',
    closable: false,
    constrain: true,
    width: 300,
    height: 125,
    layout: 'fit',
    
    initComponent: function () {

        var form = Ext.create('Ext.form.Panel',{
            border: false,
            bodyPadding: 5,
            layout: 'anchor',
            fieldDefaults: {
                labelAlign: 'right',
                xtype: 'textfield',
                labelWidth: 50,
                anchor: '100%',
                allowBlank: false,
                blankText: 'Este campo &eacute; de preenchimento obrigat&oacute;rio'
            },
            defaultType: 'textfield',
            items: [
            { name: 'nome', fieldLabel: 'Nome', emptyText: 'Digite o nome do usuário' }, 
            { name: 'senha', fieldLabel: 'Senha', emptyText: 'Digite a senha do usuário', inputType: 'password' }
            ],
            buttons: [{ xtype: 'button', text: 'Entrar', iconCls: 'app-go', handler: this.onBtnEntrarClick, scope: this }]
        });
        this.form = form;
        this.items = form;

        this.callParent(arguments);
    },

    onBtnEntrarClick: function () {

        // verifica se o usuario preencheu os campos
        if (!this.form.getForm().isValid()) {
            genericErrorAlert("Erro de Autentica&ccedil;&atilde;o", "Preencha todos os campos");
            return false; 
        }

        this.form.setLoading('Autenticando...');
        var values = this.form.getValues();

        Ext.Ajax.request({
            url: this.url,
            method: 'POST',
            scope: this,
            params: Ext.applyIf({
                nome: values.nome,
                senha: values.senha
            }, this.params),
            success: function (responseObj) {
                var response = Ext.decode(responseObj.responseText);
                if(response.success) {
                    this.form.setLoading('Redirecionando...');
                    window.location.href = response.redirect;
                } else {
                    genericErrorAlert("Erro de Autentica&ccedil;&atilde;o", response.message);
                    this.form.setLoading(false);
                }
            },
            failure: function(response, opts) {
                genericExceptionHandler(null, response, null, opts);
                this.form.setLoading(false);
            }
        });
    }
});
