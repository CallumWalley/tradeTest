using Godot;
using System;

public partial class Planet : Domain
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
