using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;
using App;

public class Main : Godot.Spatial
{
    protected PlayerCamera mainCamera = null;

    public override void _Ready()
    {
        this.mainCamera = (PlayerCamera)GetNode("Navigation/PlayerCharacter/Camera");

    }


    public override void _Process(float delta)
    {

    }

    public override void _UnhandledInput(InputEvent @event){
        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == 1 && eventMouseButton.Pressed) {

            }
        }

        if (@event is InputEventKey eventKey) {
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.I) {

            }
            if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.C) {

            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {

    }
}

