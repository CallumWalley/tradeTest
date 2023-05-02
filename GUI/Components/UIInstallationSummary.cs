using Godot;
using System;
using System.Linq;
using System.Collections;

public partial class UIInstallationSummary : UIElement
{   
    Installation installation;
    Label labelName;

    public override void _Ready()
    {
        base._Ready();
        labelName = GetNode<Label>("Name");
    }
    public void Init(Installation _installation)
    {
        installation=_installation;
    }
    public override void _Draw()
    {
        if (installation == null){return;}
        labelName.Text = installation.Name;
        base._Draw();
    }
}
