/**
* @class App.webDesktop.ShortcutModel
* @extends Ext.data.Model
* This model defines the minimal set of fields for desktop shortcuts.
*/
Ext.define('App.webDesktop.ShortcutModel', {
    extend: 'Ext.data.Model',
    fields: [
       { name: 'name' },
       { name: 'iconCls' },
       { name: 'module' }
    ]
});
