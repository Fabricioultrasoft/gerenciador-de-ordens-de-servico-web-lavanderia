//--------------
// Objeto Estado
Ext.define('App.model.enderecos.EstadoModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'codigoPais', type: 'int' },
        { name: 'nomePais', type: 'string' }
    ]
});