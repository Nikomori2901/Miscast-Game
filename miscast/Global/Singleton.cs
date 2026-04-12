using Godot;
using System;

public abstract partial class Singleton<T> : Node where T : Node
{
    #region Variables
    // --- Variables ---
    public static T Instance { get; private set; }

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }

        else
        {
            QueueFree();
        }
    }

    #endregion
}
