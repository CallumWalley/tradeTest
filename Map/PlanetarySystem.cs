using Godot;
using System;
using System.Collections.Generic;

public partial class PlanetarySystem : Node2D, Entities.IEntityable, IEnumerable<SatelliteSystem>
{
	new public string Name { get { return base.Name; } set { base.Name = value; } }
	public string Description { get; set; }

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	public IEnumerator<SatelliteSystem> GetEnumerator()
	{
		foreach (Node c in GetChildren())
		{
			if (c.GetType() == typeof(SatelliteSystem))
			{
				yield return (SatelliteSystem)c;
			}
		}
	}
}


