//------------------
// Objeto Logradouro
Ext.define('App.model.enderecos.TipoDeLogradouroModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' }
    ]
});