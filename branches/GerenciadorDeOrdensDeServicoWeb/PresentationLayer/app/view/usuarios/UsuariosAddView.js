
Ext.define('App.view.usuarios.UsuariosAddView', {
    extend: 'App.webDesktop.Module',
    id: 'module-usuarios-add',
    init: function () {
    },

    createWindow: function (options) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-usuarios-add');
        if (!win) {
            var usuariosPanel = this.createPanel(options);
            win = desktop.createWindow({
                id: 'win-usuarios-add',
                title: 'Adicionar Novo Usuario',
                width: 300,
                height: 170,
                iconCls: 'user-add',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [usuariosPanel]
            });
        }
        win.show();
        return win;
    },

    createPanel: function (options) {
        this.options = options;

        //--------------------------------------------------------------------
        var form = Ext.create('Ext.form.Panel', {
            bodyPadding: 5,
            layout: 'anchor',
            defaults: {
                anchor: '100%',
                allowBlank: false, 
                blankText: 'Este campo é obrigatório'
            },
            items: [
                { xtype: 'textfield', name: 'nome', fieldLabel: 'Nome', emptyText: 'Digite o nome do usuário', maxLength: 100 },
                { xtype: 'textfield', name: 'senha', fieldLabel: 'Senha', emptyText: 'Digite a senha do usuário', inputType: 'password' },
                { xtype: 'textfield', name: 'confirmSenha', fieldLabel: 'Confirmar Senha', emptyText: 'Digite novamente a senha', inputType: 'password' }
            ]
        });
        this.form = form;

        //--------------------------------------------------------------------
        var mainPanel = Ext.create('Ext.panel.Panel', {
            border: false,
            layout: 'fit',
            items: [form],
            buttonAlign: 'center',
            buttons: [{ text: 'Adicionar Usuario', itemId: 'btn-add-usuario', iconCls: 'user-add', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    }
});
