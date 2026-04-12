using Godot;
using System;
using Miscast;

public partial class GameManager : Singleton<GameManager>, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    #endregion

#region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "GameManager Ready");
        base._Ready();
        Reset();
    }

    #endregion

    #region Methods

    public void Reset()
    {

    }

    #endregion
}
