
Ext.define('App.model.ordensDeServico.OrdemDeServicoModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'numero', type: 'int' },
        { name: 'valorOriginal', type: 'float' },
        { name: 'valorFinal', type: 'float' },
        { name: 'codigoStatus', type: 'int' },
        { name: 'nomeStatus', type: 'string' },
        { name: 'dataDeAbertura', type: 'string' },
        { name: 'previsaoDeConclusao', type: 'string' },
        { name: 'dataDeEncerramento', type: 'string' },
        { name: 'observacoes', type: 'string' },
        { name: 'usuario', type: 'auto' },
        { name: 'cliente', type: 'auto' },
        { name: 'itens', type: 'auto' }
    ]
});
