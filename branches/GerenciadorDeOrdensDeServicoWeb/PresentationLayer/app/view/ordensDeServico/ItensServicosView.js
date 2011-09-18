
Ext.define('App.view.ordensDeServico.ItensServicosAddView', {
    extend: 'App.webDesktop.Module',
    id: 'module-itensServicosOS-add',
    init: function () {
    },

    createWindow: function (codigoTapete, codigoTipoDeCliente) {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-itensServicosOS-add');
        if (!win) {
            var panel = this.createPanel({codigoTapete: codigoTapete, codigoTipoDeCliente: codigoTipoDeCliente});
            win = desktop.createWindow({
                id: 'win-itensServicosOS-add',
                title: 'Adicionar servi&ccedil;o ao Item da Ordem de Serviço',
                width: 400,
                height: 280,
                modal: true,
                minimizable: false,
                iconCls: 'servicos-add-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [panel]
            });
        }
        win.show();
        return win;
    },

    createPanel: function (options) {
        this.options = options;

        var servicosEspecificosStore = Ext.create('App.store.servicos.ServicosEspecificosStore', { pageSize: 0 });
        servicosEspecificosStore.load({ params:{ codigoTapete: options.codigoTapete,codigoTipoDeCliente: options.codigoTipoDeCliente} });

        var form = Ext.create('Ext.form.Panel', {
            region: 'north',
            bodyPadding: 5,
            fieldDefaults: { labelAlign: 'left', labelWidth: 80, anchor: '100%', allowBlank: false, blankText: 'Este campo &eacute; obrigat&oacute;rio' },
            items: [
                { xtype: 'combobox', itemId: 'module-itensServicosOS-add_cmbServicos', store: servicosEspecificosStore, name: 'codigoServico', fieldLabel: 'Servi&ccedil;o', emptyText: 'Selecione o serviço', displayField: 'nome', valueField: 'codigo', typeAhead: true, queryMode: 'local', triggerAction: 'all', selectOnFocus: true, forceSelection: true, scope: this, listConfig: { getInnerTpl: function () { return '<div>{codigo} - {nome} - (valido para o tipo de cliente: {nomeTipoDeCliente})</div>'; } } },
                { xtype: 'textfield', fieldLabel: 'Cobrado Por', disabled: true },
                { xtype: 'textfield', fieldLabel: 'Tapete', disabled: true },
                { xtype: 'textfield', fieldLabel: 'Tipo de Cliente', disabled: true },
                { xtype: 'fieldcontainer', fieldLabel: 'Valores', disabled: true, layout: 'hbox', defaults: { labelAlign: 'top', margins: '0 4 0 0' },
                    items: [
                        { xtype: 'numberfield', flex : 1, fieldLabel: 'Valor at&eatilde; 10 m&sup2;', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                        { xtype: 'numberfield', flex : 1, fieldLabel: 'Valor acima de 10 m&sup2;', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false, margins: '0' }
                    ]
                },
                { xtype: 'numberfield', fieldLabel: 'Qtd M/M&sup2;', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                { xtype: 'numberfield', fieldLabel: 'Valor Final', hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false }
            ],
            buttonAlign: 'center',
            buttons: [{ text: 'Adicionar Servi&ccedil;o ao Item', itemId: 'btn-add-itemServicoOS', iconCls: 'servicos-add_thumb', padding: '10', scope: this}]
        });
        this.form = form;

        return form;
    }
});
