using Godot;
using System;

public class UIInfoCard : Container
{
	[Export]
	public bool focus = false;
	float count = 0f;
	static float timeToHide = 1f;
	
	private bool VisibleChildren(){
		foreach (Control child in GetChildren()){
			GD.Print(child);
			if (child.Visible){
				return true;
			}
		}
		return false;
	}
	
	private void Focus(){
	 GD.Print("focus");
		if (VisibleChildren()){
			focus = true;
		 	Visible = true;
		}
	}

	private void UnFocus(){
	// When mouse over this element.
		GD.Print("unfocus");
		focus = false;
		count = timeToHide;
	}
	public override void _Process(float delta){
		if ( ! focus && Visible ){
			count-=delta;
			if (count < 0){
				Visible = false;
			}
		}
	}
}
