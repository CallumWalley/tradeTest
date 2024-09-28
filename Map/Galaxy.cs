using Godot;
using System;
using System.Collections.Generic;


public partial class Galaxy : Node, Entities.IEntityable, IEnumerable<PlanetarySystem>
{
	new public string Name { get { return base.Name; } set { base.Name = value; } }
	public string Description { get; set; }
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	public IEnumerator<PlanetarySystem> GetEnumerator()
	{
		foreach (Node c in GetChildren())
		{
			if (c.GetType() == typeof(PlanetarySystem))
			{
				yield return (PlanetarySystem)c;
			}
		}
	}
	public Godot.Vector2 CameraPosition{ get{ throw new NotImplementedException(); }}

    public float CameraZoom{ get{ throw new NotImplementedException(); }}
}
