
Ext.define('App.model.ordensDeServico.ItemServicoModel',{
    extend: 'Ext.data.Model',
    idProperty : 'codigo',
    fields: [
        { name: 'codigo', type: 'int' },
        { name: 'codigoItem', type: 'int' },
        { name: 'codigoServico', type: 'int' },
        { name: 'nomeServico', type: 'string' },
        { name: 'quantidade_m_m2', type: 'float' },
        { name: 'valor', type: 'float' }
    ]
});
