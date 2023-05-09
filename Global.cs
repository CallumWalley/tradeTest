using Godot;
using System;

public partial class Global : Node
{

    [Signal]
    public delegate void EFrameEarlyEventHandler();
    [Signal]
    public delegate void EFrameLateEventHandler();

    bool paused = false;

    public double timePerEframe = 1;

    double timeLeft;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        timeLeft = timePerEframe;
        Eframe();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
    {
        if (!paused)
        {
            timeLeft -= delta;
            if (timeLeft <= 0)
            {
                Eframe();
                timeLeft += timePerEframe;
            }
        }
        //PrintStrayNodes();
    }
    public void PauseToggled(bool value)
    {
        paused = value;
        GD.Print($"Paused is {paused}");
    }
    public void Eframe()
    {
        EmitSignal("EFrameLate");
        EmitSignal("EFrameEarly");
    }
    public void _time_slider_value_changed(double value)
    {
        double[] timescale = { 30, 20, 15, 10, 8, 6, 4, 2, 1, 0.5f, 0.1f };
        double newTimePerEframe = timescale[(int)value];
        timeLeft = (timeLeft / timePerEframe) * newTimePerEframe;
        timePerEframe = newTimePerEframe;
        //GD.Print(timePerEframe);
    }
}
