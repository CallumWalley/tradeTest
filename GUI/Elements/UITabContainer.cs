using Godot;
using System;

public partial class UITabContainer : TabContainer, UIInterfaces.IEFrameUpdatable
{
	public void OnEFrameUpdate()
	{
		foreach (Control c in GetChildren())
		{
			if (c is UIInterfaces.IEFrameUpdatable)
			{
				((UIInterfaces.IEFrameUpdatable)c).OnEFrameUpdate();
			}
		}
	}
}
