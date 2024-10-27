using Godot;
using System;
using System.Linq;
namespace Game;

public partial class UIWindowDomainFeaturePlan : UIWindow
{
	[Export]
	public Domain domain;

	[Export]
	public UIPanelPlayerFeatureTemplateList templateList;

	[Export]
	public Button addButton;

	[Export]
	public SpinBox scaleSpinbox;

	[Export]
	public LineEdit nameLineEdit;

	public override void _Ready()
	{
		base._Ready();
		addButton.Connect("pressed", new Callable(this, "OnButtonPressed"));
		nameLineEdit.Connect("text_changed", new Callable(this, "OnLineEditTextChanged"));
		templateList.list.Connect("item_selected", new Callable(this, "OnTemplateListItemSelected"));
		scaleSpinbox.Value = 1;
	}

	public void Init()
	{
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