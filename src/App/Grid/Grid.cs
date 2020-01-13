using Godot;

namespace App.Grid
{

    public class Grid: Godot.Spatial {
        /**
         * base size of the grid in ingame meters
         */
        protected float cellSize = 2.5f;


        public Vector3 mapToWorld(Vector3 gridCoordinates) {
            return Vector3.Zero;
        }

        public Vector3 worldToMap(Vector3 worldCoordinates) {
            return Vector3.Zero;
        }
    }
}
