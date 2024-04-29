using Godot;
using System;

public partial class UIPanel : PanelContainer
{

	public override void _Ready()
	{
		GetNode<Global>("/root/Global").Connect("EFrameUI", callable: new Callable(this, "OnEFrameUI"));
	}
	public virtual void OnEFrameUI()
	{
		// Impliment any updates here that need be called less frequently than 'Draw';
		// e.g. game data updates.
		if (Visible){
			OnEFrameUpdate();
		}
	}

	public virtual void OnEFrameUpdate(){
		
	}
}
