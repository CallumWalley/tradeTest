using Godot;
using System;

public partial class SubSystem : Zone
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
	[Export]
}
