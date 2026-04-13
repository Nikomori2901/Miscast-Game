using Godot;
using System;
using Miscast;

public partial class Tooltip : Control, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    [Export] private NinePatchRect tooltipBackground;
    [Export] private Label tooltipLabel;

    // --- Variables ---
    public static Tooltip Instance { get; private set; }

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "Tooltip Ready");
        Instance = this;
        Hide();
    }

    public override void _Process(double delta)
    {
        if (!Visible)
        {
            return;
        }

        GlobalPosition = GetViewport().GetMousePosition();
    }

    #endregion

    #region Methods

    public void ShowTooltip(string text)
    {
        tooltipLabel.Text = text;
        Show();
    }

    public void HideTooltip()
    {
        Hide();
    }

    #endregion
}
