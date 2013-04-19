//-------------
// Objeto País
Ext.define('App.model.enderecos.PaisModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' }
    ]
});