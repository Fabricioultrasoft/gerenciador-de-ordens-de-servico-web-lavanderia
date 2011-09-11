
Ext.define('App.view.usuarios.UsuariosEditView', {
    extend: 'App.webDesktop.Module',
    id: 'module-usuarios-edit',
    init: function () {
    },

    createWindow: function (options) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-usuarios-edit');
        if (!win) {
            var usuariosPanel = this.createPanel(options);
            win = desktop.createWindow({
                id: 'win-usuarios-edit',
                title: 'Alterar Usuario',
                width: 300,
                height: 190,
                iconCls: 'user-edit',
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
                { xtype: 'numberfield', name: 'codigo', fieldLabel: 'Codigo', editable: false, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                { xtype: 'textfield', name: 'nome', fieldLabel: 'Nome', emptyText: 'Digite o nome do usuário', maxLength: 100 },
                { xtype: 'textfield', name: 'senha', fieldLabel: 'Senha', emptyText: 'Digite a senha do usuário', inputType: 'password' },
                { xtype: 'textfield', name: 'confirmSenha', fieldLabel: 'Confirmar Senha', emptyText: 'Digite novamente a senha', inputType: 'password' }
            ]
        });
        this.form = form;
        this.form.loadRecord(options.record);

        //--------------------------------------------------------------------
        var mainPanel = Ext.create('Ext.panel.Panel', {
            border: false,
            layout: 'fit',
            items: [form],
            buttonAlign: 'center',
            buttons: [{ text: 'Alterar Usuario', itemId: 'btn-atualizar-usuario', iconCls: 'user-edit', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    }
});
