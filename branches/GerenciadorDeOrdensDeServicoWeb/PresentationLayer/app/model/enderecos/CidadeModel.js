//--------------
// Objeto Cidade
Ext.define('App.model.enderecos.CidadeModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'codigoEstado', type: 'int' },
        { name: 'nomeEstado', type: 'string' },
        { name: 'codigoPais', type: 'int' },
        { name: 'nomePais', type: 'string' }
    ]
});