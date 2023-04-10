using Godot;
using System;
using System.Collections.Generic;

public class UIBodyCard : UICard
{   
    Body body;
    static readonly PackedScene p_tradePanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Panels/UIResourcesPanel.tscn");
	static readonly PackedScene p_astroPanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Panels/UIAstroPanel.tscn");
	static readonly PackedScene p_industryPanel = (PackedScene)GD.Load<PackedScene>("res://GUI/Panels/UIIndustryPanel.tscn");

    TabContainer tabContainer;

    public void Init(Body _body){
        body = _body;

        tabContainer = GetNode<TabContainer>("TabContainer");
		SetPosition(body.Position);

        foreach (Installation i in body.Installations){
            UIResourcesPanel tp = p_tradePanel.Instance<UIResourcesPanel>();
            tp.Init(i);
            tabContainer.AddChild(tp);
        }

        //Always add astro panel.
		UIAstroPanel ap = p_astroPanel.Instance<UIAstroPanel>();
		ap.Init(body);
		tabContainer.AddChild(ap);

    }
	// public Installation installation;

	// public UIBody uiBody;



	// bool focus = false;

	// public class Designations{
	// 	string name;
	// 	List<String> altNames;
	// 	string adjective;
	// }

	// public class Orbital{
	// 	float aphelion;
	// 	float perihelion;
	// 	float semiMajorAxis;
	// 	float eccentricity;
	// 	float period;
	// 	float inclination;
	// }


	// // Satellites:
	// public class Physical{
	// 	float circumference;
	// 	Dictionary<string, float> surfaceArea;
	// 	float mass;
	// 	float meanDensity;
	// 	float escapeVelocity;
	// 	float rotationPeriod;
	// 	float axialTilt;
	// 	float albedo;
	// 	float[] surfaceTemp;
	// }

	// public class Atmosphere{
	// 	float surfacePressure;
	// 	Dictionary<string, float> composition;
	// }
	// public override void _Ready(){

	// 	/// INSTALLATIONS
	// 	// Add resource pool if exists
	// 	installation = GetNodeOrNull<Installation>("Installation");

	// 	/// UI
	// 	// Add trade panel
	// 	if (installation!=null){
	// 		UIResourcesPanel tp = p_tradePanel.Instance<UIResourcesPanel>();
	// 		UIIndustryPanel ip = p_industryPanel.Instance<UIIndustryPanel>();

	// 		tp.Init(this);
	// 		ip.Init(this);

	// 		uiBody.AddChild(tp);
	// 		uiBody.AddChild(ip);

	// 	}

	// 	// Add astronomical info
	// 	UIAstroPanel ap = p_astroPanel.Instance<UIAstroPanel>();
	// 	ap.Init(this);
	// 	uiBody.AddChild(ap);

	// 	// Add interactive 
	// 	GetNode("Area2D").Connect("mouse_entered", this, "Focus");
	// 	GetNode("Area2D").Connect("mouse_exited", this, "UnFocus");

	// }

	// public override void _Process(float _delta)
	// {
	// 	if (Input.IsActionPressed("ui_select")){
	// 		if (focus){
	// 			uiBody.Visible = true;
	// 			uiBody.Raise();
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
	// 		installation = p_installation.Instance<Installation>();
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
	// 		tradeReceiver = p_tradeReceiver.Instance<TradeReceiver>();
	// 		tradeReceiver.Init(installation);
	// 		AddChild(tradeReceiver);
	// 	}
	// 	return tradeReceiver;
	// }

}
