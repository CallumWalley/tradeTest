using Godot;
using System;

public partial class UIResourcePoolTiny : HBoxContainer
{
    ResourcePool ResourcePool;

    public void Init(ResourcePool _ResourcePool)
    {
        ResourcePool = _ResourcePool;
    }

    public override void _Draw()
    {
        base._Draw();
        // all icons currently the same.
        //GetNode<TextureRect>("Icon").Texture;
        GetNode<Label>("Name").Text = ResourcePool.Name;
    }
}