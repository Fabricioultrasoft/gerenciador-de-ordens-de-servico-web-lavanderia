
Ext.define('App.model.clientes.MeioDeContatoModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'codigoTipoDeContato', type: 'int' },
        { name: 'nomeTipoDeContato' },
        { name: 'contato' },
        { name: 'descricao' }
    ]
});