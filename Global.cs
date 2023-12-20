using Godot;
using System;

public partial class Global : Node
{
    [Signal]
    public delegate void SFrameEventHandler();
    [Signal]
    public delegate void EFrameEarlyEventHandler();

    [Signal]
    public delegate void EFrameLateEventHandler();

    // Called at start of game once.
    // Step for objects that have an init, but have been added in editor.
    [Signal]

    public delegate void EFrameUIEventHandler();

    // Called called after late frame, for UI only.

    [Signal]

    public delegate void EFrameSetupEventHandler();

    public bool paused = false;

    public double timePerEframe = 1;
    public double timePerSFrame = 2;



    double deltaEFrame;
    double deltaSFrame;


    public int eframeCount = 0;
    public int SFrameCount = 0;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        deltaEFrame = timePerEframe;
        deltaSFrame = timePerSFrame;

        EmitSignal(SignalName.EFrameSetup);
        Sframe();
        Eframe();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
    {
        if (!paused)
        {
            deltaEFrame -= delta;
            if (deltaEFrame <= 0)
            {
                Eframe();
                deltaEFrame += timePerEframe;
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
        EmitSignal(SignalName.EFrameEarly);
        EmitSignal(SignalName.EFrameLate);
        EmitSignal(SignalName.EFrameUI);

        eframeCount += 1;
    }
    public void Sframe()
    {
        EmitSignal(SignalName.SFrame);


        SFrameCount += 1;
    }
    public void TimeRateChanged(int value)
    {
        double[] timescale = { 30, 20, 15, 10, 8, 6, 4, 2, 1, 0.5f, 0.1f };
        double newTimePerEframe = timescale[(int)value];
        double newTimePerSFrame = timescale[(int)value];
        deltaSFrame = (deltaSFrame / timePerSFrame) * newTimePerSFrame;
        deltaEFrame = (deltaEFrame / timePerEframe) * newTimePerEframe;
        timePerSFrame = newTimePerSFrame;
        timePerEframe = newTimePerEframe;
    }
}
