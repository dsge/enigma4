using Godot;

namespace App.Grid
{
    public abstract class GridCell: Godot.Spatial {

        protected Grid grid = null;
        protected Vector3 size = new Vector3(1f, 1f, 1f);
        protected CellElectricity cellElectricity = null;

        public GridCell(Grid grid = null) {
            if (grid != null) {
                this.setGrid(grid);
            }
        }

        public void setCellElectricity(CellElectricity value) {
            this.cellElectricity = value;
        }

        public CellElectricity getCellElectricity() {
            return this.cellElectricity;
        }

        public void setGrid(Grid value) {
            this.grid = value;
            this.AddToGroup(value.getElectricSystem().getChildrenGroupId());
        }

        public Grid getGrid() {
            return this.grid;
        }

        public void setSize(Vector3 value) {
            this.size = value;
        }

        public Vector3 getSize() {
            return this.size;
        }

    }
}
