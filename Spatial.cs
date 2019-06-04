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

        SurfaceTool surfaceTool = new SurfaceTool();
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        surfaceTool.AddVertex(new Vector3(0f, 0f, 1f));
        surfaceTool.AddVertex(new Vector3(0f, 1f, 0f));
        surfaceTool.AddVertex(new Vector3(1f, 0f, 0f));
        ArrayMesh arrayMesh = surfaceTool.Commit();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Godot.Spatial cube = GetNode<Godot.Spatial>("../Cube");

        cube.Rotation = new Vector3(cube.Rotation.x + delta * 2, cube.Rotation.y, cube.Rotation.z);
    }
}
