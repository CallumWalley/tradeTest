using Godot;
using System;

public class Global : Node
{
	[Signal]
	public delegate void EFrame();

	bool paused = false;

	public float timePerEframe = 1;

	float timeLeft;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timeLeft = timePerEframe;
		_EconomyFrame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(float delta)
	{
		if (! paused){
			timeLeft -= delta;
			if (timeLeft <= 0){
				_EconomyFrame();
				timeLeft += timePerEframe;
			}
		}
		PrintStrayNodes();
	}
	public void PauseToggled(bool value){
		paused = value;
		GD.Print($"Paused is {paused}");
	}
	public void _EconomyFrame()
	{
		EmitSignal("EFrame");
	}
	public void _time_slider_value_changed(float value){
		float[] timescale = { 30, 20, 15, 10, 8, 6, 4, 2, 1, 0.5f, 0.1f };
		float newTimePerEframe = timescale[(int)value];
		timeLeft = (timeLeft/timePerEframe) * newTimePerEframe;
		timePerEframe = newTimePerEframe;
		//GD.Print(timePerEframe);
	}
}
