using Godot;
using System;

public class PlayerControls : KinematicBody
{
    protected Vector3 linearVelocity = new Vector3(0f, 0f, 0f);
    protected float currentRollSpeed = 0f;
    protected float maxRollSpeed = 0.01f;
    protected float speedMultiplier = 20f;
    public float mouseSensitivity = 0.002f;

    protected PhysicsBody controlledObject = null;

    public PhysicsBody getControlledObject() {
        if (this.controlledObject == null) {
            return this;
        } else {
            return this.controlledObject;
        }
    }

    public void setControlledObject(PhysicsBody value = null) {
        this.controlledObject = value;
    }

    public override void _UnhandledInput(InputEvent @event) {

        if (Input.GetMouseMode() == Input.MouseMode.Captured) {
            if (@event is InputEventMouseMotion eventMouseMotion) {
                /**
                 * turn the whole playerCharacter towards where we are looking
                 */
                this.RotateObjectLocal(Vector3.Left, eventMouseMotion.Relative.y * mouseSensitivity); //x
                this.RotateObjectLocal(Vector3.Down, eventMouseMotion.Relative.x * mouseSensitivity); //y
            }


        }
    }

    public override void _UnhandledKeyInput(InputEventKey @event) {
        if (@event.IsActionPressed("ui_roll_left")){
            this.currentRollSpeed -= maxRollSpeed;
        } else if (@event.IsActionReleased("ui_roll_left")) {
            this.currentRollSpeed += maxRollSpeed;
        }
        if (@event.IsActionPressed("ui_roll_right")){
            this.currentRollSpeed += maxRollSpeed;
        } else if (@event.IsActionReleased("ui_roll_right")) {
            this.currentRollSpeed -= maxRollSpeed;
        }
        if (@event.IsActionPressed("ui_forward")){
            this.linearVelocity += Vector3.Forward * speedMultiplier;
        } else if (@event.IsActionReleased("ui_forward")) {
            this.linearVelocity -= Vector3.Forward * speedMultiplier;
        }
        if (@event.IsActionPressed("ui_backward")){
            this.linearVelocity -= Vector3.Forward * speedMultiplier;
        } else if (@event.IsActionReleased("ui_backward")) {
            this.linearVelocity += Vector3.Forward * speedMultiplier;
        }
        if (@event.IsActionPressed("ui_up")){
            this.linearVelocity += Vector3.Up * speedMultiplier;
        } else if (@event.IsActionReleased("ui_up")) {
            this.linearVelocity -= Vector3.Up * speedMultiplier;
        }
        if (@event.IsActionPressed("ui_down")){
            this.linearVelocity -= Vector3.Up * speedMultiplier;
        } else if (@event.IsActionReleased("ui_down")) {
            this.linearVelocity += Vector3.Up * speedMultiplier;
        }
        if (@event.IsActionPressed("ui_left")){
            this.linearVelocity += Vector3.Right * speedMultiplier;
        } else if (@event.IsActionReleased("ui_left")) {
            this.linearVelocity -= Vector3.Right * speedMultiplier;
        }
        if (@event.IsActionPressed("ui_right")){
            this.linearVelocity -= Vector3.Right * speedMultiplier;
        } else if (@event.IsActionReleased("ui_right")) {
            this.linearVelocity += Vector3.Right * speedMultiplier;
        }

        if (@event.IsActionReleased("ui_mouselock") || @event.IsActionReleased("ui_cancel")) {
            if (Input.GetMouseMode() == Input.MouseMode.Visible) {
                Input.SetMouseMode(Input.MouseMode.Captured);
            } else {
                Input.SetMouseMode(Input.MouseMode.Visible);
            }

        }

        if(@event.IsActionPressed("ui_use_activate")) {
            if (this.getControlledObject() is KinematicBody) {
                this.setControlledObject((RigidBody)this.GetNode("/root/basenode/Navigation/map/myGrid"));
            } else {
                this.setControlledObject(null);
            }

        }
    }

    protected Vector3 getSlideDirection(Spatial relativeToObject) {
        var ret = new Vector3();

        if (this.linearVelocity.z != 0f) { //forward and backward
            ret += relativeToObject.Transform.basis.z * this.linearVelocity.z;
        }

        if (this.linearVelocity.x != 0f) { //left and right
            ret -= relativeToObject.Transform.basis.x * this.linearVelocity.x;
        }

        if (this.linearVelocity.y != 0f) { //up and down
            ret += relativeToObject.Transform.basis.y * this.linearVelocity.y;
        }

        return ret;
    }

    public override void _PhysicsProcess(float delta) {
        if (!this.linearVelocity.Equals(Vector3.Zero)) {
            var controlledObject = this.getControlledObject();
            if (controlledObject is KinematicBody kinematicBody) {
                kinematicBody.MoveAndSlide(this.getSlideDirection(this));
            } else if (controlledObject is RigidBody rigidBody) {
                rigidBody.AddCentralForce(this.getSlideDirection(rigidBody));
            }
        }
        if (this.currentRollSpeed != 0f) {
            this.getControlledObject().RotateObjectLocal(Vector3.Forward, this.currentRollSpeed);
        }
    }
}
