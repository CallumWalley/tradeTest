using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class PlayerConfig : Node
{
    string confFilePath = "user://config.cfg";

    public static ConfigFile config = new ConfigFile();


    public override void _EnterTree()
    {
        base._EnterTree();
        LoadConfigFile();

    }
    public override void _Ready()
    {
        base._Ready();
    }

    public void LoadConfigFile()
    {
        string localpath = Godot.ProjectSettings.GlobalizePath(confFilePath);
        Error e = config.Load(confFilePath);
        if (e != Error.Ok)
        {
            GD.Print($"Writing default config file to {localpath}");
            config.SetValue("interface", "stepNumericalInterpolation", true);
            config.SetValue("interface", "cameraPanSpeed", 2);
            config.SetValue("interface", "cameraZoomSpeed", 2);
            config.SetValue("interface", "linearLogBase", 2); // 0 for none.
            config.SetValue("interface", "radialScale", 1000);
            config.SetValue("interface", "radialLogBase", 2); // 0 for none.

            config.Save(confFilePath);
        }
        else
        {
            GD.Print($"Loading existing config from {localpath}");
        }
        GD.Print(what: config.EncodeToText());
    }

}