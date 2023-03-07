using Godot;
using System;

public class UIBody : TabContainer
{
    bool focus;
    public override void _Ready()
    {
        Connect("mouse_entered", this, "Focus");
        Connect("mouse_exited", this, "UnFocus");
    }


    public void Focus(){
        focus = true;
    }

    public void UnFocus(){
        focus = false;
    }

    public override void _Process(float delta)
    {   
        //Close if focus removed.
        if (Visible){
            if (Input.IsActionPressed("ui_cancel")){
                    Visible = false;
            }
            // if (Input.IsActionPressed("ui_select")){
            //     if (focus == false){
            //         Visible = false;
            //     }
            // }
        }
    }
}
