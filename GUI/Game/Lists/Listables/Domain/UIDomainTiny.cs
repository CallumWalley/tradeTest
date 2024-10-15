using Godot;
using System;
namespace Game;

public partial class UIDomainTiny : HBoxContainer
{
    Domain Domain;

    public void Init(Domain _Domain)
    {
        Domain = _Domain;
    }

    public override void _Draw()
    {
        base._Draw();
        // all icons currently the same.
        //GetNode<TextureRect>("Icon").Texture;
        GetNode<Label>("Name").Text = Domain.Name;
    }
}