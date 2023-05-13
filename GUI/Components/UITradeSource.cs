using Godot;
using System;
using System.Linq;
using System.Collections;

public partial class UITradeSource : UIElement
{
    public Installation installation;
    public Installation sourceInstallation;

    // Element in change of this element.
    UITradeSourceSelector driverControl;

    UIInstallationSummary installationSummary;
    UIResource frieghtersAvailable;
    Label labelRight;
    Label labelLeft;
    Line2D line2D;
    Button button;


    // If not active, show button flat, and not clicking.
    public bool active = true;

    public override void _Ready()
    {
        base._Ready();
        button = GetNode<Button>("Button");
        labelRight = button.GetNode<Label>("AlignRight/Label");
        labelLeft = button.GetNode<Label>("AlignLeft/Label");
        frieghtersAvailable = button.GetNode<UIResource>("AlignRight/Available");
        installationSummary = button.GetNode<UIInstallationSummary>("AlignLeft/InstallationSummary");
        line2D = new Line2D();
        AddChild(line2D);
        line2D.Visible = false;

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
            frieghtersAvailable.Init(sourceInstallation.resourceDelta[901]);
            labelLeft.Hide();
        }
        Connect("mouse_entered", new Callable(this, "ShowTradeRoute"));
        Connect("mouse_exited", new Callable(this, "HideTradeRoute"));

        if (driverControl != null)
        {
            button.Pressed += () => { driverControl.SetTradeSource(sourceInstallation); };
            //.Connect("pressed", new Callable(this, "Pressed")); //sourceInstallation
        }
    }
    public void Init(Installation _installation, Installation _sourceInstallation, UITradeSourceSelector _driverControl = null)
    {
        driverControl = _driverControl;
        installation = _installation;
        sourceInstallation = _sourceInstallation;
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

    void ShowTradeRoute()
    {
        line2D.Visible = true;
        if (line2D.Visible && (sourceInstallation != null))
        {
            line2D.Points = new Vector2[] { sourceInstallation.Position - GlobalPosition, installation.Position - GlobalPosition };
            //line2D.GlobalPosition = sourceInstallation.Position;
        }
    }
    void HideTradeRoute()
    {
        line2D.Visible = false;
    }
    void Pressed()
    {
        //EmitSignal(SignalName.SetTradeSource, stateName);
    }
}