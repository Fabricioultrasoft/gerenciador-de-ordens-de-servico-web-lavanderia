
Ext.define('App.model.servicos.ValorServicoModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'codigoServico', type: 'int' },
        { name: 'codigoTapete', type: 'int' },
        { name: 'codigoTipoDeCliente', type: 'int' },
        { name: 'nomeTipoDeCliente', type: 'string' },
        { name: 'valor', type: 'float' },
        { name: 'valorAcima10m2', type: 'float' }
    ]
});
