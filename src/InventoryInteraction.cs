using Godot;
using App;

namespace App
{
    public class InventoryInteraction {


        protected static InventoryBagItem pickedUpItem;
        /**
         * this prevents the player from picking up and placing down
         * the item in the next frame - he will need to release the mouse
         * button and press it again to place the item
         */
        public static bool justPickedUpItem = false;

        public static void setPickedUpItem(InventoryBagItem item = null, bool ignoreJustPickedUpItem = false) {
            pickedUpItem = item;
            if (pickedUpItem != null && !ignoreJustPickedUpItem) {
                justPickedUpItem = true;
            }
        }

        public static InventoryBagItem getPickedUpItem() {
            return pickedUpItem;
        }

        public static bool hasPickedUpItem() {
            return pickedUpItem != null;
        }
    }
}
