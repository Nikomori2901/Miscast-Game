using Godot;
using System;
using Miscast;

public partial class MagicNode : Node2D, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;
    [Export] public AnimatedSprite2D magicNodeSprite;
    [Export] public AnimatedSprite2D hoverSprite;
    [Export] public Vector2I gridPosition;

    // --- Variables ---
    private bool active;

    #endregion

    #region Events
    public static event Action<MagicNode> NodeActivatedEvent;

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "MagicNode Ready");
        HideHoverSprite();
        HideSprite();
    }

    #endregion

    #region Methods

    public void Hovered()
    {
        DebugManager.DebugPrint(this, "Hovered");
        ShowHoverSprite();
        MagicCasting.Instance.hoveredNode = this;

        if (MagicCasting.Instance.casting)
        {
            Activate();
        }
    }

    public void UnHovered()
    {
        DebugManager.DebugPrint(this, "UnHovered");
        HideHoverSprite();
        MagicCasting.Instance.hoveredNode = null;
    }

    public void Activate()
    {
        active = true;
        NodeActivatedEvent?.Invoke(this);
    }

    public void Deactivate()
    {
        active = false;
        HideSprite();
    }

    public void HideSprite()
    {
        DebugManager.DebugPrint(this, "HideSprite");
        magicNodeSprite.Visible = false;
        magicNodeSprite.Stop();
    }

    public void ShowSprite()
    {
        DebugManager.DebugPrint(this, "ShowSprite");
        magicNodeSprite.Visible = true;
        magicNodeSprite.Play();
    }

    public void HideHoverSprite()
    {
        DebugManager.DebugPrint(this, "HideHoverSprite");
        hoverSprite.Visible = false;
        hoverSprite.Stop();
    }

    public void ShowHoverSprite()
    {
        DebugManager.DebugPrint(this, "ShowHoverSprite");
        hoverSprite.Visible = true;
        hoverSprite.Play();
    }

    #endregion
}
