
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
            sprite.SetMouseFilter(MouseFilterEnum.Ignore);
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

            // this.SetTooltip("foo bar");
        }

        public Control getMouseDragNode() {
            return this.sprite;
        }

        public override void _GuiInput(InputEvent @event){
            if (@event is InputEventMouseButton eventMouseButton) {
                if (eventMouseButton.ButtonIndex == 1) {
                    if (eventMouseButton.Pressed) {
                        /**
                         * if we are pointing at an inventory item with the mouse, then pick up that item
                         */
                        GD.Print("picking up item from inventory...");
                        ((App.Inventory.InventoryDragOverlay)GetNode("/root/InventoryDragOverlay")).setPickedUpItem(this.item, this.GetParent<InventoryBag>());
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

        public void onPickup(InventoryBag bag = null) {
            this.Modulate = new Color(1, 1, 1, 0.5f);
            this.shouldHaveHoverBorder = false;
            this.Update();
        }

        public void onDrop(InventoryBag bag = null) {
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
