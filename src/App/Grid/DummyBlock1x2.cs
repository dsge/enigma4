using Godot;

namespace App.Grid
{
    public class DummyBlock1x2: GridCell {

        protected new Vector3 size = new Vector3(2f, 1f, 1f);
        protected PackedScene prototype = (PackedScene)GD.Load("res://gameObjects/Grid/DummyBlock1x2.tscn");
        public override void _Ready() {
            var instance = prototype.Instance();
            this.AddChild(instance);
        }
    }
}
