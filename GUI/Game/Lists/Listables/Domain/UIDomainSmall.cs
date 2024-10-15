using Godot;
using System;
using System.Linq;
using System.Collections;
namespace Game;

public partial class UIDomainSmall : Control, Lists.IListable<Domain>
{
    Domain Domain;
    public Domain GameElement { get { return Domain; } }
    public bool Destroy { get; set; } = false;

    Label labelName;

    public override void _Ready()
    {
        labelName = GetNode<Label>("Name");
    }
    public void Init(Domain _Domain)
    {
        Domain = _Domain;
    }
    public void Update()
    {

    }
    public override void _Draw()
    {
        if (Domain == null) { return; }
        labelName.Text = Domain.Name;
        if (Destroy)
        {
            QueueFree();
        }
    }
}
