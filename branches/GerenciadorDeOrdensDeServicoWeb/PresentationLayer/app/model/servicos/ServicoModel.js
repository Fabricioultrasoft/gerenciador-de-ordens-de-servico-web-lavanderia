
Ext.define('App.model.servicos.ServicoModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'descricao', type: 'string' },
        { name: 'codigoCobradoPor', type: 'int' },
        { name: 'nomeCobradoPor', type: 'string' },
        { name: 'flagValorUnico', type: 'boolean' },
        { name: 'valorBase', type: 'float' },
        { name: 'valores', type: 'auto' },
    ]
});
