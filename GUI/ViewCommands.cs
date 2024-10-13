using Game;
using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;
using YAT.Classes;
using YAT.Scenes;
using YAT.Enums;
using YAT.Helpers;
namespace YAT.Commands;

public partial class ViewCommands : Node
{
    Camera camera;
    YAT yat;
    public override void _Ready()
    {
        base._Ready();
        camera = GetNode<Camera>("/root/Global/Map/Galaxy/Camera2D");
        Global global = Global.Instance;
        YAT yat = GetNode<YAT>("/root/YAT");

        RegisterCommands();
    }

    private void RegisterCommands()
    {
        Focus.camera = camera;
        RegisteredCommands.AddCommand(typeof(Focus));

    }
    [Command("focus", "Focus", "Move focus to [i]node_path[/i] ", new string[] { "view", "cv" })]
    [Argument(
    "node_path",
    "string",
    "The node path of the new selected node")]
    public sealed class Focus : ICommand
    {
        public static Camera camera;
        public CommandResult Execute(CommandData data)
        {
            Cn cn = new Cn();
            CommandResult cr = cn.Execute(data);
            if (cr.Result != 0)
            {
                return ICommand.Failure(string.Format("{0} does not exist.", data.Arguments["node_path"]));
            }

            if (data.Terminal.SelectedNode.Current is Entities.IEntityable)
            {
                camera.Center((Entities.IEntityable)data.Terminal.SelectedNode.Current);
                return ICommand.Success();
            }
            else
            {
                return ICommand.Failure($"{data.Terminal.SelectedNode.Current.Name} is not a valid target to view.");
            }
        }
    }
}

