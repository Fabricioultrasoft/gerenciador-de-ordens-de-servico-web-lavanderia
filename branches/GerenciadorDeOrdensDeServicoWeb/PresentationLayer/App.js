Ext.Loader.setConfig({
    enabled: true,
    disableCaching: false
});

Ext.application({
    name: 'App',

    appFolder: 'app',
    controllers: [
        'enderecos.PaisesController',
        'enderecos.EstadosController',
        'enderecos.CidadesController',
        'enderecos.BairrosController',
        'enderecos.LogradourosController',
        'enderecos.TiposDeLogradourosController',
        'tapetes.TapetesController',
        'clientes.ClientesAddController',
        'clientes.ClientesEditController',
        'clientes.ClientesSearchController',
        'servicos.ServicosSearchController'
    ],
    launch: function () {
        Ext.create('App.webDesktop.MyAppWebDesktop', {});
    }
});



//+----------------------+
//| all exception events |
//+----------------------+
function genericExceptionHandler(thisProxy, response, operation, options) {

    if (response.status == 200) {
        genericErrorAlert("Exce&ccedil;&atilde;o Remota", operation.error);
    }
    else if (response.status == 500 && response.responseText) {
        var title = response.responseText.substring(response.responseText.indexOf("<title>"), response.responseText.lastIndexOf("</title>") + 8);
        var style = response.responseText.substring(response.responseText.indexOf("<style>"), response.responseText.lastIndexOf("</style>") + 8);
        var msg = response.responseText.replace("<html>", "").replace("</html>", "").replace("<head>", "").replace("</head>", "").replace("<body>", "").replace("</doby>", "").replace(title, "").replace(style, "");
        title = title.replace("<title>", "").replace("</title>", "");
        genericErrorAlert(title, "<div scrolling='yes' style='width:500px;height:400px;overflow:auto;'>" + msg + "</div>");
    }
}

function genericErrorAlert(title, message) {
    Ext.Msg.show({
        title: (title) ? title : 'Erro',
        msg: (message) ? message : 'Erro indefinido.',
        buttons: Ext.Msg.OK,
        icon: Ext.MessageBox.ERROR
    });
};


Ext.notification = function () {
    var msgCt;
    function createBox(t, s) {
        return '<div class="msg"><h3>' + t + '</h3><p>' + s + '</p></div>';
    }
    return {
        msg: function (title, format) {
            if (!msgCt) {
                msgCt = Ext.core.DomHelper.insertFirst(document.body, { id: 'msg-div' }, true);
            }
            var s = Ext.String.format.apply(String, Array.prototype.slice.call(arguments, 1));
            var m = Ext.core.DomHelper.append(msgCt, createBox(title, s), true);
            m.hide();
            m.slideIn('t').ghost("t", { delay: 5000, remove: true });
        },
        init: function () {}
    };
} (); 