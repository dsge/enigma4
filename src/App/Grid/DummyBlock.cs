using Godot;

namespace App.Grid
{
    public class DummyBLock: GridCell {

        protected new Vector3 size = new Vector3(1f, 1f, 1f);
        protected PackedScene prototype = (PackedScene)GD.Load("res://gameObjects/Grid/DummyBlock.tscn");
        public override void _Ready() {
            var instance = prototype.Instance();
            this.AddChild(instance);
        }
    }
}
