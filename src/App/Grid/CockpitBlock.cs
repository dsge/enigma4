using Godot;

namespace App.Grid
{
    public class CockpitBlock: GridCell {
        protected new Vector3 size = new Vector3(2f, 1f, 1f);
        protected PackedScene prototype = (PackedScene)GD.Load("res://gameObjects/Grid/Cockpit.tscn");
        protected Node instance = null;

        public override void _Ready() {
            this.instance = prototype.Instance();
            this.AddChild(this.instance);

            this.FindNode("window", true, false).FindNode("StaticBody", true, false).Connect("mouse_entered", this, "onCockpitWindowMouseEntered");
            this.FindNode("window", true, false).FindNode("StaticBody", true, false).Connect("mouse_exited", this, "onCockpitWindowMouseExited");
        }

        protected void onCockpitWindowMouseEntered() {
            this.setWindowHighlight(true);
        }

        protected void onCockpitWindowMouseExited() {
            this.setWindowHighlight(false);
        }

        protected void setWindowHighlight(bool value) {
            AnimationPlayer animationPlayer = (AnimationPlayer)this.instance.FindNode("AnimationPlayer", true, false);

            if (value) {
                animationPlayer.Play("windowhighlight");
            } else {
                animationPlayer.PlayBackwards("windowhighlight");
            }
        }
    }
}
