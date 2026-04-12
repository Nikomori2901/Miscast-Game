using Godot;
using System;
using Miscast;

public partial class PauseMenu : Panel, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    [Export] private Control settingsScreen;

    // --- Variables ---
    private bool paused = false;

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "PauseMenu Ready");
        paused = false;
    }

    // ----- Update Methods -----
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Pause"))
        {
            if (paused)
            {
                StopPause();
                DebugManager.DebugPrint(this, "Unpause");
            }

            else
            {
                StartPause();
                DebugManager.DebugPrint(this, "Pause");
            }
        }
    }

    #endregion

    #region Methods

    public void StartPause()
    {
        paused = true;
        GetTree().Paused = true;
        Visible = true;
    }

    public void StopPause()
    {
        paused = false;
        GetTree().Paused = false;
        Visible = false;
    }

    public void ResumePress()
    {
        DebugManager.DebugPrint(this, "Resume Button Press");
        StopPause();
    }

    public void MainMenuPress()
    {
        DebugManager.DebugPrint(this, "Main Menu Button Press");
        StopPause();
        SceneManager.Instance.LoadScene("res://Scenes/main_menu.tscn");
    }

    public void SettingsPress()
    {
        DebugManager.DebugPrint(this, "Settings Button Press");
        settingsScreen.Visible = true;
        Visible = false;
    }

    public void SettingsBackPress()
    {
        DebugManager.DebugPrint(this, "Settings Back Button Press");
        settingsScreen.Visible = false;
        Visible = true;
    }

    #endregion
}
