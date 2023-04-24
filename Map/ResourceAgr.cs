using Godot;
using System;
using System.Collections.Generic;

public class ResourceAgr : Resource
{
	[Export]
	public override int Type { get; set; }
	public override float Sum { get { return _Sum(); } }
	// Does not consider child members.
	public override int Count { get { return (add.Count + multi.Count); } }

	public IEnumerable<Resource> GetAdd { get { return add; } }
	public IEnumerable<Resource> GetMulti { get { return multi; } }


	List<Resource> add = new List<Resource>();
	List<Resource> multi = new List<Resource>();
	public ResourceAgr(int _type, List<Resource> _add, string _details = "Sum")
	{
		Type = _type;
		add = _add;
		Details = _details;
	}
	public ResourceAgr(int _type, string _details = "Sum")
	{
		Type = _type;
		add = new List<Resource>();
		Details = _details;
	}

	public Resource First()
	{
		return add[0];
	}

	public override void Clear()
	{
		add.Clear();
		multi.Clear();
	}

	public static explicit operator ResourceStatic(ResourceAgr ra)
	{
		//throw new
		if (ra.Count != 1) { throw new InvalidCastException("Can only cast single member ResourceAgr to ResourceStatic"); }
		return (ResourceStatic)ra.First();
	}
	public void Add(Resource ra)
	{
		//throw new
		add.Add(ra);
	}
	public void Multiply(Resource rm)
	{
		//throw new
		multi.Add(rm);
	}

	float _Sum()
	{
		float addCum = 0;
		float multiCum = 1;
		foreach (Resource i in add)
		{
			addCum += i.Sum;
		}
		foreach (Resource i in multi)
		{
			multiCum += i.Sum;
		}
		return addCum * multiCum;
	}
}
