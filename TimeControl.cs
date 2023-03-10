using Godot;
using System;

public class TimeControl : Container
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
        global.Connect("EFrame_Collect",this, "_EFrameCollect");
        //Connect("Pause_toggled", global, "_EFrameCollect");

    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        frameCount+=1;

        labelWalltime.Text = $"Walltime: {(Time.GetTicksMsec()-startTime).ToString()}";
        labelEframes.Text = $"Economy Frames: {eframeCount.ToString()}";
        labelFrames.Text = $"Frames: {frameCount.ToString()}";
        labelEFramePeriod.Text = $"Seconds Between Economy Frames: {global.timePerEframe.ToString()}";
    }

    void _EFrameCollect(){
        eframeCount+=1;
        PrintStrayNodes();

    }
}
