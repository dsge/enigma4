
using Godot;

namespace App.Inventory
{
    /**
     * an InventoryItem's 3d node in the ground
     */
    public class GroundNode : Spatial {
        protected MeshInstance meshInstance = null;

        public InventoryItem item = null;

        public GroundNode(MeshInstance meshInstance) {
            this.meshInstance = meshInstance;
        }
        public override void _Ready() {
            this.AddChild(this.meshInstance);
        }
    }
}
