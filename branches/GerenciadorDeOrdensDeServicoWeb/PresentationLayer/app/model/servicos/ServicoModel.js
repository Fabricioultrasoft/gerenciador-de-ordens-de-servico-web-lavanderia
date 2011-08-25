
Ext.define('App.model.servicos.ServicoModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'descricao', type: 'string' },
        { name: 'valores', type: 'auto' }
    ]
});
