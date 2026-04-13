using Godot;
using System;
using Miscast;

public partial class InteractableObject : Node2D, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    [Export] private Sprite2D sprite;
    [Export] private AnimatedSprite2D hoverSprite;
    [Export] private string tooltipText = "Interact";

    // --- Variables ---

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "InteractableObject Ready");
        HideHoverSprite();
    }

    #endregion

    #region Methods

    public void Hovered()
    {
        DebugManager.DebugPrint(this, "Hovered");
        ShowHoverSprite();
        Tooltip.Instance.ShowTooltip(tooltipText);
    }

    public void UnHovered()
    {
        DebugManager.DebugPrint(this, "UnHovered");
        HideHoverSprite();
        Tooltip.Instance.HideTooltip();
    }

    public void OnAreaInputEvent(Node viewport, InputEvent inputEvent, long shapeIndex)
    {
        if (inputEvent is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
            {
                OnLeftClick();
            }
        }
    }

    private void OnLeftClick()
    {
        DebugManager.DebugPrint(this, "OnLeftClick");
    }

    private void ShowHoverSprite()
    {
        hoverSprite.Visible = true;
        hoverSprite.Play();
    }

    private void HideHoverSprite()
    {
        hoverSprite.Visible = false;
        hoverSprite.Stop();
    }

    #endregion
}
