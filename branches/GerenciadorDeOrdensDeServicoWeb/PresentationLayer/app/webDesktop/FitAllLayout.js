/**
* @class Ext.ux.desktop.FitAllLayout
* @extends Ext.layout.container.AbstractFit
* <p>This layout applies a "fit" layout to all items, overlaying them on top of each
* other.</p>
*/
Ext.define('App.webDesktop.FitAllLayout', {
    extend: 'Ext.layout.container.AbstractFit',
    alias: 'layout.fitall',

    // @private
    onLayout: function () {
        var me = this;
        me.callParent();

        var size = me.getLayoutTargetSize();

        me.owner.items.each(function (item) {
            me.setItemBox(item, size);
        });
    },

    getTargetBox: function () {
        return this.getLayoutTargetSize();
    },

    setItemBox: function (item, box) {
        var me = this;
        if (item && box.height > 0) {
            if (item.layoutManagedWidth == 2) {
                box.width = undefined;
            }
            if (item.layoutManagedHeight == 2) {
                box.height = undefined;
            }

            item.getEl().position('absolute', null, 0, 0);
            me.setItemSize(item, box.width, box.height);
        }
    }
});