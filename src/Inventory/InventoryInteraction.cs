using Godot;
using App;

namespace App.Inventory
{
    public class InventoryInteraction {


        protected static InventoryItem pickedUpItem;
        /**
         * this prevents the player from picking up and placing down
         * the item in the next frame - he will need to release the mouse
         * button and press it again to place the item
         */
        public static bool justPickedUpItem = false;

        public static void setPickedUpItem(InventoryItem item = null, bool ignoreJustPickedUpItem = false) {
            if (pickedUpItem != null){
                /**
                 * if we already had an item picked up but we are either
                 * dropping it, or switching it to another item, then
                 * notify the item that this is happening
                 */
                pickedUpItem.getInventoryNode().onDrop();
            }
            if (item != null) {
                /**
                 * notify the item that it was picked up
                 */
                item.getInventoryNode().onPickup();
            }
            pickedUpItem = item;
            if (pickedUpItem != null && !ignoreJustPickedUpItem) {
                justPickedUpItem = true;
            }
        }

        public static InventoryItem getPickedUpItem() {
            return pickedUpItem;
        }

        public static bool hasPickedUpItem() {
            return pickedUpItem != null;
        }
    }
}
