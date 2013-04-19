//------------------
// Objeto Endereco
Ext.define('App.model.enderecos.EnderecoModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'complemento', type: 'string' },
        { name: 'pontoDeReferencia', type: 'string' },
        { name: 'numero', type: 'int' },
        { name: 'cep', type: 'string' },
        { name: 'codigoLogradouro', type: 'int' },
        { name: 'nomeLogradouro', type: 'string' },
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