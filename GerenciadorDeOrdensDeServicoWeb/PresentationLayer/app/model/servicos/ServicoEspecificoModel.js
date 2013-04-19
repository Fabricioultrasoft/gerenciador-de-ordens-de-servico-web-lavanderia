
Ext.define('App.model.servicos.ServicoEspecificoModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'nome', type: 'string' },
        { name: 'descricao', type: 'string' },
        { name: 'codigoCobradoPor', type: 'int' },
        { name: 'nomeCobradoPor', type: 'string' },
        { name: 'codigoTapete', type: 'int' },
        { name: 'nomeTapete', type: 'string' },
        { name: 'codigoTipoDeCliente', type: 'int' },
        { name: 'nomeTipoDeCliente', type: 'string' },
        { name: 'valor', type: 'float' },
        { name: 'valorAcima10m2', type: 'float' }
    ]
});
