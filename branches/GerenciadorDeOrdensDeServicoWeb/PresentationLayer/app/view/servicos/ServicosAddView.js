
Ext.define('App.view.servicos.ServicosAddView', {
    extend: 'App.webDesktop.Module',
    id: 'module-servicos-add',
    
    createWindow: function () {

        var desktop = this.app.getDesktop();
        var win = desktop.getWindow('win-servicos-add');
        if (!win) {
            var servicosPanel = this.createPanel();
            win = desktop.createWindow({
                id: 'win-servicos-add',
                title: 'Adicionar Serviço',
                width: 600,
                height: 540,
                iconCls: 'servicos-add-thumb',
                animCollapse: false,
                constrainHeader: true,
                layout: 'fit',
                border: false,
                items: [servicosPanel]
            });
        }
        win.show();
        return win;
    },

    createPanel: function () {

        var valoresStore = Ext.create('App.store.servicos.ValoresServicosStore',{ pageSize:0 });
        
        var formServico = Ext.create('Ext.form.Panel',{
            title: 'Dados do servi&ccedil;o',
            iconCls: 'servicos-thumb',
            bodyPadding: 5,
            height: 250,
            region: 'north',
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [
                { xtype: 'textfield', name: 'nome', fieldLabel: 'Nome', emptyText: 'Digite o nome do serviço', maxLength: 255, allowBlank: false, blankText: 'O NOME do servi&ccedil;o &eacute; obrigat&oacute;rio' },
                {
                    xtype: 'combobox', name: 'codigoCobradoPor', fieldLabel: 'Cobrado por', emptyText: 'Selecione como o serviço é cobrado',
                    store: Ext.create('Ext.data.Store', { fields: ['codigo', 'nome'], data: [ { 'codigo': 1, 'nome': "Metro (M)" }, { 'codigo': 2, 'nome': "Metro quadrado (m²)" } ] }),
                    queryMode: 'local', displayField: 'nome', valueField: 'codigo', selectOnFocus: true, forceSelection: true, allowBlank: false, blankText: 'Uma op&ccedil;&atilde;o deve ser selecionada'
                },
                { xtype: 'checkbox', itemId: 'addServicoFlgValorUnico', name: 'flgValorUnico', fieldLabel: '&Eacute; valor unico', inputValue: '1' },
                { xtype: 'numberfield', itemId: 'addServicoValorUnico', name: 'valorUnico', fieldLabel: 'Valor unico', disabled: true, hideTrigger: true, keyNavEnabled: false, mouseWheelEnabled: false },
                { xtype: 'textarea', name: 'descricao', fieldLabel: 'Descri&ccedil;&atilde;o', emptyText: 'Descrição do serviço', height: 100 }
            ]
        });
        this.formServico = formServico;
        
        var gridValoresServico =  Ext.create('Ext.tree.Panel', {
            title: 'Valores do servi&ccedil;o',
            iconCls: 'cifrao-thumb',
            region: 'center',
            rootVisible: false,
            useArrows: true,
            store: valoresStore,
            columns: [
                { xtype: 'treecolumn', dataIndex: 'nomeTapete', text: 'Tapete', flex: 2, sortable: true },
                { dataIndex: 'nomeTipoDeCliente', text: 'Tipo de cliente', flex: 1, align: 'center', sortable: true },
                { xtype: 'numbercolumn', dataIndex: 'valor', text: 'Valor', align: 'center', width: 70, sortable: true },
                { xtype: 'numbercolumn', dataIndex: 'valorAcima10m2', text: 'Acima 10m&sup2;', align: 'center', width: 70, sortable: true },
                {
                    xtype: 'actioncolumn',
                    text: 'Açao',
                    sortable: false,
                    align: 'center',
                    width: 50,
                    items: [{
                        iconCls: 'act-edit',  // Use a URL in the icon config
                        tooltip: 'Editar valores do servi&ccedil;o para este tapete',
                        handler: function(view, rowIndex, colIndex, item, event ) {
                            var rec = store.getAt(rowIndex);
                            alert("Sell " + rec.get('company'));
                        }
                    }, {
                        getClass: function(v, meta, rec) {          // Or return a class from a function
                            if (rec.get('codigoTipoDeCliente') > 0) {
                                this.items[1].tooltip = 'Remover condi&ccedil;&atilde;o de valor especial';
                                return 'act-dinheiro-del';
                            } else {
                                this.items[1].tooltip = 'Adicionar uma condi&ccedil;&atilde;o de valor especial para um tipo de cliente';
                                return 'act-dinheiro-add';
                            }
                        },
                        handler: function(grid, rowIndex, colIndex) {
                            var rec = store.getAt(rowIndex);
                            alert((rec.get('change') < 0 ? "Hold " : "Buy ") + rec.get('company'));
                        }
                    }]}
            ]
        });



        var mainPanel = Ext.create('Ext.panel.Panel', {
            xtype: 'panel',
            layout: 'border',
            border: false,
            items: [formServico,gridValoresServico],
            buttonAlign: 'center',
            buttons: [{ text: 'Adicionar Servi&ccedil;o', itemId: 'btn-add-servico', iconCls: 'servicos-add-thumb', padding: '10', scope: this}]
        });
        this.mainPanel = mainPanel;

        return mainPanel;
    }
});
