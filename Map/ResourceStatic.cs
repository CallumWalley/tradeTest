using Godot;
using System;
using System.Collections.Generic;

public class ResourceStatic : Resource
{
	public override int Type { get; set; }

	public override float Sum { get; set; }

	public float Requested { get; set; }

	// count is always 1;
	public override int Count { get { return 1; } }

	public override void Clear()
	{
		Sum = 0;
	}

	public ResourceStatic(int _type, float _sum, string _details = "Base value")
	{
		Type = _type;
		Sum = _sum;
		Details = _details;
	}
	// cast static to agr.
	public static implicit operator ResourceAgr(ResourceStatic rs)
	{
		return new ResourceAgr(rs.Type, new List<Resource> { rs });
	}
	// public void Decriment(){
	// 	Sum--;
	// }
	// public void Incriment(){
	// 	Sum++;
	// }
	public void Change(float newValue)
	{
		Sum = newValue;
	}
}
