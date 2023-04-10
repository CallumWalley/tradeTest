using Godot;
using System;

public class UIResource : UIElement
{
	public Resource resource;
	// Prefabs
	static readonly PackedScene p_UIResourceList = GD.Load<PackedScene>("res://GUI/Components/UIResourceList.tscn");
	static readonly PackedScene p_UIResourceBreakdown = GD.Load<PackedScene>("res://GUI/Components/UIResourceBreakdown.tscn");

	// Child components
   	protected Control details;
	protected Label value;

	public void Init(Resource _resource)
	{	
		resource = _resource;
		if (resource != null)
		{
			((TextureRect)GetNode("Icon")).Texture = Resources.Icon(resourceCode: resource.Type);
		}
		else
		{
			logger.warning("UI made without object");
		}
	}

	public override void _Ready(){	
		base._Ready();

		// Assign children
		value = GetNode<Label>("Value");
		// details assigned on first call of ShowDetails
	}

	void CreateDetails(){
		details = p_UIResourceBreakdown.Instance<UIResourceBreakdown>();
		((UIResourceBreakdown)details).Init(resource);
		AddChild(details);
		// Add details panel
		// if (resource is ResourceAgr){
		// 	UIResourceList rl = p_UIResourceList.Instance<UIResourceList>();
		// 	rl.Init(((ResourceAgr)resource).add);
		// 	details.GetNode<Control>("PanelContainer").AddChild(rl);
		// }else{
		// 	Label label = new Label();
		// 	label.Text="Details"; //=resource.Name;
		// 	details.GetNode<Control>("PanelContainer").AddChild(label);
		// }
	}

	protected override void ShowDetails()
	{	
		// Create details panel if first time.
		if (details is null){
			CreateDetails();
		}
		base.ShowDetails();
		details.RectGlobalPosition = RectGlobalPosition + new Vector2(2, 0);
		details.Show();
	}

	protected override void HideDetails()
	{
		base.HideDetails();
		details.Hide();
	}
	
	public override void _Draw()
	{
		if (resource != null)
		{	
			value.Text = (resource.Sum).ToString();
		}
		else
		{
			logger.warning("UI made without object");
		}
	}
}
