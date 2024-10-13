using Godot;
using System;
using System.Collections.Generic;
namespace Game;
public sealed class GameAttributes
{
    /// <summary>
    /// This has an effect on the game, e.g. not view.
    /// </summary>
    public sealed class Command : Attribute
    {
        public Command()
        {
        }
    }
    /// <summary>
    /// Changes the behavior of an agent that can make game commands.
    /// </summary>
    public sealed class Automation : Attribute
    {
        public Automation()
        {
        }
    }
}