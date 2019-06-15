using Godot;
using System;
using Godot.Collections;

public class Spatial : Godot.Spatial
{

    protected Vector3 raycastFrom = Vector3.Zero;
    protected Vector3 raycastTo = Vector3.Zero;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("This is the same as overriding _Ready()... 2");

        AddChild(this.createFloor());

        Godot.Spatial camera = GetNode<Godot.Spatial>("Camera");
        camera.Translation = new Vector3(10f, 10f, 10f);
        camera.LookAt(new Vector3(0, 0, 0), new Vector3(0, 1f, 0));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Godot.Spatial cube = GetNode<Godot.Spatial>("Cube");

        cube.Rotation = new Vector3(cube.Rotation.x + delta * 2, cube.Rotation.y, cube.Rotation.z);
    }

    public override void _Input(InputEvent @event){
        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == 1 && eventMouseButton.Pressed) {
                var rayLength = 1000;
                var camera = (Camera)GetNode("Camera");
                this.raycastFrom = camera.ProjectRayOrigin(eventMouseButton.Position);
                this.raycastTo = raycastFrom + camera.ProjectRayNormal(eventMouseButton.Position) * rayLength;
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!this.raycastFrom.Equals(Vector3.Zero) && !this.raycastFrom.Equals(Vector3.Zero)) {
            var spaceState = GetWorld().DirectSpaceState;
            Dictionary result = spaceState.IntersectRay(this.raycastFrom, this.raycastTo);
            if (result.Count > 0) {
                var obj = (Godot.StaticBody)result["collider"];
                var obj2 = obj.GetNode<Godot.Spatial>("..");
                GD.Print("hit");
            } else {
                GD.Print("miss");
            }
            this.raycastFrom = Vector3.Zero;
            this.raycastTo = Vector3.Zero;
        }
    }

    protected MeshInstance createFloor() {
        SurfaceTool surfaceTool = new SurfaceTool();
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        surfaceTool.AddVertex(new Vector3(1f, 0f, 0f));
        surfaceTool.AddVertex(new Vector3(1f, 0f, 1f));
        surfaceTool.AddVertex(new Vector3(0f, 0f, 1f));
        surfaceTool.AddVertex(new Vector3(0f, 0f, 1f));
        surfaceTool.AddVertex(new Vector3(0f, 0f, 0f));
        surfaceTool.AddVertex(new Vector3(1f, 0f, 0f));
        ArrayMesh arrayMesh = surfaceTool.Commit();

        var ret = new MeshInstance();
        var mat = new SpatialMaterial();
        var color = new Color(0.9f, 0.1f, 0.1f);
        mat.AlbedoColor = color;
        ret.Mesh = arrayMesh;
        ret.Scale = new Vector3(15f, 15f, 15f);
        ret.Translation = new Vector3(-7.5f, 0, -7.5f);


        ret.CreateConvexCollision();

        return ret;
    }
}
