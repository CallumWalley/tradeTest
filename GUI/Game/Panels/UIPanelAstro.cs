using Godot;
using System;

public partial class UIPanelAstro : Control
{
    public Body body;

    public void Init(Body _body)
    {
        body = _body;
    }
    public override void _Ready()
    {

    }
}