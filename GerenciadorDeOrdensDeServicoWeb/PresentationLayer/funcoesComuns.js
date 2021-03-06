﻿//+----------------------+
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
    
    if(isArray(message)) {
        var i, msgAux = "";
        for(i = 0; i < message.length; i++) {
            msgAux += message[i];
        }
        message = msgAux;
    }
    
    Ext.Msg.show({
        title: (title) ? title : 'Erro',
        msg: (message) ? message : 'Erro indefinido.',
        buttons: Ext.Msg.OK,
        icon: Ext.MessageBox.ERROR
    });
};

function isArray(o){
	return(typeof(o.length) == "undefined" || typeof(o) == "string" || typeof(o) == "number") ? false : true;
}