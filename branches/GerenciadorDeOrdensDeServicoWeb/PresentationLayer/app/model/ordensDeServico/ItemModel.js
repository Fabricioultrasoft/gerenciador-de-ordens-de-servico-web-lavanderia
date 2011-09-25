
Ext.define('App.model.ordensDeServico.ItemModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'codigoOrdemDeServico', type: 'int' },
        { name: 'codigoTapete', type: 'int' },
        { name: 'nomeTapete', type: 'string' },
        { name: 'tapete', type: 'auto' },
        { name: 'comprimento', type: 'float' },
        { name: 'largura', type: 'float' },
        { name: 'area', type: 'float' },
        { name: 'valor', type: 'float' },
        { name: 'observacoes', type: 'string' },
        { name: 'servicosDoItem', type: 'auto' }
    ]
});
