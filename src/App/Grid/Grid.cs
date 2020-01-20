using Godot;

namespace App.Grid
{

    public class Grid: Godot.Spatial {
        /**
         * base size of the grid in ingame meters
         */
        protected float cellSize = 2.5f;

        protected GridElectricSystem electricSystem = null;


        public override void _Ready() {

            this.setElectricSystem(new GridElectricSystem(this));

            this.addCellAt(new DummyBlock(), new Vector3(3f, 0, 0));
            this.addCellAt(new DummyBlock(), new Vector3(4f, 0, 0));
            this.addCellAt(new DummyBlock1x2(), new Vector3(3f, 0, 1f));
            this.addCellAt(new ThrusterBlock(), new Vector3(5f, 0, 1f));
            this.addCellAt(new CockpitBlock(), new Vector3(2f, 0, 0));
        }

        public void addCellAt(GridCell cell, Vector3 position) {
            cell.Translation = this.mapToWorld(position);
            cell.setGrid(this);
            this.AddChild(cell);
        }

        public Vector3 mapToWorld(Vector3 gridCoordinates) {
            return gridCoordinates * this.cellSize;
        }

        public Vector3 worldToMap(Vector3 worldCoordinates) {
            return worldCoordinates / this.cellSize;
        }

        public void setElectricSystem(GridElectricSystem value) {
            if (this.electricSystem != null) {
                this.RemoveChild(this.electricSystem);
            }
            this.electricSystem = value;
            this.AddChild(this.electricSystem);
        }

        public GridElectricSystem getElectricSystem() {
            return this.electricSystem;
        }
    }
}
