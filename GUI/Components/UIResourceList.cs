using Godot;
using System;
using System.Collections.Generic;

public class UIResourceList : UIList
{
	static readonly PackedScene p_resource = (PackedScene)GD.Load<PackedScene>("res://GUI/Elements/Display/UIResource.tscn");

	// wrapper
	public void Init(IEnumerable<System.Object> _object)
	{
		base.Init(_object, p_resource);
	}
}
