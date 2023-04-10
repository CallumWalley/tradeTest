using Godot;
using System;

public class Resource
{	
	public virtual float Sum{get; set;}
	public virtual int Type{get; set;}
	public string Details = "";

	public string Name{get{return Resources.Index(Type).name;}}
	public Texture Icon{get{return Resources.Index(Type).icon;}}
	public float ShipWeight{get{return Resources.Index(Type).shipWeight;}}
	public bool Storable{get{return Resources.Index(Type).storable;}}
}
