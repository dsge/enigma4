using Godot;
using System;
using Godot.Collections;

public class Main : Godot.Spatial
{

    protected Vector3 raycastFrom = Vector3.Zero;
    protected Vector3 raycastTo = Vector3.Zero;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("This is the same as overriding _Ready()... 2");

        AddChild(this.createFloor());
        AddChild(this.createPlayerCharacter());

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
                /*var obj = (Godot.Spatial)result["collider"];
                var obj2 = obj.GetNode<Godot.Spatial>("..");*/
                var obj = result["collider"];
                if (obj is RigidBody rigidBody) {
                    if (rigidBody.Sleeping) {
                        rigidBody.Sleeping = false;
                    }
                }
                GD.Print("hit");
            } else {
                GD.Print("miss");
            }
            this.raycastFrom = Vector3.Zero;
            this.raycastTo = Vector3.Zero;
        }
    }

    protected Godot.Spatial createFloor() {
        SurfaceTool surfaceTool = new SurfaceTool();
        surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        surfaceTool.AddVertex(new Vector3(15f, 0f, 0f));
        surfaceTool.AddVertex(new Vector3(15f, 0f, 15f));
        surfaceTool.AddVertex(new Vector3(0f, 0f, 15f));
        surfaceTool.AddVertex(new Vector3(0f, 0f, 15f));
        surfaceTool.AddVertex(new Vector3(0f, 0f, 0f));
        surfaceTool.AddVertex(new Vector3(15f, 0f, 0f));
        surfaceTool.AddIndex(0);
        surfaceTool.AddIndex(1);
        surfaceTool.AddIndex(2);
        surfaceTool.AddIndex(3);
        surfaceTool.AddIndex(4);
        surfaceTool.AddIndex(5);
        ArrayMesh arrayMesh = surfaceTool.Commit();

        var meshInstance = new MeshInstance();
        var mat = new SpatialMaterial();
        var color = new Color(0.9f, 0.1f, 0.1f);
        mat.AlbedoColor = color;
        meshInstance.Mesh = arrayMesh;
        meshInstance.Translation = new Vector3(-7.5f, 0, -7.5f);

        StaticBody rigidBody = new StaticBody();
        // rigidBody.Mode = RigidBody.ModeEnum.Static;

        BoxShape boxShape = new BoxShape();
        boxShape.Extents = new Vector3(7.5f, 0.0001f, 7.5f);

        CollisionShape collisionShape = new CollisionShape();
        collisionShape.Shape = boxShape;

        rigidBody.AddChild(collisionShape);
        rigidBody.AddChild(meshInstance);

        Godot.Spatial ret = new Godot.Spatial();

        ret.AddChild(rigidBody);
        //ret.Translation = new Vector3(-7.5f, 0, -7.5f);


        Navigation nav = new Navigation();

        NavigationMesh navMesh = new NavigationMesh();
        navMesh.CreateFromMesh(meshInstance.Mesh);

        nav.NavmeshAdd(navMesh, meshInstance.Transform);

        var path = nav.GetSimplePath(new Vector3(0, 0, 0), new Vector3(3f, 0, 0));

        return ret;
    }

    protected RigidBody createPlayerCharacter() {
        var packedScene = GD.Load<PackedScene>("res://gameObjects/Cylinder.tscn");

        MeshInstance ret = (MeshInstance)packedScene.Instance();
        ret.Translation = new Vector3(0, 0, 0);

        RigidBody rigidBody = new RigidBody();

        CylinderShape shape = new CylinderShape();
        shape.Radius = 1f;
        shape.Height = 2f;

        CollisionShape collisionShape = new CollisionShape();
        collisionShape.Shape = shape;

        rigidBody.AddChild(collisionShape);

        rigidBody.AddChild(ret);

        rigidBody.Translation = new Vector3(0, 0.6f + 5f, 0);

        rigidBody.Sleeping = true;

        return rigidBody;
    }
}
