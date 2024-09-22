using Godot;
using System;

public partial class UIPanelPoolTrade : UIPanel
{
	public Domain resourcePool;

	public UIDropDownSetHead uIDropDownSetHead;
	public override void _Ready()
	{
		uIDropDownSetHead = GetNode<UIDropDownSetHead>("VBoxContainer/DropDown");
		uIDropDownSetHead.Init(resourcePool);
		GetNode<UINetworkInfo>("VBoxContainer/NetworkInfo").domain = resourcePool;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
