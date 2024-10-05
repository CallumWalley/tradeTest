using Godot;
using System;

public partial class UICameraControl : VBoxContainer
{
    //Labels
    Label labelFocus;
    Label labelZoom;
    Label labelPosition;

    Slider sliderRadialLogBase;
    Slider sliderLinearLogBase;

    Slider sliderRadialScale;

    Camera camera;

    Global global;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        camera = (Camera)GetViewport().GetCamera2D();

        labelFocus = (Label)GetNode("Count/Focus");
        labelZoom = (Label)GetNode("Count/Zoom");
        labelPosition = (Label)GetNode("Count/Position");


        sliderRadialLogBase = (Slider)GetNode("Control/RadialLogBase");
        sliderLinearLogBase = (Slider)GetNode("Control/LinearLogBase");

        sliderRadialScale = (Slider)GetNode("Control/RadialScale");

        sliderRadialLogBase.Value = (float)PlayerConfig.config.GetValue("interface", "radialLogBase");
        sliderLinearLogBase.Value = (float)PlayerConfig.config.GetValue("interface", "linearLogBase");
        sliderRadialScale.Value = (float)PlayerConfig.config.GetValue("interface", "radialScale");


        sliderRadialLogBase.Connect("value_changed", Callable.From((float x) => ChangeViewSettings("radialLogBase", x)));
        sliderLinearLogBase.Connect("value_changed", Callable.From((float x) => ChangeViewSettings("linearLogBase", x)));
        sliderRadialScale.Connect("value_changed", Callable.From((float x) => ChangeViewSettings("radialScale", x)));
    }

    public void ChangeViewSettings(string setting, float value)
    {
        PlayerConfig.config.SetValue("interface", setting, value);
        float newVal = (float)PlayerConfig.config.GetValue("interface", key: setting);
        camera.Center();
        GD.Print($"Camera {setting} changed to {newVal}");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        labelFocus.Text = $"Focus: {camera.focus.Name}";
        labelZoom.Text = string.Format("Zoom: {0:N1}", camera.Zoom.X);
        labelPosition.Text = string.Format("Position: {0:G2},\n        : {1:G2}", camera.GlobalPosition.X, camera.GlobalPosition.Y);
    }
}
