
using Godot;

namespace App.Inventory
{
    /**
     * an InventoryItem's 3d node in the ground
     */
    public class GroundNode: Spatial {
        protected MeshInstance meshInstance = null;

        public InventoryItem item = null;
        public bool shouldHaveHoverEffects = false;

        public GroundNode(MeshInstance meshInstance) : base() {
            this.meshInstance = meshInstance;
            Godot.Collections.Array children = meshInstance.GetChildren();
            foreach (var child in children) {
                if (child is CollisionObject collider) {
                    collider.Connect("input_event", this, nameof(this.onInputEvent));
                    collider.Connect("mouse_entered", this, nameof(this.onMouseEntered));
                    collider.Connect("mouse_exited", this, nameof(this.onMouseExited));
                    break;
                }
            }
            // meshInstance.MaterialOverride = new
        }

        public void onInputEvent(Node camera, InputEvent @event, Vector3 click_position, Vector3 click_normal, int shape_idx) {
            if (@event is InputEventMouseButton eventMouseButton) {
                if (eventMouseButton.ButtonIndex == 1) {
                    if (eventMouseButton.Pressed) {
                        GD.Print("picking up item from the ground...");
                        ((App.Inventory.InventoryDragOverlay)GetNode("/root/InventoryDragOverlay")).setPickedUpItem(this.item);
                        this.GetParent<Node>().RemoveChild(this);
                    }
                }
            }
        }

        public void onMouseEntered() {
            var mat = new ShaderMaterial();
            mat.Shader = (Shader)GD.Load("res://scenes/item-on-ground-hover.shader");
            this.meshInstance.MaterialOverride = mat;
        }

        public void onMouseExited() {
            this.meshInstance.MaterialOverride = null;
        }

        public override void _Ready() {
            this.AddChild(this.meshInstance);
        }
    }
}
