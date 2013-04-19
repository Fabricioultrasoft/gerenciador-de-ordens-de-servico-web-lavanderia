
Ext.define('App.webDesktop.Settings', {
    extend: 'Ext.window.Window',

    uses: [
        'Ext.tree.Panel',
        'Ext.tree.View',
        'Ext.form.field.Checkbox',
        'Ext.layout.container.Anchor',
        'Ext.layout.container.Border',

        'App.webDesktop.Wallpaper',
        'App.webDesktop.WallpaperModel'
    ],

    layout: 'anchor',
    title: 'Alterar Características',
    iconCls: 'settings',
    modal: true,
    width: 700,
    height: 540,
    border: false,

    initComponent: function () {
        var me = this;

        me.selected = me.desktop.getWallpaper();
        me.stretch = me.desktop.wallpaper.stretch;

        me.preview = Ext.create('widget.wallpaper');
        me.preview.setWallpaper(me.selected);
        me.tree = me.createTree();

        me.buttons = [
            { text: 'OK', handler: me.onOK, scope: me },
            { text: 'Cancelar', handler: me.close, scope: me }
        ];

        me.items = [
            {
                anchor: '0 -30',
                border: false,
                layout: 'border',
                items: [
                    me.tree,
                    {
                        xtype: 'panel',
                        title: 'Preview',
                        iconCls: 'lupa',
                        region: 'center',
                        layout: 'fit',
                        items: [me.preview]
                    }
                ]
            },
            {
                xtype: 'checkbox',
                boxLabel: 'Ajustar à tela',
                checked: me.stretch,
                listeners: {
                    change: function (comp) {
                        me.stretch = comp.checked;
                    }
                }
            }
        ];

        me.callParent();
    },

    createTree: function () {
        var me = this;

        function child(img) {
            return { img: img, text: me.getTextOfWallpaper(img), iconCls: 'picture', leaf: true };
        }

        var tree = new Ext.tree.Panel({
            title: 'Planos de Fundo',
            iconCls: 'picture',
            rootVisible: false,
            lines: false,
            autoScroll: true,
            width: 150,
            region: 'west',
            split: true,
            minWidth: 100,
            listeners: {
                afterrender: { fn: this.setInitialSelection, delay: 100 },
                select: this.onSelect,
                scope: this
            },
            store: new Ext.data.TreeStore({
                model: 'App.webDesktop.WallpaperModel',
                root: {
                    text: 'Wallpaper',
                    expanded: true,
                    children: [
                        { text: "None", iconCls: 'picture', leaf: true },
                        child('Another-Poppy.png'),
                        child('Aquarius.png'),
                        child('desk.jpg'),
                        child('Foresight.png'),
                        child('Happy.png'),
                        child('Is-Doodle-Orange.png'),
                        child('Sunergos-Grey.png')
                    ]
                }
            })
        });

        return tree;
    },

    getTextOfWallpaper: function (path) {
        var text = path, slash = path.lastIndexOf('/');
        if (slash >= 0) {
            text = text.substring(slash + 1);
        }
        var dot = text.lastIndexOf('.');
        text = Ext.String.capitalize(text.substring(0, dot));
        text = text.replace(/[-]/g, ' ');
        return text;
    },

    onOK: function () {
        var me = this;
        if (me.selected) {
            me.desktop.setWallpaper(me.selected, me.stretch);
        }
        me.destroy();
    },

    onSelect: function (tree, record) {
        var me = this;

        if (record.data.img) {
            me.selected = 'resources/images/wallpapers/' + record.data.img;
        } else {
            me.selected = Ext.BLANK_IMAGE_URL;
        }

        me.preview.setWallpaper(me.selected);
    },

    setInitialSelection: function () {
        var s = this.desktop.getWallpaper();
        if (s) {
            var path = 'resources/images/wallpapers/' + this.getTextOfWallpaper(s);
            this.tree.selectPath(path, 'text');
        }
    }
});