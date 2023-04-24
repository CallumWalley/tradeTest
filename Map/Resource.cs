using Godot;
using System;

public class Resource
{
	public virtual float Sum { get; set; }
	public virtual int Type { get; set; }
	public string Details = "";

	public virtual int Count { get { return 0; } }
	public virtual void Clear() { return; }


	public string Name { get { return Resources.Index(Type).name; } }
	public Texture Icon { get { return Resources.Index(Type).icon; } }
	public float ShipWeight { get { return Resources.Index(Type).shipWeight; } }
	public bool Storable { get { return Resources.Index(Type).storable; } }
}
