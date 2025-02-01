using Godot;
using System;
namespace Game;
using System.Collections.Generic;

public partial class NearOrbit : Node2D, Entities.IPosition
{
	[Export]
	public Planet Planet;

	[ExportGroup("Economic")]
	[Export]
	public bool HasEconomy { get; set; }

	[Export]
	public string Description { get; set; }

	public Entities.IFeature this[int index]
	{
		get
		{
			return (Entities.IFeature)GetChild(index);
		}
	}
	public List<string> Tags { get; set; } = new List<string> { };


	public Entities.IDomain Domain { get { return (Domain)GetParent(); } }

	public IEnumerable<Entities.IFeature> Features
	{
		get
		{
			foreach (Entities.IFeature f in GetChildren())
			{
				yield return f;
			}
		}
	}
	public float CameraZoom
	{
		get
		{
			return Planet.CameraZoom - 10;
		}
	}
	public Vector2 CameraPosition
	{
		get
		{
			return Planet.GlobalPosition;
		}
	}
	public float SemiMajorAxis { get { return Planet.SemiMajorAxis; } }
	public float Anomaly { get { return Planet.Anomaly; } }
	public float Eccentricity { get { return Planet.Eccentricity; } }
	public float Period { get { return Planet.Period; } }

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	public IEnumerator<Entities.IFeature> GetEnumerator()
	{
		foreach (Entities.IFeature f in Features)
		{
			yield return f;
		}
	}
	public void AddFeature(Entities.IFeature f)
	{
		AddChild((Node)f);
	}
	public void RemoveFeature(Entities.IFeature f)
	{
		RemoveChild((Node)f);
	}

}
