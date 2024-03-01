using Godot;
using System;
using System.Linq;
using System.Collections;

public partial class UIResourcePoolSmall : Control, Lists.IListable<ResourcePool>
{
    ResourcePool ResourcePool;
    public ResourcePool GameElement { get { return ResourcePool; } }
    public bool Destroy { get; set; } = false;

    Label labelName;

    public override void _Ready()
    {
        labelName = GetNode<Label>("Name");
    }
    public void Init(ResourcePool _ResourcePool)
    {
        ResourcePool = _ResourcePool;
    }
    public void Update()
    {

    }
    public override void _Draw()
    {
        if (ResourcePool == null) { return; }
        labelName.Text = ResourcePool.Name;
        if (Destroy)
        {
            QueueFree();
        }
    }
}
