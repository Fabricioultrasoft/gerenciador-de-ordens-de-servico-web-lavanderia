﻿ // Objeto que cria um ambiente desktop no navegador Ext.define('App.webDesktop.AppWebDesktop', {     mixins: {         observable: 'Ext.util.Observable'     },     requires: ['Ext.container.Viewport', 'App.webDesktop.Desktop'],     isReady: false,     modules: null,     useQuickTips: true,     constructor: function (config) {         var me = this;         me.addEvents('ready', 'beforeunload');         me.mixins.observable.constructor.call(this, config);         if (Ext.isReady) {             Ext.Function.defer(me.init, 10, me);         } else {             Ext.onReady(me.init, me);         }     },     init: function () {          var me = this;         var desktopCfg;          if (me.useQuickTips) {             Ext.QuickTips.init();         }         me.modules = me.getModules();         if (me.modules) {             me.initModules(me.modules);         }         desktopCfg = me.getDesktopConfig();                  me.desktop = new App.webDesktop.Desktop(desktopCfg);                  me.viewport = new Ext.container.Viewport({             layout: 'fit',             items: [me.desktop]         });         Ext.EventManager.on(window, 'beforeunload', me.onUnload, me);         me.isReady = true;                  me.fireEvent('ready', me);     },     /**     * This method returns the configuration object for the Desktop object. A derived     * class can override this method, call the base version to build the config and     * then modify the returned object before returning it.     */     getDesktopConfig: function () {         var me = this, cfg = {             app: me,             taskbarConfig: me.getTaskbarConfig()         };         Ext.apply(cfg, me.desktopConfig);         return cfg;     },     getModules: Ext.emptyFn,     /**     * This method returns the configuration object for the Start Button. A derived     * class can override this method, call the base version to build the config and     * then modify the returned object before returning it.     */     getStartConfig: function () {         var me = this, cfg = {             app: me,             menu: []         };         Ext.apply(cfg, me.startConfig);         Ext.each(me.modules, function (module) {             if (module.launcher) {                 cfg.menu.push(module.launcher);             }         });         return cfg;     },     /**     * This method returns the configuration object for the TaskBar. A derived class     * can override this method, call the base version to build the config and then     * modify the returned object before returning it.     */     getTaskbarConfig: function () {         var me = this, cfg = {             app: me,             startConfig: me.getStartConfig()         };         Ext.apply(cfg, me.taskbarConfig);         return cfg;     },     initModules: function (modules) {         var me = this;         Ext.each(modules, function (module) {             module.app = me;         });     },     getModule: function (name) {         var ms = this.modules;         for (var i = 0, len = ms.length; i < len; i++) {             var m = ms[i];             if (m.id == name || m.appType == name) {                 return m;             }         }         return null;     },     onReady: function (fn, scope) {         if (this.isReady) {             fn.call(scope, this);         } else {             this.on({                 ready: fn,                 scope: scope,                 single: true             });         }     },     getDesktop: function () {         return this.desktop;     },     onUnload: function (e) {         if (this.fireEvent('beforeunload', this) === false) {             e.stopEvent();         }     } }); 