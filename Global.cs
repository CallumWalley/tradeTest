using Godot;
using System;

public partial class Global : Node
{

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

    [Signal]

    public delegate void SFrameEventHandler();
    public bool paused = false;

    public double timePerEframe = 1;
    public double timePerSframe = 2;



    double deltaEFrame;
    double deltaSFrame;


    public int eframeCount = 0;
    public int sframeCount = 0;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        deltaEFrame = timePerEframe;
        deltaSFrame = timePerSframe;

        EmitSignal(SignalName.EFrameSetup);
        SFrame();
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
    public void SFrame()
    {
        EmitSignal(SignalName.SFrame);


        sframeCount += 1;
    }
    public void TimeRateChanged(int value)
    {
        double[] timescale = { 30, 20, 15, 10, 8, 6, 4, 2, 1, 0.5f, 0.1f };
        double newTimePerEframe = timescale[(int)value];
        double newTimePerSframe = timescale[(int)value];
        deltaSFrame = (deltaSFrame / timePerSframe) * newTimePerSframe;
        deltaEFrame = (deltaEFrame / timePerEframe) * newTimePerEframe;
        timePerSframe = newTimePerSframe;
        timePerEframe = newTimePerEframe;
    }
}
