using Godot;
using System;
using System.Collections.Generic;
using Miscast;

public partial class SettingsScreen : Control, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;
    [Export] public OptionButton windowModeButton;
    [Export] public OptionButton resolutionButton;
    [Export] public Slider masterVolumeSlider;
    [Export] public Label masterVolumeLabel;
    [Export] public Slider musicVolumeSlider;
    [Export] public Label musicVolumeLabel;
    [Export] public Slider sfxVolumeSlider;
    [Export] public Label sfxVolumeLabel;

    // --- Variables ---

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "SettingsScreen Ready");
        LoadSettings();
    }

    #endregion

    #region Methods

    public void LoadSettings()
    {
        resolutionButton.Selected = SettingsManager.Instance.GetResolutionIndex();
        windowModeButton.Selected = SettingsManager.Instance.GetWindowModeIndex();

        masterVolumeSlider.Value = AudioManager.Instance.masterVolume;
        masterVolumeLabel.Text = AudioManager.Instance.masterVolume + "%";

        musicVolumeSlider.Value = AudioManager.Instance.musicVolume;
        musicVolumeLabel.Text = AudioManager.Instance.musicVolume + "%";

        sfxVolumeSlider.Value = AudioManager.Instance.sfxVolume;
        sfxVolumeLabel.Text = AudioManager.Instance.sfxVolume + "%";
    }

    public void MasterVolumeChanged(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
        masterVolumeLabel.Text = AudioManager.Instance.masterVolume + "%";
    }

    public void MusicVolumeChanged(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
        musicVolumeLabel.Text = AudioManager.Instance.musicVolume + "%";
    }

    public void SFXVolumeChanged(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
        sfxVolumeLabel.Text = AudioManager.Instance.sfxVolume + "%";
    }

    public void ResolutionChanged(int index)
    {
        SettingsManager.Instance.ChangeResolution(index);
    }

    public void WindowModeChanged(int index)
    {
        SettingsManager.Instance.ChangeWindowMode(index);

        if (SettingsManager.Instance.windowMode == SettingsManager.WindowMode.BorderlessFullscreen || SettingsManager.Instance.windowMode == SettingsManager.WindowMode.ExclusiveFullscreen)
        {
            resolutionButton.Disabled = true;
        }

        else
        {
            resolutionButton.Disabled = false;
        }
    }

    #endregion
}
