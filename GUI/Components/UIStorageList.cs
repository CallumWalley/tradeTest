using Godot;
using System;
using System.Collections.Generic;


// Class for generating a list of elements showing the remaining storage.
// Has a lot in common with draw resource class. Maybe merge?
public class UIStorageList : Control
{
	public Installation installation;
	static readonly PackedScene p_storageIcon = (PackedScene)GD.Load<PackedScene>("res://GUI/Components/UIStorage.tscn");
	protected HBoxContainer hbox;

	public void Init(Installation _installation)
	{
		installation = _installation;
	}

	public override void _Draw()
	{
		// if (installation == null){
		// 	return;
		// }
		int index = 0;
		foreach (Resource r in installation.resourceDelta)
		{
			if (r.Storable)
			{
				UpdateStorage(installation.resourceStorage.GetType(r.Type), index);
				index++;
			}
		}
		// Any remaining elements greater than index must no longer exist.
		while (hbox.GetChildCount() > index)
		{
			UIResource uir = hbox.GetChildOrNull<UIResource>(index + 1);
			hbox.RemoveChild(uir);
			uir.QueueFree();
		}
	}

	public override void _Ready()
	{
		GetNode<Global>("/root/Global").Connect("EFrameEarly", this, "DeferredDraw");
		hbox = GetNode<HBoxContainer>("HBoxContainer");

	}
	void DeferredDraw()
	{
		Visible = false;
		SetDeferred("visible", true);
	}
	void UpdateStorage(Resource r, int index)
	{
		foreach (UIStorage uir in hbox.GetChildren())
		{
			if (r.Type == uir.storage.Type)
			{
				hbox.MoveChild(uir, index);
				return;
			}
		}
		List<Resource> l = new List<Resource>();
		// If doesn't exist, add it and insert at postition.
		UIStorage ui = (UIStorage)p_storageIcon.Instance();
		ui.Init(r, installation.resourceStockpile.GetType(r.Type), installation.resourceDelta.GetType(r.Type));
		hbox.AddChild(ui);
		hbox.MoveChild(ui, index);
	}
}
