using Godot;
using System;

public class Global : Node
{
	[Signal]
<<<<<<< HEAD
	public delegate void EFrame_Collect();
	[Signal]

	public delegate void EFrame_Move();
	[Signal]

	public delegate void EFrame_Produce();
=======
	public delegate void EFrame();
>>>>>>> 5259abcdd5f490e80e5a7c118d5f60b37c957db6

	bool paused = false;

	public float timePerEframe = 1;

	float timeLeft;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timeLeft = timePerEframe;
<<<<<<< HEAD
		Eframe();
=======
		_EconomyFrame();
>>>>>>> 5259abcdd5f490e80e5a7c118d5f60b37c957db6
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(float delta)
	{
		if (! paused){
			timeLeft -= delta;
			if (timeLeft <= 0){
<<<<<<< HEAD
				Eframe();
=======
				_EconomyFrame();
>>>>>>> 5259abcdd5f490e80e5a7c118d5f60b37c957db6
				timeLeft += timePerEframe;
			}
		}
		PrintStrayNodes();
	}
	public void PauseToggled(bool value){
		paused = value;
		GD.Print($"Paused is {paused}");
	}
<<<<<<< HEAD
	public void Eframe()
	{
		EmitSignal("EFrame_Collect");
		EmitSignal("EFrame_Move");
		EmitSignal("EFrame_Produce");
=======
	public void _EconomyFrame()
	{
		EmitSignal("EFrame");
>>>>>>> 5259abcdd5f490e80e5a7c118d5f60b37c957db6
	}
	public void _time_slider_value_changed(float value){
		float[] timescale = { 30, 20, 15, 10, 8, 6, 4, 2, 1, 0.5f, 0.1f };
		float newTimePerEframe = timescale[(int)value];
		timeLeft = (timeLeft/timePerEframe) * newTimePerEframe;
		timePerEframe = newTimePerEframe;
		//GD.Print(timePerEframe);
	}
}
