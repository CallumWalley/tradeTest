using Godot;
using System;

public class Global : Node
{
	[Signal]
	public delegate void EFrame_Collect();
	[Signal]

	public delegate void EFrame_Move();
	[Signal]

	public delegate void EFrame_Produce();

	bool paused = false;

	public float timePerEframe = 1;

	float timeLeft;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timeLeft = timePerEframe;
		Eframe();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(float delta)
	{
		if (! paused){
			timeLeft -= delta;
			if (timeLeft <= 0){
				Eframe();
				timeLeft += timePerEframe;
			}
		}
		//PrintStrayNodes();
	}
	public void PauseToggled(bool value){
		paused = value;
		GD.Print($"Paused is {paused}");
	}
	public void Eframe()
	{
		EmitSignal("EFrame_Collect");
		EmitSignal("EFrame_Move");
		EmitSignal("EFrame_Produce");
	}
	public void _time_slider_value_changed(float value){
		float[] timescale = { 30, 20, 15, 10, 8, 6, 4, 2, 1, 0.5f, 0.1f };
		float newTimePerEframe = timescale[(int)value];
		timeLeft = (timeLeft/timePerEframe) * newTimePerEframe;
		timePerEframe = newTimePerEframe;
		//GD.Print(timePerEframe);
	}
}
