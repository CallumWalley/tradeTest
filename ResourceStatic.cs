using Godot;
using System;

public class ResourceStatic : Resource
{	
	[Export]
	public override int Type{get; set;}

	[Export]
	public override float Sum{ get; set; }

	public void Decriment(){
		Sum--;
	}
	public void Incriment(){
		Sum++;
	}

	public void Change(float newValue){
		Sum=newValue;
	}
}
