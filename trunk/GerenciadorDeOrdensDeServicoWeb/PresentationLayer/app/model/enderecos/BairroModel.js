//--------------
// Objeto Bairro
Ext.define('App.model.enderecos.BairroModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'codigoCidade', type: 'int' },
        { name: 'nomeCidade', type: 'string' },
        { name: 'codigoEstado', type: 'int' },
        { name: 'nomeEstado', type: 'string' },
        { name: 'codigoPais', type: 'int' },
        { name: 'nomePais', type: 'string' }
    ]
});