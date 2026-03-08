using Godot;
using System;

public partial class InteractableObject : Container
{
    [Export] private Control hoverLabel;
    [Export] private Control tempDialogueBox;
    
    public override void _Ready()
    {
        base._Ready();
        tempDialogueBox.Visible = false;
    }

    public void OnInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton inputEventMouseButton)
        {
            if (inputEventMouseButton.GetButtonIndex() == MouseButton.Left && inputEventMouseButton.Pressed)
            {
                OnLeftClick();
            }
        }
    }

    public void OnLeftClick()
    {
        GD.Print("OnLeftClick");
        if (tempDialogueBox.Visible)
        {
            tempDialogueBox.Visible = false;
        }

        else
        {
            tempDialogueBox.Visible = true;
        }
    }
    
    private void OnMouseEnter()
    {
        GD.Print("OnMouseEnter");
        hoverLabel.Show();
        Modulate = new Color(1.2f, 1.2f, 1.2f);
    }

    private void OnMouseExit()
    {
        hoverLabel.Hide();
        GD.Print("OnMouseExit");
        Modulate = new Color(1, 1, 1);
    }
}
