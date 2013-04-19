//-----------------
// BARRA DE TAREFAS
Ext.define('App.webDesktop.TaskBar', {
    extend: 'Ext.toolbar.Toolbar',
    requires: [
        'Ext.button.Button',
        'Ext.resizer.Splitter',
        'Ext.menu.Menu',
    // classes do projeto
        'App.webDesktop.StartMenu',
        'App.webDesktop.TrayClock'
    ],
    alias: 'widget.taskbar',
    cls: 'ux-taskbar',

    // TEXTO PARA O BOTAO 'INICIAR'
    startBtnText: 'Funcionalidades',

    initComponent: function () {
        var me = this;

        // MENU INICIAR
        
        me.startMenu = Ext.create('App.webDesktop.StartMenu', me.startConfig);
        
        // MENU INICIO RAPIDO
        me.quickStart = Ext.create('Ext.toolbar.Toolbar', me.getQuickStart());
        
        // BARRA DAS JANELAS
        me.windowBar = Ext.create('Ext.toolbar.Toolbar', me.getWindowBarConfig());
        
        // BARRA DO RELOGIO
        me.tray = Ext.create('Ext.toolbar.Toolbar', me.getTrayConfig());

        me.items = [
            {
                xtype: 'button',
                cls: 'ux-start-button',
                iconCls: 'ux-start-button-icon',
                menu: me.startMenu,
                menuAlign: 'bl-tl',
                text: me.startBtnText,
                height: 28
            },
            me.quickStart,
            {
                xtype: 'splitter', html: '&#160;',
                height: 14, width: 2, // TODO - there should be a CSS way here
                cls: 'x-toolbar-separator x-toolbar-separator-horizontal'
            },
            //'-',
            me.windowBar,
            '-',
            me.tray
        ];

        me.callParent();
    },

    afterLayout: function () {
        var me = this;
        me.callParent();
        me.windowBar.el.on('contextmenu', me.onButtonContextMenu, me);
    },

    /**
     * Este metodo retorna  o objeto de configuracao para o menu de inicio rapido.
     * uma subclasse desta pode sobreescrever este metodo e quando chamar este metodo sobreescrito
     * chame esta versao basica primeiro para contruir uma configuracao basica e entao
     * modifique o valor retornado antes de finalizar o metodo sobreescrito.
     */
    getQuickStart: function () {
        var me = this, ret = {
            minWidth: 20,
            width: 60,
            items: [],
            enableOverflow: true
        };

        Ext.each(this.quickStart, function (item) {
            ret.items.push({
                tooltip: { text: item.name, align: 'bl-tl' },
                //tooltip: item.name,
                overflowText: item.name,
                iconCls: item.iconCls,
                module: item.module,
                handler: me.onQuickStartClick,
                scope: me
            });
        });

        return ret;
    },

    /**
     * Este metodo retorna um objeto de configuracao para a Barra do relogio. Uma classe derivada
     * pode sobreescrever este metodo, chamar a versao base para construir o objeto de configuracao e
     * entao modificar o obj de configuracao antes de retorna-lo.
     */
    getTrayConfig: function () {
        var ret = {
            width: 80,
            items: this.trayItems
        };
        delete this.trayItems;
        return ret;
    },

    getWindowBarConfig: function () {
        return {
            flex: 1,
            cls: 'ux-desktop-windowbar',
            items: [ '&#160;' ],
            layout: { overflowHandler: 'Scroller' }
        };
    },

    getWindowBtnFromEl: function (el) {
        var c = this.windowBar.getChildByElement(el);
        return c || null;
    },

    onQuickStartClick: function (btn) {
        var module = this.app.getModule(btn.module);
        if (module) {
            module.createWindow();
        }
    },

    onButtonContextMenu: function (e) {
        var me = this;
        var t = e.getTarget();
        var btn = me.getWindowBtnFromEl(t);
        
        if (btn) {
            e.stopEvent();
            me.windowMenu.theWin = btn.win;
            me.windowMenu.showBy(t);
        }
    },

    onWindowBtnClick: function (btn) {
        var win = btn.win;

        if (win.minimized || win.hidden) {
            win.show();
        } else if (win.active) {
            win.minimize();
        } else {
            win.toFront();
        }
    },

    addTaskButton: function(win) {
        var config = {
            iconCls: win.iconCls,
            enableToggle: true,
            toggleGroup: 'all',
            width: 140,
            text: Ext.util.Format.ellipsis(win.title, 20),
            listeners: {
                click: this.onWindowBtnClick,
                scope: this
            },
            win: win
        };

        var cmp = this.windowBar.add(config);
        cmp.toggle(true);
        return cmp;
    },

    removeTaskButton: function (btn) {
        var found;
        var me = this;

        me.windowBar.items.each(function (item) {
            if (item === btn) {
                found = item;
            }
            return !found;
        });
        if (found) {
            me.windowBar.remove(found);
        }
        return found;
    },

    setActiveButton: function(btn) {
        if (btn) {
            btn.toggle(true);
        } else {
            this.windowBar.items.each(function (item) {
                if (item.isButton) {
                    item.toggle(false);
                }
            });
        }
    }
});