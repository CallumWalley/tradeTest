using Godot;
using System;

public partial class UIPanel : PanelContainer,  UIInterfaces.IEFrameUpdatable
{
	public virtual void OnEFrameUpdate()
	{
		// Impliment any updates here that need be called less frequently than 'Draw';
		// e.g. game data updates.
		QueueRedraw();
	}
}
