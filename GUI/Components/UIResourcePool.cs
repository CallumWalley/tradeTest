using Godot;
using System;

public class UIResourcePool: Control
{   
	[Export]
	public ResourcePool resourcePool;
	static readonly PackedScene resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UIResource.tscn");


	public void Init(ResourcePool _resourcePool){
		resourcePool=_resourcePool;
	}

	public override void _Draw(){
		GD.Print(resourcePool.members.Count);
		//Clear();
		foreach (UIResource r in GetNode("ResourcePool").GetChildren()){
			GetNode("ResourcePool").RemoveChild(r);
			r.QueueFree();
		}
		foreach (Resource r in resourcePool.GetStandard()){
			UIResource ui = resourceIcon.Instance<UIResource>();
			ui.Init(r);
			ui.showDetails = true;
			GetNode("ResourcePool").AddChild(ui);
		}
	}
}
