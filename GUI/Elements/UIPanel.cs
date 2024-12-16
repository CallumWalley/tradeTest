using Godot;
using System;

public partial class UIPanel : PanelContainer, UIInterfaces.IEFrameUpdatable
{

	public override void _Ready()
	{
		GetNode<Global>("/root/Global").Connect("EFrameUI", callable: new Callable(this, "OnEFrameUI"));
		GetNode<Global>("/root/Global").Connect("Setup", callable: new Callable(this, "OnSetup"));

	}
	public virtual void OnEFrameUI()
	{
		// Impliment any updates here that need be called less frequently than 'Draw';
		// e.g. game data updates.
		if (IsVisibleInTree())
		{
			OnEFrameUpdate();
		}
	}

	public virtual void OnSetup()
	{
		// Called once, before the first EFrame, but after ready
	}
	public virtual void OnEFrameUpdate()
	{

	}
}
