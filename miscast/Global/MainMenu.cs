using Godot;
using System;
using Miscast;

public partial class MainMenu : Node, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    [Export] public Control mainScreen;
    [Export] public Control settingsScreen;

    // --- Variables ---
    private Control currentScreen;

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "MainMenu Ready");
        GameManager.Instance.Reset();
    }

    // ----- Update Methods -----
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Return") && currentScreen != mainScreen)
        {
            MainScreen();
        }
    }

    #endregion

    #region Methods

    public void SettingsScreen()
    {
        DebugManager.DebugPrint(this, "Settings Screen");
        currentScreen = settingsScreen;
        settingsScreen.Show();
        mainScreen.Hide();
    }

    public void MainScreen()
    {
        DebugManager.DebugPrint(this, "Main Menu Screen");
        currentScreen = mainScreen;
        mainScreen.Show();
        settingsScreen.Hide();
    }

    public void BlueskyLink()
    {
        OS.ShellOpen("https://bsky.app/profile/nikomori2901.bsky.social");
    }

    public void YoutubeLink()
    {
        OS.ShellOpen("https://www.youtube.com/@NikomoriKiba/featured");
    }

    public void ItchLink()
    {
        OS.ShellOpen("https://nikomori29.itch.io/");
    }

    public void ExitButton()
    {
        GetTree().Quit();
    }

    #endregion
}
