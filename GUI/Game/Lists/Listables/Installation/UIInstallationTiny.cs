using Godot;
using System;

public partial class UIInstallationTiny : HBoxContainer
{
    Installation installation;

    public void Init(Installation _installation)
    {
        installation = _installation;
    }

    public override void _Draw()
    {
        base._Draw();
        // all icons currently the same.
        //GetNode<TextureRect>("Icon").Texture;
        GetNode<Label>("Name").Text = installation.Name;
    }
}