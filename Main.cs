using Godot;
using System;
using Godot.Collections;

public class Main : Godot.Spatial
{

    protected Vector3 raycastFrom = Vector3.Zero;
    protected Vector3 raycastTo = Vector3.Zero;
    // Called when the node enters the scene tree for the first time.
    protected Navigation navigation = null;
    protected KinematicBody playerCharacter = null;
    protected Camera mainCamera = null;
    protected ImmediateGeometry immediateGeometry = null;
    protected Vector3[] playerMovementPath;
    public override void _Ready()
    {
        GD.Print("This is the same as overriding _Ready()... 2");

        this.mainCamera = (Camera)GetNode("Navigation/PlayerCharacter/Camera");
        this.playerCharacter = (KinematicBody)GetNode("Navigation/PlayerCharacter");
        this.navigation = (Navigation)GetNode("Navigation");
        this.immediateGeometry = (ImmediateGeometry)GetNode("ImmediateGeometry");
    }

    public override void _Process(float delta)
    {
        if (this.immediateGeometry != null && this.playerMovementPath != null) {
            this.immediateGeometry.Clear();
            this.immediateGeometry.Begin(Mesh.PrimitiveType.LineStrip);
            for (int i = 0; i < this.playerMovementPath.Length; i++) {
                this.immediateGeometry.AddVertex(new Vector3(this.playerMovementPath[i].x, this.playerMovementPath[i].y + 1f, this.playerMovementPath[i].z));
            }
            this.immediateGeometry.End();
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
                    Vector3 position = (Vector3)result["position"];
                    var path = this.navigation.GetSimplePath(this.playerCharacter.Translation, position);
                    if (path.Length > 0) {
                        if (this.playerCharacter.IsOnFloor()) {
                            // this.playerCharacter.MoveLockY = true;
                        }
                        this.playerMovementPath = path;
                        GD.Print("path found");
                    } else {
                        GD.Print("path not found");
                    }
                }
            } else {
                GD.Print("miss");
            }
            this.raycastFrom = Vector3.Zero;
            this.raycastTo = Vector3.Zero;
        }

        if (this.playerMovementPath != null && this.playerCharacter != null) {
            Vector3 targetPoint = this.playerMovementPath[1];
            var speed = 10f;
            var collision = this.playerCharacter.MoveAndSlide(this.playerCharacter.ToLocal(targetPoint).Normalized() * speed);
            if (collision != null) {
                GD.Print("collision");
            }
            var distance = this.playerCharacter.Translation.DistanceTo(targetPoint);
            if (distance < 1f) {
                this.playerMovementPath = null;
            }
        }
    }
}
