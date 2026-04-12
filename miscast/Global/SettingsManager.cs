using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Miscast;

public partial class SettingsManager : Singleton<SettingsManager>, IDebuggable, IGameData
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    // --- Variables ---
    public enum Resolution
    {
        R640x360, R1280x720, R1920x1080
    }

    public enum WindowMode
    {
        ExclusiveFullscreen, BorderlessFullscreen, Windowed
    }

    public Resolution resolution = Resolution.R1920x1080;
    public WindowMode windowMode = WindowMode.ExclusiveFullscreen;

    #endregion

    #region Events

    #endregion

    #region Inherited Methods

    #endregion

    #region Methods

    public void ChangeResolution(Resolution resolution)
    {
        this.resolution = resolution;

        switch (resolution)
        {
            case Resolution.R640x360:
                DisplayServer.WindowSetSize(new Vector2I(640, 360));
                break;
            case Resolution.R1280x720:
                DisplayServer.WindowSetSize(new Vector2I(1280, 720));
                break;
            case Resolution.R1920x1080:
                DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
                break;
        }
    }

    public void ChangeResolution(int index)
    {
        switch (index)
        {
            case 0:
                resolution = Resolution.R640x360;
                break;
            case 1:
                resolution = Resolution.R1280x720;
                break;
            case 2:
                resolution = Resolution.R1920x1080;
                break;
        }

        ChangeResolution(resolution);
    }

    public void ChangeWindowMode(WindowMode windowMode)
    {
        this.windowMode = windowMode;

        switch (windowMode)
        {
            case WindowMode.ExclusiveFullscreen:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                ApplyResolution();
                break;
            case WindowMode.BorderlessFullscreen:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
                ApplyResolution();
                break;
            case WindowMode.Windowed:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                ApplyResolution();
                break;
        }
    }

    public async void ApplyResolution()
    {
        await ToSignal(GetTree(), "process_frame");
        ChangeResolution(resolution);
    }

    public void ChangeWindowMode(int index)
    {
        switch (index)
        {
            case 0:
                windowMode = WindowMode.ExclusiveFullscreen;
                break;
            case 1:
                windowMode = WindowMode.BorderlessFullscreen;
                break;
            case 2:
                windowMode = WindowMode.Windowed;
                break;
        }

        ChangeWindowMode(windowMode);
    }

    public int GetResolutionIndex()
    {
        switch (resolution)
        {
            case Resolution.R640x360:
                return 0;
            case Resolution.R1280x720:
                return 1;
            case Resolution.R1920x1080:
                return 2;
            default:
                return -1;
        }
    }

    public int GetWindowModeIndex()
    {
        switch (windowMode)
        {
            case WindowMode.ExclusiveFullscreen:
                return 0;
            case WindowMode.BorderlessFullscreen:
                return 1;
            case WindowMode.Windowed:
                return 2;
            default:
                return -1;
        }
    }

    #endregion

    #region IGameData
    public string gameDataName => "Settings";

    public void SaveGameData()
    {
        List<object> gameData = new List<object>();
        {
            gameData.Add(resolution);
            gameData.Add(windowMode);
        }

        GameDataManager.Instance.SaveGameData(this, gameData);
    }

    public void LoadGameData()
    {
        List<object> loadedGameData = GameDataManager.Instance.LoadGameData(this);

        resolution = (Resolution)loadedGameData[0];
        windowMode = (WindowMode)loadedGameData[1];
    }
    #endregion
}
