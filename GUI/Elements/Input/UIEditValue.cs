using Godot;
using System;

public class UIEditValue : Label
{

	LineEdit lineEdit;
	
	// if mouuse is over this element
	bool mouseOver = false;
	//Callback functions
	public Func<String, bool> textChanged;
	public Func<String, bool> textEntered;

	// Must be tidyr way.
	bool TextChanged(String str){
		if (textChanged != null){
			return textChanged(str);
		}
		return true;
	}
	bool TextEntered(String str){
		if (textEntered != null){
			return textEntered(str);
		}
		return true;
	}
	public override void _Input(InputEvent @event)
	{   
		//Any event dismisses.
		if (lineEdit.Visible){
		   // Dismiss without callback
		   if (@event.IsActionPressed("ui_cancel")){
				lineEdit.SetDeferred("visible", false);
			}else if (@event.IsActionPressed("ui_accept")){
				lineEdit.SetDeferred("visible", false);
				TextEntered(lineEdit.Text);
			}else if (@event.IsActionPressed("ui_select") && !mouseOver){
				lineEdit.SetDeferred("visible", false);
				TextEntered(lineEdit.Text);
			}
		}else if (@event is InputEventMouseButton){
			if (((InputEventMouseButton)@event).Doubleclick)
			{
				lineEdit.SetDeferred("visible", true);
				lineEdit.Text = Text;
				lineEdit.SelectAll();
				lineEdit.GrabFocus();
			}
		}
	}
	public Action<float> callback;
	public override void _Ready()
	{
		lineEdit = GetNode<LineEdit>("LineEdit");

		lineEdit.Connect("text_changed", this, "TextChanged");
		lineEdit.Connect("text_entered", this, "TextEntered");

		lineEdit.Connect("mouse_entered", this, "MouseEnter");
		lineEdit.Connect("mouse_exited", this, "MouseExit");
	}

	void MouseEnter(){
		mouseOver = true;
	}
	void MouseExit(){
		mouseOver = false;
	}
}
//InputEventMouseButton
