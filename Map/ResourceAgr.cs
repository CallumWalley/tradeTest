using Godot;
using System;
using System.Collections.Generic;

public class ResourceAgr: Resource
{	
	
	public List<Resource> add = new List<Resource>();
	public List<Resource> multi =  new List<Resource>();
	public ResourceAgr(int _type, List<Resource> _add, string _details="Sum"){
		Type = _type;
		add = _add;
		Details = _details;
	}
	
	[Export]
	public override int Type{get; set;}
	public override float Sum{get{return _Sum();}}
	
	public float value = 0;

	public void Clear(){
		add.Clear();
		multi.Clear();
	}

	//If can be treated like a static 
	public bool IsSingle(){
		return ((add.Count + multi.Count)<2);
	}

	public float _Sum(){
		float addCum=0;
		float multiCum=1;
		foreach (Resource i in add){
			addCum+=i.Sum;
		}
		foreach (Resource i in multi){
			multiCum+=i.Sum;
		}
		return addCum * multiCum;
	}	
}
