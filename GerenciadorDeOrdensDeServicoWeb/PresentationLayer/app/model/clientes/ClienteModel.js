
Ext.define('App.model.clientes.ClienteModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'conjuge', type: 'string' },
        { name: 'codigoTipoDeCliente', type: 'int' },
        { name: 'nomeTipoDeCliente', type: 'string' },
        { name: 'ativoTipoDeCliente', type: 'boolean' },
        { name: 'sexo', type: 'int' },
        { name: 'strSexo', type: 'string' },
        { name: 'dataDeNascimento', type: 'string' },
        { name: 'rg', type: 'string' },
        { name: 'cpf', type: 'string' },
        { name: 'observacoes', type: 'string' },
        { name: 'ativo', type: 'boolean' },
        { name: 'dataDeCadastro', type: 'string' },
        { name: 'dataDeAtualizacao', type: 'string' },
        { name: 'meiosDeContato', type: 'auto' },
        { name: 'enderecos', type: 'auto' }
    ]
});
