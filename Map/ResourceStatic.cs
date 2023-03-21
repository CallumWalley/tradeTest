using Godot;
using System;

public class ResourceStatic : Resource
{	
	public override int Type{ get; set;}

	public override float Sum{ get; set; }

	public float Requested{ get; set; }

	public ResourceStatic(int _type, float _sum){
		Type = _type;
		Sum = _sum;
	}
	
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
