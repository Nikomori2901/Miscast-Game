using Godot;
using System;
using Miscast;

public partial class NodeConnection : AnimatedSprite2D, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    [Export] public Vector2I nodePosition1;
    [Export] public Vector2I nodePosition2;

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "NodeConnection Ready");
        Hide();
    }

    #endregion

    #region Methods

    public void ShowConnection()
    {
        DebugManager.DebugPrint(this, $"ShowConnection {nodePosition1} - {nodePosition2}");
        Visible = true;
        Play();
    }

    public void HideConnection()
    {
        DebugManager.DebugPrint(this, $"HideConnection {nodePosition1} - {nodePosition2}");
        Visible = false;
        Stop();
    }

    #endregion
}
