using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public class Main : Godot.Spatial
{

    protected Vector3 raycastFrom = Vector3.Zero;
    protected Vector3 raycastTo = Vector3.Zero;
    // Called when the node enters the scene tree for the first time.
    protected Navigation navigation = null;
    protected KinematicBody playerCharacter = null;
    protected Camera mainCamera = null;
    protected ImmediateGeometry immediateGeometry = null;
    protected List<Vector3> playerMovementPath;
    public override void _Ready()
    {
        GD.Print("This is the same as overriding _Ready()... 2");

        this.mainCamera = (Camera)GetNode("Navigation/PlayerCharacter/Camera");
        this.playerCharacter = (KinematicBody)GetNode("Navigation/PlayerCharacter");
        //this.navigation = (Navigation)GetNode("Navigation");
        this.navigation = new Navigation();
        this.immediateGeometry = (ImmediateGeometry)GetNode("ImmediateGeometry");

        var map = (Spatial)GetNode("Navigation/map");
        if (map != null && this.navigation != null) {
            this.generateNavigationMeshInstances(this.navigation, map.GetChildren());
            if (this.playerCharacter != null) {
                var closestPoint = this.navigation.GetClosestPoint(this.playerCharacter.Translation);
                closestPoint.y = (float)Math.Round(closestPoint.y);
                closestPoint.y = 0;
                // this.playerCharacter.Translation = closestPoint;
            }
        }


    }

    protected void generateNavigationMeshInstances(Navigation nav, Godot.Collections.Array nodes) {
        for (int i = 0; i < nodes.Count; i++) {
            if (nodes[i] is StaticBody staticBody) {
                var meshInstance = staticBody.GetNodeOrNull<MeshInstance>("MeshInstance");
                if (meshInstance != null) {
                    NavigationMesh navMesh = new NavigationMesh();
                    navMesh.CreateFromMesh(meshInstance.Mesh);
                    nav.NavmeshAdd(navMesh, meshInstance.GetGlobalTransform());
                    var transform = meshInstance.Transform;
                    var translation = meshInstance.Translation;
                    var globalTransform = meshInstance.GlobalTransform;
                    GD.Print("created navmesh for meshinstance");
                } else {
                    GD.Print("[navmesh] meshinstance skipped (no child named 'MeshInstance')");
                }
            } else {
                GD.Print("[navmesh] meshinstance skipped (no staticbody)");
            }

        }
    }

    public override void _Process(float delta)
    {
        if (this.immediateGeometry != null) {
            this.immediateGeometry.Clear();
            if (this.playerMovementPath != null) {
                this.immediateGeometry.Begin(Mesh.PrimitiveType.LineStrip);
                for (int i = 0; i < this.playerMovementPath.Count; i++) {
                    this.immediateGeometry.AddVertex(
                        new Vector3(
                            this.playerMovementPath[i].x,
                            this.playerMovementPath[i].y + 1f,
                            this.playerMovementPath[i].z
                        )
                    );
                }
                this.immediateGeometry.End();
            }
        }
    }

    public override void _Input(InputEvent @event){
        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == 1 && eventMouseButton.Pressed) {
                var rayLength = 1000;
                var camera = this.mainCamera;
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

                if (obj is StaticBody staticBody && this.playerCharacter != null && this.navigation != null) {
                    Vector3 to = (Vector3)result["position"];
                    Vector3 from = this.navigation.GetClosestPoint(this.playerCharacter.Translation);

                    var path = this.navigation.GetSimplePath(
                        from,
                        to
                    );
                    if (path.Length > 1) {
                        this.playerMovementPath = new List<Vector3>(path);
                        GD.Print("path found");
                    } else {
                        this.playerMovementPath = null;
                        GD.Print("path not found");
                    }
                }

                if (!(obj is StaticBody) && !(obj is RigidBody)) {
                    this.playerMovementPath = null;
                    GD.Print("unknown object type, ignoring");
                }
            } else {
                GD.Print("miss");
            }
            this.raycastFrom = Vector3.Zero;
            this.raycastTo = Vector3.Zero;
        }

        if (this.playerMovementPath != null && this.playerCharacter != null) {
            Vector3 targetPoint = new Vector3(this.playerMovementPath[1]);
            var speed = 10f;
            var targetDirection = this.playerCharacter.ToLocal(targetPoint).Normalized();
            var collision = this.playerCharacter.MoveAndSlide(targetDirection * speed);
            var isOnFloor = this.playerCharacter.IsOnFloor();
            var translation = this.playerCharacter.Translation;

            var distance = this.navigation.GetClosestPoint(this.playerCharacter.Translation).DistanceTo(targetPoint);
            if (distance < (speed * delta)) {
                this.playerMovementPath.RemoveAt(0);
                if (this.playerMovementPath.Count < 2) {
                    this.playerMovementPath = null;
                }
            }

            if (this.playerCharacter.IsOnFloor()){
                GD.Print("is on floor");
            } else {
                // GD.Print("is NOT on floor " + this.playerCharacter.Translation.y);
            }
        }
    }
}

