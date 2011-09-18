
Ext.define('App.model.ordensDeServico.ItemModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'codigoOrdemDeServico', type: 'int' },
        { name: 'codigoTapete', type: 'int' },
        { name: 'nomeTapete', type: 'string' },
        { name: 'comprimento', type: 'float' },
        { name: 'largura', type: 'float' },
        { name: 'area', type: 'float' },
        { name: 'valor', type: 'float' },
        { name: 'm_m2', type: 'int' },
        { name: 'observacoes', type: 'string' },
        { name: 'itensServicos', type: 'auto' }
    ]
});
