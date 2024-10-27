using Godot;
using System;
using System.Linq;
namespace Game;

public partial class UIWindowDomainFeaturePlan : UIWindow
{
	[Export]
	public Domain domain;

	[Export]
	public UIPanelFeatureTemplateList templateList;

	[Export]
	public Button addButton;

	[Export]
	public SpinBox scaleSpinbox;

	[Export]
	public LineEdit nameLineEdit;

	Player player;

	public override void _Ready()
	{
		base._Ready();
		// This is bad. Calling up tree.
		domain = GetParent<UIPanelDomainFeatures>().domain;
		player = GetNode<Player>("/root/Global/Player");
		addButton.Connect("pressed", new Callable(this, "OnButtonPressed"));
		nameLineEdit.Connect("text_changed", new Callable(this, "OnLineEditTextChanged"));
		templateList.list.Connect("item_selected", new Callable(this, "OnTemplateListItemSelected"));
		templateList.baseDomain = domain;
		scaleSpinbox.Value = 1;
		ValidateName(nameLineEdit.Text);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public void OnTemplateListItemSelected(int i)
	{
		nameLineEdit.Text = templateList.selected.GenerateName();
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


	public void OnButtonPressed()
	{
		domain.AddFeature(templateList.selected, nameLineEdit.Text, scaleSpinbox.Value);
		//templateList.OnItemListItemSelected(newFeature.GetIndex());
		OnCloseRequested();
	}
}