using Godot;
using System;
using System.Collections.Generic;

public partial class UIDrawEFrame : Control
{

    public override void _Ready()
    {
        base._Ready();
        GetNode<Global>("/root/Global").Connect("EFrameUI", new Callable(this, "EFrameUI"));

    }

    public void EFrameUI()
    {
        _Draw();
    }

    public override void _Draw()
    {
        base._Draw();
    }

}
