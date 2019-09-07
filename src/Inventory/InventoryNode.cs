
using Godot;

namespace App.Inventory
{
    /**
     * an InventoryItem's 2d node in the inventory
     */
    public class InventoryNode : Control {
        protected Vector2 cellPosition;
        /**
         * in pixels
         *
         * @todo do not hardcode this in InventoryNode
         */
        protected int itemGridCellSize = 64;
        /**
         * in inventory grid units
         */
        protected Vector2 gridCellSize;
        protected TextureRect sprite;
        public InventoryItem item = null;
        protected bool shouldHaveHoverBorder = false;

        public InventoryNode(Texture texture, Vector2? gridCellSize = null) : base() {
            if (gridCellSize == null) {
                gridCellSize = new Vector2(1, 1);
            }
            this.gridCellSize = (Vector2)gridCellSize;

            var sprite = new TextureRect();
            sprite.Texture = texture;
            sprite.SetAnchorsPreset(LayoutPreset.Wide);
            this.sprite = sprite;
        }
        public override void _Ready()
        {
            this.SetName("InventoryBagItem");
            this.SetMouseFilter(Control.MouseFilterEnum.Pass);
            this.AnchorLeft = 1;
            this.AnchorTop = 1;
            this.SetCustomMinimumSize(new Vector2(
                this.itemGridCellSize,
                this.itemGridCellSize
            ) * this.gridCellSize);

            this.AddChild(this.sprite);

            this.Connect("mouse_entered", this, nameof(this.onMouseEntered));
            this.Connect("mouse_exited", this, nameof(this.onMouseExited));

            this.SetTooltip("foo bar");
        }

        public override void _GuiInput(InputEvent @event){
            if (@event is InputEventMouseButton eventMouseButton) {
                if (eventMouseButton.ButtonIndex == 1) {
                    if (eventMouseButton.Pressed) {
                        if (InventoryInteraction.hasPickedUpItem()) {
                            if (!InventoryInteraction.justPickedUpItem) {
                                /**
                                * place the item down
                                */
                                GD.Print("place the item down");
                                // InventoryInteraction.setPickedUpItem(null);
                            } else {
                                /**
                                * the player picked this item up but did not release it just yet
                                * drag the item on the screen but do not drop it yet
                                *
                                * godot does not actually calls this, because it only calls _GuiInput when
                                * there was a change and not when the user keeps a mouse pressed
                                */
                            }
                        } else {
                            /**
                             * if we are pointing at an inventory item with the mouse, then pick up that item
                             */
                            GD.Print("picking up item...");
                            InventoryInteraction.setPickedUpItem(this.item);
                            this.SetGlobalPosition(eventMouseButton.GetGlobalPosition() - (this.GetCustomMinimumSize() / 2));
                        }
                    } else {
                        /**
                         * the player just released LMB
                         */
                        if (InventoryInteraction.hasPickedUpItem()) {
                            /**
                            * the player let go of the mouse - we should remember
                            * that when the next time he presses the mouse we want to drop the item
                            * that he's holding
                            */
                            InventoryInteraction.justPickedUpItem = false;
                        }
                    }
                }
            }
        }

        public Vector2 getCellPosition() {
            return this.cellPosition;
        }
        public void setCellPosition(Vector2 position){
            this.cellPosition = position;
        }

        public override void _Draw(){

            if (this.shouldHaveHoverBorder) {
                var style = new StyleBoxFlat();
                style.DrawCenter = false;
                style.BorderBlend = true;
                style.BorderColor = new Color(1, 1, 0, 1);
                style.SetBorderWidthAll(3);
                this.DrawStyleBox(style, new Rect2(new Vector2(0, 0), this.GetCustomMinimumSize()));
            }
        }

        public void onPickup() {
            this.Modulate = new Color(1, 1, 1, 0.5f);
            this.shouldHaveHoverBorder = false;
            this.Update();
        }

        public void onDrop() {
            this.Modulate = new Color(1, 1, 1, 1);
            this.shouldHaveHoverBorder = true;
            this.Update();
        }

        public void onMouseEntered() {
            this.shouldHaveHoverBorder = true;
            this.Update();
        }

        public void onMouseExited() {
            this.shouldHaveHoverBorder = false;
            this.Update();
        }
    }
}
