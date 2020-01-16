using Godot;

namespace App.Grid
{
    public class CockpitBlock: GridCell {
        protected new Vector3 size = new Vector3(2f, 1f, 1f);
        protected PackedScene prototype = (PackedScene)GD.Load("res://gameObjects/Grid/CockpitBlock.tscn");
        public override void _Ready() {
            var instance = prototype.Instance();
            this.AddChild(instance);
        }

        public void activateManualThrust() {
            this.FindNode("MeshInstance2", false);
        }

        public void deactivateManualThrust() {

        }
    }
}
