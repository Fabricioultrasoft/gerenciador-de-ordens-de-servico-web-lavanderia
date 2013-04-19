
Ext.define('App.model.clientes.TipoDeClienteModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'ativo', type: 'boolean' }
    ]
});
