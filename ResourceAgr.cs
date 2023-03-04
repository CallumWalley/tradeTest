using Godot;
using System;

public class ResourceAgr: Resource
{	
	[Export]
	public override int Type{get; set;}
	public override float Sum{get{return value;}}
	
	public float value=0;
	public Godot.Collections.Array<Resource> _add = new Godot.Collections.Array<Resource>();
	public Godot.Collections.Array<Resource> _sub = new Godot.Collections.Array<Resource>();
	public Godot.Collections.Array<Resource> _multi = new Godot.Collections.Array<Resource>();


	public override void EconomyFrame(){
		float add=0;
		float multi=1;
		foreach (Resource i in _add){
			add+=i.Sum;
		}
		foreach (Resource i in _sub){
			add-=i.Sum;
		}
		foreach (Resource i in _add){
			multi+=i.Sum;
		}
		value = add * multi;
	}

    
}
