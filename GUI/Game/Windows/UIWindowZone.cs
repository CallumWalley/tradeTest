using Godot;
using System;
using System.Collections.Generic;

public partial class UIWindowZone : UIWindow
{
    [Export]
    public Body body;
    static readonly PackedScene p_tradePanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelInstallation.tscn");
    static readonly PackedScene p_astroPanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/Panels/UIPanelAstro.tscn");
    static readonly PackedScene p_industryPanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Panels/UIIndustryPanel.tscn");



    TabContainer tabContainer;

    public void Init(Body _body)
    {
        body = _body;

        tabContainer = GetNode<TabContainer>("TabContainer");
        Position = (Vector2I)body.Position;



        foreach (Installation i in body.Installations)
        {
            UIPanelInstallation tp = p_tradePanel.Instantiate<UIPanelInstallation>();
            tp.Init(i);
            tabContainer.AddChild(tp);
        }
        //Always add astro panel.
        UIPanelAstro ap = GetNode<UIPanelAstro>("TabContainer/Astronomical");
        ap.Init(body);
        tabContainer.MoveChild(ap, -1);
        tabContainer.CurrentTab = 0;
        tabContainer.QueueRedraw();

    }


    public override void _Ready()
    {
        base._Ready();
        GetNode<Global>("/root/Global").Connect("EFrameUI", callable: new Callable(this, "EFrame"));
    }

    public void EFrame()
    {
        foreach (Control panel in tabContainer.GetChildren())
        {
            panel.QueueRedraw();
        }
    }


    // bool focus = false;

    // public class Designations{
    // 	string name;
    // 	List<String> altNames;
    // 	string adjective;
    // }

    // public class Orbital{
    // 	double aphelion;
    // 	double perihelion;
    // 	double semiMajorAxis;
    // 	double eccentricity;
    // 	double period;
    // 	double inclination;
    // }


    // // Satellites:
    // public class Physical{
    // 	double circumference;
    // 	Dictionary<string, double> surfaceArea;
    // 	double mass;
    // 	double meanDensity;
    // 	double escapeVelocity;
    // 	double rotationPeriod;
    // 	double axialTilt;
    // 	double albedo;
    // 	double[] surfaceTemp;
    // }

    // public class Atmosphere{
    // 	double surfacePressure;
    // 	Dictionary<string, double> composition;
    // }
    // public override void _Ready(){

    // 	/// INSTALLATIONS
    // 	// Add resource pool if exists
    // 	installation = GetNodeOrNull<Installation>("Installation");

    // 	/// UI
    // 	// Add trade panel
    // 	if (installation!=null){
    // 		UIPanelInstallation tp = p_tradePanel.Instantiate<UIPanelInstallation>();
    // 		UIIndustryPanel ip = p_industryPanel.Instantiate<UIIndustryPanel>();

    // 		tp.Init(this);
    // 		ip.Init(this);

    // 		uiBody.AddChild(tp);
    // 		uiBody.AddChild(ip);

    // 	}

    // 	// Add astronomical info
    // 	UIPanelAstro ap = p_astroPanel.Instantiate<UIPanelAstro>();
    // 	ap.Init(this);
    // 	uiBody.AddChild(ap);

    // 	// Add interactive 
    // 	GetNode("Area2D").Connect("mouse_entered", this, "Focus");
    // 	GetNode("Area2D").Connect("mouse_exited", this, "UnFocus");

    // }

    // public override void _Process(double _delta)
    // {
    // 	if (Input.IsActionPressed("ui_select")){
    // 		if (focus){
    // 			uiBody.Visible = true;
    // 			uiBody.MoveToForeground();
    // 		}
    // 	}
    // 	//  && focus)
    // 	// {
    // 	// 	uiBody.Visible = true;
    // 	// }
    // }

    // public void Focus(){
    // 	focus = true;
    // }

    // public void UnFocus(){
    // 	focus = false;
    // }

    // public Installation AddInstallation(){
    // 	installation = GetNodeOrNull<Installation>("Installation");
    // 	if (installation==null){
    // 		installation = p_installation.Instantiate<Installation>();
    // 		AddChild(installation);
    // 	}
    // 	if (hasTradeReceiver){
    // 		installation.isValidTradeReceiver=true;
    // 	}
    // 	return installation;
    // }

    // public TradeReceiver AddTradeReceiver(){
    // 	tradeReceiver = GetNodeOrNull<TradeReceiver>("Installation");
    // 	if (tradeReceiver==null){
    // 		tradeReceiver = p_tradeReceiver.Instantiate<TradeReceiver>();
    // 		tradeReceiver.Init(installation);
    // 		AddChild(tradeReceiver);
    // 	}
    // 	return tradeReceiver;
    // }

}
