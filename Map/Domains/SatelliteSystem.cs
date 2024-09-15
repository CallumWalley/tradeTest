using Godot;
using System;

public partial class SatelliteSystem : Domain
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
