using Godot;
using System;

public class Spatial : Godot.Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

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

    protected MeshInstance createFloor() {
        SurfaceTool surfaceTool = new SurfaceTool();
        surfaceTool.Begin(Mesh.PrimitiveType.TriangleFan);
        surfaceTool.AddVertex(new Vector3(1f, 0f, 0f));
        surfaceTool.AddVertex(new Vector3(1f, 0f, 1f));
        surfaceTool.AddVertex(new Vector3(0f, 0f, 1f));
        surfaceTool.AddVertex(new Vector3(0f, 0f, 0f));
        ArrayMesh arrayMesh = surfaceTool.Commit();

        var ret = new MeshInstance();
        var mat = new SpatialMaterial();
        var color = new Color(0.9f, 0.1f, 0.1f);
        mat.AlbedoColor = color;
        ret.Mesh = arrayMesh;
        ret.Scale = new Vector3(15f, 15f, 15f);
        ret.Translation = new Vector3(-7.5f, 0, -7.5f);
        return ret;
    }
}
