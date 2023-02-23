using Godot;
using System;

public class InfoCard : Container
{
	bool focus = false;
	float count = 0f;
	static float timeToHide = 1f;


	private void Focus(){
	 GD.Print("focus");
	 focus = true;
	 Visible = true;
		// When mouse over this element.
//		if (resourceGroup.GetChildCount() > 0){
//			focus = true;
//			Visible = true;
//		}
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
