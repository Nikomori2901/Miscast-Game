using Godot;
using System;
using Miscast;

public partial class DialogueBox : Control, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    #endregion
}
