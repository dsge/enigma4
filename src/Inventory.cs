using Godot;

namespace App
{
    public class Inventory: PanelContainer {


        public override void _Ready()
        {
            this.SetMouseFilter(Control.MouseFilterEnum.Ignore);
            var bag = new InventoryBag();


            bag.AnchorLeft = 0f;
            bag.AnchorTop = 0;

            bag.MarginLeft = 60;
            bag.MarginRight = -60;
            bag.MarginTop = 100;
            this.GetNode("Panel").AddChild(bag);
        }
    }
}
