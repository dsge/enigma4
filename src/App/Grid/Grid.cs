using Godot;

namespace App.Grid
{

    public class Grid: Godot.Spatial {
        /**
         * base size of the grid in ingame meters
         */
        protected float cellSize = 2.5f;


        public override void _Ready() {

            this.addCellAt(new DummyBLock(), new Vector3(3f, 0, 0));
        }

        public void addCellAt(GridCell cell, Vector3 position) {
            cell.Translation = this.mapToWorld(position);
            cell.setGrid(this);
            this.AddChild(cell);
        }

        public Vector3 mapToWorld(Vector3 gridCoordinates) {
            return Vector3.Zero;
        }

        public Vector3 worldToMap(Vector3 worldCoordinates) {
            return Vector3.Zero;
        }
    }
}
