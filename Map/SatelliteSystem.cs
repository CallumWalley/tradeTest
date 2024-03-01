using Godot;
using System;

public partial class SatelliteSystem : ResourcePool
{
	[ExportGroup("Orbital")]
	[Export]
	double aphelion;
	[Export]
	double perihelion;
	[Export]
	double semiMajorAxis;
	[Export]
	double eccentricity;
	[Export]
	double period;
}
