using Godot;

namespace App.Grid
{

    public class Grid: Godot.RigidBody {
        /**
         * base size of the grid in ingame meters
         */
        protected float cellSize = 2.5f;

        protected GridElectricSystem electricSystem = null;
        /**
         * the "forward", "up" and "right" directions are decided by this cockpit
         *
         * this is just a cached value for getForwardTransform() so it does not have to iterate blocks in every frame
         */
        protected CockpitBlock forwardTransformCockpit = null;


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

            if (cell is CockpitBlock cockpit) {
                this.forwardTransformCockpit = cockpit;
            }
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
        /**
         * tells us which way is "forward", "up", and "right" when controlling this grid
         */
        public Transform getForwardTransform() {
            if (this.forwardTransformCockpit == null) {
                return this.Transform;
            } else {
                return this.forwardTransformCockpit.Transform;
            }
        }
    }
}
