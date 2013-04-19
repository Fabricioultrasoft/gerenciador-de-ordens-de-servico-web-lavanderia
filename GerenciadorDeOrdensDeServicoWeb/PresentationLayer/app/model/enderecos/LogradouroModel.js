//------------------
// Objeto Logradouro
Ext.define('App.model.enderecos.LogradouroModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'cep', type: 'string' },
        { name: 'codigoTipoDeLogradouro', type: 'int' },
        { name: 'nomeTipoDeLogradouro', type: 'string' },
        { name: 'codigoBairro', type: 'int' },
        { name: 'nomeBairro', type: 'string' },
        { name: 'codigoCidade', type: 'int' },
        { name: 'nomeCidade', type: 'string' },
        { name: 'codigoEstado', type: 'int' },
        { name: 'nomeEstado', type: 'string' },
        { name: 'codigoPais', type: 'int' },
        { name: 'nomePais', type: 'string' }
    ]
});