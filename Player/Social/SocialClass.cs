using Godot;
using System;
using System.Collections.Generic;

public partial class SocialClass : Node
{
	Global global;

	// Parameters
	double Population;
	double Cohesion;
	double GrowthRate;
	double LabourFraction;

	public List<Demographic> Demographics;
	public List<Meme> Memes;
	public List<Conflict> Conflicts;

	public partial class Demographic{
	}
	public partial class Meme{
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
