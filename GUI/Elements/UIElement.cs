using Godot;
using System;

//Base class for informational element.
public partial class UIElement : Control
{
    protected Logger logger;
    protected bool mouseOver;
    bool active = false;
    double count = 0f;

    // 


    // How long to hover before showing details.

    public Action ShowDetailsCallback;
    public Action HideDetailsCallback;


    static readonly double showPeriod = 0.6f;

    // How long before hiding details after mouse leave.
    static readonly double hidePeriod = 0.6f;

    public override void _Ready()
    {
        base._Ready();
        count = showPeriod;
        logger = new Logger(this);

        // Should get this from inheritence, this should be temp
        GetNode<Global>("/root/Global").Connect("EFrame", new Callable(this, "QueueRedrawWrap"));

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
    //Dunno why I cant call directly.
    void QueueRedrawWrap()
    {
        QueueRedraw();
        //GD.Print("QR");
    }
    // TODO replace with co-routine?
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Visible)
        {
            if (active != mouseOver)
            {
                count -= delta;
            }

            if (count < 0)
            {
                if (active)
                {
                    active = false;
                    count = showPeriod;
                    HideDetailsCallback?.Invoke();
                }
                else
                {
                    active = true;
                    count = hidePeriod;
                    ShowDetailsCallback?.Invoke();
                }
            }
        }


    }

}