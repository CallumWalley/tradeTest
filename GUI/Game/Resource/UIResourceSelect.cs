using Godot;
using System;
using System.Collections.Generic;
namespace Game;

public partial class UIResourceSelect : GridContainer
{

    [Signal]
    public delegate void OnTogglerEventHandler(int rindex, bool toggled);

    [Signal]
    public delegate void OnClickEventHandler(int rindex);


    public Dictionary<int, TextureButton> Map = new Dictionary<int, TextureButton>();

    /// <summary>
    /// If true, will toggle elements rather than pick one.
    /// </summary>
    public bool Toggle { get; set; } = true;


    // Which items to disable.
    public List<int> isDisabled = null;

    public override void _Ready()
    {
        base._Ready();
        foreach (KeyValuePair<int, Resource.ResourceType> item in Resource.GetRegular())
        {
            TextureButton nb = new TextureButton();
            nb.TextureNormal = item.Value.icon;
            AddChild(nb);
            int index = nb.GetIndex();
            Map[item.Key] = nb;
            if (Toggle)
            {
                nb.ToggleMode = true;
                nb.Connect("toggled", Callable.From((bool x) => OnToggle(item.Key, x)));
            }
            else
            {
                nb.Connect("button_up", Callable.From(() => OnButtonUp(item.Key)));
            }

        }
    }
    public override void _Draw()
    {
        if (isDisabled != null)
        {
            if (Toggle)
            {
                foreach (KeyValuePair<int, TextureButton> item in Map)
                {
                    item.Value.ButtonPressed = isDisabled.Contains(item.Key);
                }
            }
            else
            {
                foreach (KeyValuePair<int, TextureButton> item in Map)
                {
                    item.Value.Disabled = isDisabled.Contains(item.Key);
                }
            }
        }
    }

    public void OnButtonUp(int index)
    {
        EmitSignal(SignalName.OnClick, index);
    }
    public void OnToggle(int index, bool toggled)
    {
        EmitSignal(SignalName.OnToggler, index, toggled);
    }
}
