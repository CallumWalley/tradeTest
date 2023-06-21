using Godot;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public partial class UITradeSource : UIElement
{
    Installation installation;
    Installation sourceInstallation;

    // Element in change of this element.
    UITradeSourceSelector driverControl;

    UI_Installation_Small installationSummary;
    UIResource frieghtersAvailable;
    Label labelRight;
    Label labelLeft;
    Line2D line2D;
    Button button;


    // If not active, show button flat, and not clicking.
    public bool active = true;
    public void Init(Installation _installation, Installation _sourceInstallation, UITradeSourceSelector _driverControl = null)
    {
        driverControl = _driverControl;
        installation = _installation;
        sourceInstallation = _sourceInstallation;
    }
    public override void _Ready()
    {
        base._Ready();
        button = GetNode<Button>("Button");
        labelRight = button.GetNode<Label>("AlignRight/Label");
        labelLeft = button.GetNode<Label>("AlignLeft/Label");
        frieghtersAvailable = button.GetNode<UIResource>("AlignRight/Available");
        installationSummary = button.GetNode<UI_Installation_Small>("AlignLeft/InstallationSummary");

        // If no destination, hide summary, show no trade route message.
        if (sourceInstallation == null)
        {
            labelRight.Hide();
            installationSummary.Hide();
            frieghtersAvailable.Hide();
        }
        else
        {
            installationSummary.Init(sourceInstallation);
            frieghtersAvailable.Init(sourceInstallation.RDelta[901]);
            labelLeft.Hide();
        }
        Connect("mouse_entered", new Callable(this, "ShowTradeRoute"));
        Connect("mouse_exited", new Callable(this, "HideTradeRoute"));

        if (driverControl != null)
        {
            button.Pressed += () => { driverControl.SetTradeSource(sourceInstallation); };
            button.Pressed += () => { HideTradeRoute(); };
            //.Connect("pressed", new Callable(this, "Pressed")); //sourceInstallation
        }
    }

    public override void _Draw()
    {
        base._Draw();
        if (installation == null) { return; }
        if (labelRight.Visible)
        {
            double dist = installation.Position.DistanceTo(sourceInstallation.Position);
            double time = GetNode<PlayerTech>("/root/Global/Player/Tech").GetFreighterTons(1, dist);
            labelRight.Text = string.Format("{0} - {1}", UnitTypes.DistanceSI(dist), UnitTypes.TimeSol(time));
        }
    }

    public void ShowTradeRoute()
    {
        if (sourceInstallation != null)
        {
            line2D = new Line2D();
            GetNode("/root/Global/UI").AddChild(line2D);
            line2D.Width = 4;
            line2D.Points = new Vector2[] { sourceInstallation.Position, installation.Position };
        }
    }
    public void HideTradeRoute()
    {
        if (line2D != null)
        {
            line2D.QueueFree();
            line2D = null;
        }
    }
    void Pressed()
    {
        //EmitSignal(SignalName.SetTradeSource, stateName);
    }
}