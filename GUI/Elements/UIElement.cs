using Godot;
using System;

//Base class for informational element.
public partial class UIElement : Control
{
    protected Logger logger;
    protected bool mouseOver;
    bool showDetails = false;
    double count = 0f;

    // How long to hover before showing details.

    static readonly double showPeriod = 0.6f;

    // How long before hiding details after mouse leave.
    static readonly double hidePeriod = 0.6f;


    public override void _Ready()
    {
        base._Ready();
        count = showPeriod;
        logger = new Logger(this);
        GD.Print("new object.");

        // // defaultVisibile = Visible;
        Connect("mouse_entered", new Callable(this, "MouseEnter"));
        Connect("mouse_exited", new Callable(this, "MouseExit"));
    }

    void MouseEnter()
    {
        mouseOver = true;

    }
    void MouseExit()
    {
        mouseOver = false;
    }
    protected virtual void ShowDetails()
    {
        showDetails = true;
    }
    protected virtual void HideDetails()
    {
        showDetails = false;
    }


    public override void _Draw()
    {
        base._Draw();
        // If parent not visible, hide.
        Control p = GetParentOrNull<Control>();
        if (!p.Visible)
        {
            Visible = false;
        }
    }

    // TODO replace with co-routine?
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Visible)
        {
            if (showDetails != mouseOver)
            {
                count -= delta;
            }

            if (count < 0)
            {
                if (showDetails)
                {
                    HideDetails();
                    count = showPeriod;

                }
                else
                {
                    ShowDetails();
                    count = hidePeriod;

                }
            }
        }


    }

}