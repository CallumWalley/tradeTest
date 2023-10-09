using Godot;
using System;

public partial class UITimeControl : Container
{

    //Labels
    Label labelWalltime;
    Label labelEframes;
    Label labelFrames;
    Label labelEFramePeriod;

    Global global;
    static ulong startTime = Time.GetTicksMsec();
    int frameCount = 0;
    int eframeCount = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        labelWalltime = (Label)GetNode("Count/walltime");
        labelEframes = (Label)GetNode("Count/eframes");
        labelFrames = (Label)GetNode("Count/frames");
        labelEFramePeriod = (Label)GetNode("Count/eframePeriod");

        global = (Global)GetNode("/root/Global");
        global.Connect("EFrameEarly", new Callable(this, "EFrameEarly"));
        GetNode<CheckButton>("Control/Pause").Connect("toggled", new Callable(global, "PauseToggled"));
        GetNode<HSlider>("Control/HSlider").Connect("value_changed", new Callable(global, "TimeRateChanged"));


    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        frameCount += 1;

        labelWalltime.Text = $"Walltime: {(Time.GetTicksMsec() - startTime).ToString()}";
        labelEframes.Text = $"Economy Frames: {eframeCount.ToString()}";
        labelFrames.Text = $"Frames: {frameCount.ToString()}";
        if (global.paused)
        {
            labelEFramePeriod.Text = $"Seconds Between Economy Frames: PAUSED";
        }
        else
        {
            labelEFramePeriod.Text = $"Seconds Between Economy Frames: {global.timePerEframe.ToString()}";
        }
    }

    //{ 30, 20, 15, 10, 8, 6, 4, 2, 1, 0.5f, 0.1f }

    void EFrameEarly()
    {
        eframeCount += 1;
        PrintOrphanNodes();
    }
}
