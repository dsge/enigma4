using Godot;

namespace App.Grid
{
    public class GridElectricSystem: Godot.Node {

        protected Grid grid = null;

        public GridElectricSystem(Grid grid = null) {
            if (grid != null) {
                this.setGrid(grid);
            }
        }

        public void setGrid(Grid value) {
            this.grid = value;
        }

        public Grid getGrid() {
            return this.grid;
        }

        public string getChildrenGroupId() {
            ulong gridId = this.getGrid().GetInstanceId();

            return "foo" + gridId;
        }

        public override void _Process(float delta) {
            var cells = this.GetTree().GetNodesInGroup(this.getChildrenGroupId());

            if (cells.Count > 0) {

            }
        }
    }
}
