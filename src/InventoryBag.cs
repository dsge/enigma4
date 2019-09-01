using Godot;
using System.Collections.Generic;

namespace App
{
    public class InventoryBag: Control {

        protected int itemGridCellSize = 64; //pixels
        protected List<InventoryBagItem> items = new List<InventoryBagItem>();
        public override void _Ready()
        {
            this.SetMouseFilter(Control.MouseFilterEnum.Pass);
            this.initItemGrid(new Vector2(4, 6));

            this.addItem(new InventoryBagItem());
            this.addItem(new InventoryBagItem());
        }

        public override void _Input(InputEvent @event){
            if (@event is InputEventMouseMotion eventMouseMotion) {
                if (InventoryInteraction.hasPickedUpItem()) {
                    this.AcceptEvent();
                    InventoryBagItem item = InventoryInteraction.getPickedUpItem();
                    item.SetGlobalPosition(eventMouseMotion.GetGlobalPosition());
                }
            }
            if (@event is InputEventMouseButton eventMouseButton) {
                if (eventMouseButton.ButtonIndex == 1) {
                    if (eventMouseButton.Pressed) {
                        if (InventoryInteraction.hasPickedUpItem()) {
                            if (!InventoryInteraction.justPickedUpItem) {
                                /**
                                * place the item down
                                */
                                this.AcceptEvent();
                                GD.Print("place the item down");
                                this.addItem(InventoryInteraction.getPickedUpItem(), this.localToCellPosition(this.GetLocalMousePosition()));
                                InventoryInteraction.setPickedUpItem(null);
                            }
                        }
                    }
                }
            }
        }

        protected void initItemGrid(Vector2 size) {
            var texture = Util.imageToTexture("res://static/inventory-grid-item.png");

            for(var i = 0; i < size.x; i++) {
                for(var j = 0; j < size.y; j++) {
                    var tmp = new Control();
                    tmp.SetName("ItemGridElement");
                    // tmp.Position = new Vector2(this.itemGridCellSize * i, this.itemGridCellSize * j);
                    tmp.MarginLeft = this.itemGridCellSize * i;
                    tmp.MarginTop = this.itemGridCellSize * j;

                    var sprite = new TextureRect();
                    sprite.Texture = texture;
                    tmp.AddChild(sprite);
                    this.AddChild(tmp);
                }
            }
        }

        public void addItem(InventoryBagItem item, Vector2? forceCellPosition = null) {
            item.setCellPosition(calculateNextOpenCellPosition(forceCellPosition));

            var position = this.cellPositionToLocal(item.getCellPosition());
            item.MarginLeft = position.x;
            item.MarginTop = position.y;
            if (!this.hasItem(item)) {
                this.AddChild(item);
                this.items.Add(item);
            }
        }

        protected Vector2 calculateNextOpenCellPosition(Vector2? forceCellPosition = null) {
            if (forceCellPosition != null) {
                return (Vector2)forceCellPosition;
            }
            return new Vector2(this.items.Count, 0);
        }

        public bool hasItem(InventoryBagItem item) {
            return this.items.Contains(item);
        }

        public void removeItem(InventoryBagItem item) {
            this.items.Remove(item);
        }

        /**
         * what are the local coordinates of a cell?
         */
        protected Vector2 cellPositionToLocal(Vector2 cellPosition) {
            return new Vector2(cellPosition.x * this.itemGridCellSize, cellPosition.y * this.itemGridCellSize);
        }
        /**
         * which cell do these local coordinates refer to?
         */
        protected Vector2 localToCellPosition(Vector2 localCoordinates) {
            return new Vector2(
                (int)System.Math.Floor(localCoordinates.x / this.itemGridCellSize),
                (int)System.Math.Floor(localCoordinates.y / this.itemGridCellSize)
            );
        }
    }

    public interface InventoryBagItemInterface {
        Vector2 getCellPosition();
        void setCellPosition(Vector2 position);
    }

    public class InventoryBagItem : Control {
        protected Vector2 cellPosition;
        /**
         * in pixels
         *
         * @todo do not hardcode this in InventoryBagItem
         */
        protected int itemGridCellSize = 64;
        protected bool shouldHaveHoverBorder = false;
        public override void _Ready()
        {
            this.SetName("InventoryBagItem");
            this.SetMouseFilter(Control.MouseFilterEnum.Pass);
            this.AnchorLeft = 1;
            this.AnchorTop = 1;
            this.SetCustomMinimumSize(new Vector2(this.itemGridCellSize, this.itemGridCellSize));

            var sprite = new TextureRect();
            sprite.Texture = Util.imageToTexture("res://static/dummy-sword-inventory.png");
            sprite.SetAnchorsPreset(LayoutPreset.Wide);
            this.AddChild(sprite);

            this.Connect("mouse_entered", this, nameof(this.onMouseEntered));
            this.Connect("mouse_exited", this, nameof(this.onMouseExited));
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
                            InventoryInteraction.setPickedUpItem(this);
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
            GD.Print("inventorybagitem mouse_entered");
            this.shouldHaveHoverBorder = true;
            this.Update();
        }

        public void onMouseExited() {
            GD.Print("inventorybagitem mouse_exited");
            this.shouldHaveHoverBorder = false;
            this.Update();
        }
    }
}
