using Godot;
using System;
using Miscast;

public partial class InteractableObject : Container, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    [Export] private Control hoverLabel;
    [Export] private Control tempDialogueBox;

    // --- Variables ---

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "InteractableObject Ready");
        tempDialogueBox.Visible = false;
    }

    #endregion

    #region Methods

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
        DebugManager.DebugPrint(this, "OnLeftClick");
        tempDialogueBox.Visible = !tempDialogueBox.Visible;
    }

    private void OnMouseEnter()
    {
        DebugManager.DebugPrint(this, "OnMouseEnter");
        hoverLabel.Show();
        Modulate = new Color(1.2f, 1.2f, 1.2f);
    }

    private void OnMouseExit()
    {
        DebugManager.DebugPrint(this, "OnMouseExit");
        hoverLabel.Hide();
        Modulate = new Color(1, 1, 1);
    }

    #endregion
}
