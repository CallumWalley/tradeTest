using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

public partial class TradeRoutePlanet : TradeRoute
{

    new Resource.RStatic shipDemand = new Resource.RStatic(812, 0);

    public override void _Ready()
    {
        base._Ready();
    }

}
