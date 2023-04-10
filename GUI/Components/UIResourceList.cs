using Godot;
using System;
using System.Collections.Generic;

public class UIResourceList: Control
{   
	public IEnumerable<Resource> resourceList;
	public bool editable;
	static readonly PackedScene p_resourceIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIResource.tscn");
	protected HBoxContainer hbox;

	public void Init(IEnumerable<Resource> _resourceList, bool _editable=false){

		resourceList=_resourceList;
		editable=_editable;
		hbox = GetNode<HBoxContainer>("HBoxContainer/AlignLeft");

		if (_editable ){
			GetNode<TextureButton>("HBoxContainer/AlignRight/Add").Visible = true;
			//TODO impliment
			//GetNode<HBoxContainer>("HBoxContainer/AlignRight/Add")
		}
	}

	public override void _Draw(){
		if (resourceList == null){return;}
		// Go over all trade routes in pool, and either update or create. 
		int index = 0;
		foreach (Resource r in resourceList){
			if (r.Storable){
				UpdateResource(r: r,index);
				index++;
			}
		}
		// Any remaining elements greater than index must no longer exist.
		while (hbox.GetChildCount()>index){
			UIResource uir = hbox.GetChildOrNull<UIResource>(index+1);
			hbox.RemoveChild(uir);
			uir.QueueFree();
		}
	}

	public override void _Ready(){
		GetNode<Global>("/root/Global").Connect("EFrameEarly", this, "DeferredDraw");
	}
	void DeferredDraw(){
		Visible=false;
		SetDeferred("visible", true);
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
