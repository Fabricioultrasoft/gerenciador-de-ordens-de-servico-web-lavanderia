
Ext.define('App.view.ajuda.AjudaView', {
    extend: 'App.webDesktop.Module',
    id: 'module-ajuda',
    init: function () {
        this.launcher = {
            text: 'Ajuda',
            iconCls: 'help',
            handler: function() {
                var newWindow = window.open('/PresentationLayer/app/view/ajuda/index.html');
	            if (window.focus) {newWindow.focus()} 
            },
            scope: this
        };
    }
});