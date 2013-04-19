
Ext.define('App.model.tapetes.TapeteModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'descricao', type: 'string' },
        { name: 'ativo', type: 'boolean' }
    ]
});