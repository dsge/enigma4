using Godot;

namespace App
{

    /**
     * since navigation in Godot is still pretty primitive (hard to setup navmesh from multiple parts and add obstacles at runtime)
     * we will try to rely on it as little as possible
     *
     * but we will still maintain some compatibility with Godot's Navigation API to ease future refactoring
     */
    public class Navigation: Godot.Navigation {

        new public Vector3 GetClosestPoint(Vector3 toPoint) {
            return toPoint;
        }
        new public Vector3[] GetSimplePath(Vector3 start, Vector3 end, bool optimize = true){
            return new Vector3[] {start, end};
        }
    }
}
