using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

/// <summary>
/// Window to pop up when planning new feature.
/// </summary>
public partial class UIPanelDomainFeatureTemplate : UIPanel
{
	[Export]
	public Domain domain;

	[Export]
	public Button addButton;

	[Export]
	public SpinBox scaleSpinbox;

	[Export]
	public LineEdit nameLineEdit;

	[Export]
	ItemList itemList;
	Player player;
	static readonly PackedScene prefab_UIPanelFeatureTemplateFull = (PackedScene)GD.Load<PackedScene>("res://GUI/Game/FeatureTemplate/UIPanelFeatureTemplateFull.tscn");

	// Full list of features to display
	public List<PlayerFeatureTemplate> featureList;
	// Where selected element is displayed.
	[Export]
	ScrollContainer display;

	public PlayerFeatureTemplate selected;
	int selectedIndex = 0;

	public override void _Ready()
	{
		base._Ready();
		player = GetNode<Player>("/root/Global/Player");

		itemList.Connect("item_selected", new Callable(this, "OnItemListItemSelected"));
		itemList.Connect("visibility_changed", new Callable(this, "OnItemListVisibilityChanged"));
		addButton.Connect("pressed", new Callable(this, "OnButtonAddFeaturePressed"));
		UpdateElements();
		//OnItemListItemSelected(0);
	}

	public void OnItemListVisibilityChanged()
	{
		UpdateElements();
		DrawDisplay();
	}

	public void OnItemListItemSelected(int i)
	{
		selectedIndex = i;
		// if (selectedIndex >= player.featureTemplates.GetValid(domain).Count) { return; }
		selected = (PlayerFeatureTemplate)featureList[selectedIndex];
		DrawDisplay();
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
	void DrawDisplay()
	{
		// Draws the UI. Does not refresh elements.
		if (featureList.Count < 1) { return; }

		// If selected feature changed, or none.
		if ((display.GetChildCount() < 1) || display.GetChild<UIPanelFeatureTemplateFull>(0).template != selected)
		{
			foreach (Control c in display.GetChildren().Cast<Control>())
			{
				c.Visible = false;
				c.QueueFree();
				display.RemoveChild(c);
			}
			// Change display to selected thing.
			selected ??= (PlayerFeatureTemplate)featureList[0];
			UIPanelFeatureTemplateFull uipff = prefab_UIPanelFeatureTemplateFull.Instantiate<UIPanelFeatureTemplateFull>();
			uipff.template = selected;
			display.AddChild(uipff);
		}
		display.GetChild<UIPanelFeatureTemplateFull>(0).OnEFrameUpdate();
	}
	/// <summary>
	/// Updates list elements. Does not draw them.
	/// </summary>
	public void UpdateElements()
	{
		featureList = player.featureTemplates.GetValid(domain).ToList();

		if (featureList.Count < 1) { return; }

		itemList.Clear();
		foreach (Node f in featureList)
		{
			itemList.AddItem(((PlayerFeatureTemplate)f).Name, ((PlayerFeatureTemplate)f).Feature.iconMedium);
		}
		itemList.Select(selectedIndex);
	}
	public override void OnEFrameUpdate()
	{
		// If visible, and there are features, update the list to reflect reality.
		base.OnEFrameUpdate();
		if (IsVisibleInTree() && featureList.Count > 0)
		{
			UpdateElements();
			DrawDisplay();
		}
	}

	public void OnTemplateListItemSelected(int i)
	{
		nameLineEdit.Text = selected.GenerateName();
	}
	public void OnLineEditTextChanged(string new_text)
	{
		ValidateName(new_text);
	}

	public void ValidateName(string new_text)
	{
		if ((domain == null) || (domain.Count() < 1)) { return; }
		if (domain.Any(x => x.Name == new_text))
		{
			addButton.Disabled = true;
			addButton.TooltipText = "Name must be unique";
		}
		else
		{
			addButton.Disabled = false;
			addButton.TooltipText = null;
		}
	}

	public void OnButtonAddFeaturePressed()
	{
		((FeatureBase)domain.AddFeature(selected, nameLineEdit.Text)).ChangeSize(scaleSpinbox.Value);
		GetWindow().Hide();
	}
}