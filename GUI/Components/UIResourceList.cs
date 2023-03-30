using Godot;
using System;
using System.Collections.Generic;

public class UIResourceList: Control
{   
	public List<Resource> resourceList;
	static readonly PackedScene p_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/UIResource.tscn");
	HBoxContainer hbox;

	public void Init(List<Resource> _resourceList){
		resourceList=_resourceList;
		hbox = GetNode<HBoxContainer>("HBoxContainer");
	}

	public override void _Draw(){
		// Go over all trade routes in pool, and either update or create. 
		int index = 0;
		foreach (Resource r in resourceList){
			UpdateResource(r: r,index);
			index++;
		}
		// Any remaining elements greater than index must no longer exist.
		while (hbox.GetChildCount()>index){
			UIResource uir = hbox.GetChildOrNull<UIResource>(index+1);
			hbox.RemoveChild(uir);
			uir.QueueFree();
		}
		// foreach (UIResource r in hbox.GetChildren()){
		// 	hbox.RemoveChild(r);
		// 	GD.Print("Removing UI resource");
		// 	r.QueueFree();
		// }
		// foreach (Resource r in resourceList){
		// 	GD.Print("Adding UI resource");
		// 	UIResource ui = p_resourceIcon.Instance<UIResource>();
		// 	ui.Init(r);
		// 	ui.showDetails = true;
		// 	Control c = GetNode<Control>("HBoxContainer");
		// 	c.AddChild(ui);
		// }
	}

	void UpdateResource(Resource r, int index){
		
		foreach (UIResource uir in hbox.GetChildren()){
			if (r.Type == uir.resource.Type){
				hbox.MoveChild(uir, index);
				return;
			}
		}
		// If doesn't exist, add it and insert at postition.
		UIResource ui = (UIResource)p_resourceIcon.Instance();
		ui.Init(r);
		hbox.AddChild(ui);
		hbox.MoveChild(ui, index);
	}
}
