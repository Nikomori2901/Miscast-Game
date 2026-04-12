using Godot;
using System;
using Miscast;

public partial class NodeConnection : AnimatedSprite2D, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    [Export] public GridPosition fromNode;
    [Export] public GridPosition toNode;
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

    public void ShowConnection(Vector2I fromPosition)
    {
        DebugManager.DebugPrint(this, $"ShowConnection {fromPosition} -> {(fromPosition == nodePosition1 ? nodePosition2 : nodePosition1)}");

        bool reversed = fromPosition == nodePosition2;
        if (reversed)
        {
            Vector2I direction = nodePosition2 - nodePosition1;
            float rotationDeg = Mathf.Abs(Mathf.Round(RotationDegrees)) % 360;
            bool rotated = rotationDeg == 90 || rotationDeg == 270;

            if (rotated)
            {
                FlipH = direction.Y != 0;
                FlipV = direction.X != 0;
            }

            else
            {
                FlipH = direction.X != 0;
                FlipV = direction.Y != 0;
            }
        }

        Visible = true;
        Play();
    }

    public void HideConnection()
    {
        DebugManager.DebugPrint(this, $"HideConnection {nodePosition1} - {nodePosition2}");
        FlipH = false;
        FlipV = false;
        Visible = false;
        Stop();
    }

    #endregion
}
