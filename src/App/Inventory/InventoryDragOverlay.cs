using Godot;
using App;

namespace App.Inventory
{
    public class InventoryDragOverlay: CanvasLayer {


        protected InventoryItem pickedUpItem;
        protected Control mouseDragNode;
        /**
         * this prevents the player from picking up and placing down
         * the item in the next frame - he will need to release the mouse
         * button and press it again to place the item
         */
        public bool justPickedUpItem = false;

        public void setPickedUpItem(InventoryItem item = null, InventoryBag bag = null, bool ignoreJustPickedUpItem = false) {
            if (pickedUpItem != null){
                /**
                 * if we already had an item picked up but we are either
                 * dropping it, or switching it to another item, then
                 * notify the item that this is happening
                 */
                pickedUpItem.getInventoryNode().onDrop();
                var parent = pickedUpItem.getInventoryNode().GetParentOrNull<Node>();
                if (parent != null) {
                    parent.RemoveChild(pickedUpItem.getInventoryNode());
                }
                if (bag != null) {
                    bag.AddChild(pickedUpItem.getInventoryNode());
                }
                if (this.mouseDragNode != null) {
                    this.RemoveChild(mouseDragNode);
                }
            }
            if (item != null) {
                /**
                 * notify the item that it was picked up
                 */
                item.getInventoryNode().onPickup(bag);
                var parent = item.getInventoryNode().GetParentOrNull<Node>();
                if (parent != null) {
                    parent.RemoveChild(item.getInventoryNode());
                }
                this.mouseDragNode = (Control)(item.getInventoryNode().getMouseDragNode().Duplicate());
                this.mouseDragNode.SetMouseFilter(Control.MouseFilterEnum.Ignore);
                this.mouseDragNode.SetGlobalPosition(this.GetViewport().GetMousePosition() - (item.getInventoryNode().GetCustomMinimumSize() / 2));
                this.AddChild(this.mouseDragNode);
            }
            pickedUpItem = item;
            if (pickedUpItem != null && !ignoreJustPickedUpItem) {
                justPickedUpItem = true;
            }
        }

        public InventoryItem getPickedUpItem() {
            return pickedUpItem;
        }

        public bool hasPickedUpItem() {
            return pickedUpItem != null;
        }

        public override void _Input(InputEvent @event){
            if (@event is InputEventMouseMotion eventMouseMotion) {
                if (this.hasPickedUpItem()) {
                    //this.AcceptEvent(); //handled in _GuiInput instead
                    InventoryItem item = this.getPickedUpItem();
                    this.mouseDragNode.SetGlobalPosition(eventMouseMotion.GetGlobalPosition() - (item.getInventoryNode().GetCustomMinimumSize() / 2));
                }
            }
        }
    }
}
