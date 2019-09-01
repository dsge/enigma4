using Godot;
using System.Collections.Generic;

namespace App
{
    public class InventoryBag: Control {

        protected int itemGridCellSize = 64; //pixels
        protected List<InventoryBagItem> items = new List<InventoryBagItem>();
        protected Rect2? dropzoneIndicator = null;
        public override void _Ready()
        {
            this.SetMouseFilter(Control.MouseFilterEnum.Pass);
            this.initItemGrid(new Vector2(4, 6));

            this.addItem(new InventoryBagItem(Util.imageToTexture("res://static/dummy-sword-inventory.png")));
            this.addItem(new InventoryBagItem(Util.imageToTexture("res://static/dummy-sword-inventory.png")));
            this.addItem(new InventoryBagItem(Util.imageToTexture("res://static/dummy-sword-inventory-1x2.png"), new Vector2(1, 2)));
        }

        public override void _Input(InputEvent @event){
            if (@event is InputEventMouseMotion eventMouseMotion) {
                if (InventoryInteraction.hasPickedUpItem()) {
                    //this.AcceptEvent(); //handled in _GuiInput instead
                    InventoryBagItem item = InventoryInteraction.getPickedUpItem();
                    item.SetGlobalPosition(eventMouseMotion.GetGlobalPosition() - (item.GetCustomMinimumSize() / 2));
                    this.Update();
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
                                this.dropzoneIndicator = null;
                                this.Update();
                            }
                        }
                    }
                }
            }
        }

        public override void _GuiInput(InputEvent @event){
            if (@event is InputEventMouseMotion eventMouseMotion) {
                /**
                 * we only want this to happen when the player is hovering over the inventory grid
                 * which is what _GuiInput implicitly does
                 */
                if (InventoryInteraction.hasPickedUpItem()) {

                    var relativeMousePosition = eventMouseMotion.Position;
                    var targetCellPosition = this.localToCellPosition(relativeMousePosition);
                    var item = InventoryInteraction.getPickedUpItem();

                    this.dropzoneIndicator = new Rect2(this.cellPositionToLocal(targetCellPosition), item.GetCustomMinimumSize());
                    this.AcceptEvent();
                } else {
                    if (this.dropzoneIndicator != null) {
                        this.dropzoneIndicator = null;
                    }
                }
            }
        }

        public override void _Draw() {
            if (this.dropzoneIndicator != null) {
                var style = new StyleBoxFlat();
                //style.DrawCenter = false;
                style.BgColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
                style.BorderBlend = true;
                style.BorderColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                style.SetBorderWidthAll(2);
                this.DrawStyleBox(style, (Rect2)this.dropzoneIndicator);
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
                    tmp.ShowBehindParent = true;

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
}
