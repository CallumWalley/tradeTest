using Godot;
using System;

public class UIAstroPanel : MarginContainer
{
    public Body body;

    public void Init(Body _body){
       body=_body;
    }
    public override void _Ready(){
       
    }
}
