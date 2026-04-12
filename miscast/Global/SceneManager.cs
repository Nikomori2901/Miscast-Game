using Godot;
using System;
using Miscast;

public partial class SceneManager : Singleton<SceneManager>, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    // --- Variables ---

    #endregion

    #region Events

    #endregion

    #region Inherited Methods

    #endregion

    #region Methods

    public void LoadScene(string scenePath)
    {
        CallDeferred("DeferredLoadScene", scenePath);
    }

    private void DeferredLoadScene(string scenePath)
    {
        GetTree().ChangeSceneToFile(scenePath);
    }

    #endregion
}
