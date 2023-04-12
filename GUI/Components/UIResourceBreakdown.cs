using Godot;
using System;

public class UIResourceBreakdown : UIPopover
{
	Resource resource;

	Tree tree;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		base._Ready();
		tree = new Tree();	
		GetNode("MarginContainer").AddChild(tree);
		Render(resource, null);
	}
	public override void _Draw()
	{
		base._Draw();
		Raise();
	}
	public void Init(Resource _resource)
	{
		resource = _resource;
	}

	void Render(Resource r, TreeItem parent){
		TreeItem ni = tree.CreateItem(parent);
		ni.Collapsed = true;
		ni.SetIcon(0, r.Icon);
		ni.SetText(0, string.Format("{0:F1} - {1}", r.Sum, r.Details));
		
		if (r is ResourceAgr){
			foreach (Resource r2 in ((ResourceAgr)r).add){
				Render(r2, ni);
			}
			foreach (Resource r2 in ((ResourceAgr)r).multi){
				Render(r2, ni);
			}
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}