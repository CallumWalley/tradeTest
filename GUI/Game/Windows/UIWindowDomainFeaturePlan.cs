using Godot;
using System;
using System.Linq;

public partial class UIWindowDomainFeaturePlan : UIWindow
{
	[Export]
	public Domain resourcePool;

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

		scaleSpinbox.Value = 1;
	}

	public void Init()
	{
		ValidateName(nameLineEdit.Text);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public void OnLineEditTextChanged(string new_text)
	{
		ValidateName(new_text);
	}

	public void ValidateName(string new_text)
	{
		if (resourcePool.Any(x => x.Name == new_text))
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
		FeatureBase newFeature = templateList.selected.Instantiate();
		newFeature.Template = templateList.selected;
		resourcePool.AddFeature(newFeature);

		newFeature.Name = nameLineEdit.Text;
		// If has scale
		if (newFeature.FactorsSingle.ContainsKey(901))
		{
			newFeature.FactorsSingle[901].Sum = (scaleSpinbox.Value);
		}

		//templateList.OnItemListItemSelected(newFeature.GetIndex());
		OnCloseRequested();
	}
}