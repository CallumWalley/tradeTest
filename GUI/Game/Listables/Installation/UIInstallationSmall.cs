using Godot;
using System;
using System.Linq;
using System.Collections;

public partial class UIInstallationSmall : Control, UIContainers.IListable<Installation>
{
    Installation installation;
    public Installation GameElement { get { return installation; } }
    public bool Destroy { get; set; } = false;

    Label labelName;

    public override void _Ready()
    {
        labelName = GetNode<Label>("Name");
    }
    public void Init(Installation _installation)
    {
        installation = _installation;
    }
    public override void _Draw()
    {
        if (installation == null) { return; }
        labelName.Text = installation.Name;
        if (Destroy)
        {
            QueueFree();
        }
    }
}
