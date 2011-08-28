
Ext.define('App.controller.servicos.ServicosSearchController', {
    extend: 'Ext.app.Controller',

    models: ['servicos.ServicoModel','servicos.ValorServicoModel'],

    views: ['servicos.ServicosSearchView'],

    stores: ['servicos.ServicosStore'],

    init: function () {
        this.control({
            '#grid-servicos': {
                itemdblclick: this.editServico
            },
            '#btnAddServico': {
                click: this.onAddServicoClick
            },
            '#btnEditServico': {
                click: this.onEditServicoClick
            },
            '#btnDelServico': {
                click: this.onDelServicoClick
            },
            '#btnShowDescricaoServico': {
                click: this.onShowDescricaoServicoClick
            }
        });
    },

    addServico: function (grid, record) {
        grid.panel.module.app.getModule("win-servicos-add").createWindow({record:record});
    },

    editServico: function (grid, record) {
        grid.panel.module.app.getModule("win-servicos-edit").createWindow({record:record});
    },

    onEditServicoClick: function(btn, eventObject, options) {
        var grid = btn.scope.gridServicos;
        var record = grid.getSelectionModel().getSelection()[0];

        grid.fireEvent("itemdblclick",grid.view,record ); 
    },

    onDelServicoClick: function (btn, eventObject, options) {
        var sm = btn.scope.gridServicos.getSelectionModel();

        Ext.Msg.show({
            title: 'Excluir servi&ccedil;o',
            msg: '<b>Tem certeza de que deseja excluir este registro?</b><br />Cod: ' + sm.getSelection()[0].data.codigo + '<br />Servi&ccedil;o: ' + sm.getSelection()[0].data.nome,
            buttons: Ext.Msg.YESNO,
            fn: function (buttonId) {
                if (buttonId == 'yes') {
                    btn.scope.gridServicos.getStore().remove(sm.getSelection());
                    btn.scope.gridServicos.getStore().sync();
                    btn.scope.gridServicos.getDockedItems( 'pagingtoolbar' )[0].doRefresh();
                    if (btn.scope.gridServicos.getStore().getCount() > 0) {
                        sm.select(0);
                    }
                }
            },
            animateTarget: btn.id,
            icon: Ext.Msg.QUESTION
        });
    },

    onShowDescricaoServicoClick: function (btn, event, options) {
        btn.scope.gridServicos.getComponent('viewServicos').getPlugin('previewServicos').toggleExpanded(btn.pressed);
    }
});