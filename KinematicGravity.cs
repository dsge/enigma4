using Godot;
using System;
using Godot.Collections;

public class KinematicGravity : KinematicBody
{
    float fallingSpeed = 0;
    const float gravity = 0.5f;
    public override void _PhysicsProcess(float delta) {
        if (!this.IsOnFloor()) {
            this.fallingSpeed += gravity * delta;
            this.MoveAndCollide(new Vector3(0, fallingSpeed * -1, 0));
            if (this.IsOnFloor()) {
                this.fallingSpeed = 0;
            }
        }
    }
}
