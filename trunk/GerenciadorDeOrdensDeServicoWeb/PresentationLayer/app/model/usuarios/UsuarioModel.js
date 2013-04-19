
Ext.define('App.model.usuarios.UsuarioModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'senha', type: 'string' }
    ]
});
