using Godot;
using System;

public partial class UIPanelPoolTrade : UIPanel
{
	public ResourcePool resourcePool;

	public UIDropDownSetHead uIDropDownSetHead;
	public override void _Ready()
	{
		uIDropDownSetHead = GetNode<UIDropDownSetHead>("VBoxContainer/DropDown");
		uIDropDownSetHead.Init(resourcePool);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
