using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SocialClass : Node
{
	Global global;

	// Parameters
	[ExportGroup("Parameters")]
	[Export]
	public double Population;
	[Export]
	public double Cohesion;
	[Export]
	double GrowthRate;
	[Export]
	double LabourFraction;

	
	public List<Demographic> Demographics;
	public List<MemeValue> Memes;
	public List<Conflict> Conflicts;

	public partial class Demographic{
	}


	public partial class MemeValue{

		Dictionary<string, double> positivePressure = new();
		Dictionary<string, double> negativePressure = new();
	
		double min;
		double max;

		double Mean{get{
			return ( max + min ) / 2;
		}}
		double Range{get { return max-min; }}
		public void Normalise(){
			double mean = Mean;
			min += Math.Sqrt(Math.Pow(0.1*(Mean-min), 2.0) - negativePressure.Sum(x=> x.Value));
			max -= Math.Sqrt(Math.Pow(0.1*(Mean-min), 2.0) + negativePressure.Sum(x=> x.Value));
		}
	}
	public partial class Conflict{
	}
    public override void _Ready()
    {
        base._Ready();

        global = GetNode<Global>("/root/Global");
		global.Connect("SFrame", new Callable(this, "SFrame"));
	}

	public void SFrame(){
		Population += GrowthRate;
	}

}
