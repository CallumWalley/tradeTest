using Godot;
using System;
using System.Collections.Generic;


[Tool]
public partial class Body : Features.FeatureBase
{

    UITabContainerPool uiBody;
    bool focus = false;

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
    double inclination;
    [ExportGroup("Physical")]
    [Export]
    double equatorialRadius = 6.378; //MM


    // Dictionary<string, double> surfaceArea;
    // double mass;
    // double meanDensity;
    // double escapeVelocity;
    // double rotationPeriod;
    // double axialTilt;
    // double albedo;
    // double[] surfaceTemp;
    // double circumference
    // {
    //     get { return equatorialRadius * 2 * Math.PI; }
    // }
    // bool focus = false;

    // public class Designations{
    // 	string name;
    // 	List<String> altNames;
    // 	string adjective;
    // }

    // public class Orbital{
    // 	double aphelion;
    // 	double perihelion;
    // 	double semiMajorAxis;
    // 	double eccentricity;
    // 	double period;
    // 	double inclination;
    // }


    // // Satellites:
    // public class Physical{
    // 	double circumference;
    // 	Dictionary<string, double> surfaceArea;
    // 	double mass;
    // 	double meanDensity;
    // 	double escapeVelocity;
    // 	double rotationPeriod;
    // 	double axialTilt;
    // 	double albedo;
    // 	double[] surfaceTemp;
    // }
    [ExportGroup("Atmosphere")]
    [Export]
    double surfacePressure;
    [Export]
    Color color;
    // [Export]
}
