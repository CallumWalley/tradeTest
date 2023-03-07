// using Godot;
// using System;

// public class UIInfoCard : Container
// {
// 	[Export]
// 	public bool focus = false;
// 	bool defaultVisibile;
// 	float count = 0f;
// 	static readonly float timeToHide = 1f;
// 	//public Global global;
	
// 	public override void _Ready(){
// 		defaultVisibile = Visible;
// 		//global = (Global)GetNode("/root/Global");
// 		Connect("mouse_entered", this, "Focus");
// 		Connect("mouse_exited", this, "UnFocus");
// 	}
	
// 	private bool VisibleChildren(){
// 		foreach (Control child in GetChildren()){
// 			if (child.Visible){
// 				return true;
// 			}
// 		}
// 		return false;
// 	}
	
// 	public void Focus(){
// 		// When mouse over this element.

// 		//GD.Print($"{GetPath()} Focus");
// 		if (VisibleChildren()){
// 			focus = true;
// 			Visible = true;
// 		}
// 	}

// 	public void UnFocus(){
// 		//GD.Print("UnFocus");
// 		focus = false;
// 		count = timeToHide;
// 	}
	
// 	// TODO replace with co-routine
// 	public override void _Process(float delta){

// 		if ( ( ! defaultVisibile ) && Visible && ! focus ){
// 			count-=delta;
// 			if (count < 0){
// 				Visible = false;
// 			}
// 		}
// 	}
// }
