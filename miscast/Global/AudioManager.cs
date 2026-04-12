using Godot;
using System;
using System.Collections.Generic;
using Miscast;

public partial class AudioManager : Singleton<AudioManager>, IDebuggable, IGameData
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;
    [Export] public float masterVolume;
    [Export] public float musicVolume;
    [Export] public float sfxVolume;
    [Export] public Godot.Collections.Dictionary<string, SFXSource> sfxDictionary;
    [Export] public Godot.Collections.Dictionary<string, AudioStreamPlayer2D> musicDictionary;

    // --- Variables ---

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "AudioManager Ready");
        base._Ready();
    }

    #endregion

    #region Methods

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        AudioServer.SetBusVolumeDb(0, Mathf.LinearToDb(volume / 100));
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        AudioServer.SetBusVolumeDb(1, Mathf.LinearToDb(volume / 100));
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        AudioServer.SetBusVolumeDb(2, Mathf.LinearToDb(volume / 100));
    }

    public void PlayMusic(string musicName)
    {
        musicDictionary[musicName].Play();
    }

    public void StopMusic(string musicPlayer)
    {
        musicDictionary[musicPlayer].Stop();
    }

    public void StopAllMusic()
    {
        foreach (AudioStreamPlayer2D musicPlayer in musicDictionary.Values)
        {
            musicPlayer.Stop();
        }
    }

    public bool MusicIsPlaying()
    {
        foreach (AudioStreamPlayer2D musicPlayer in musicDictionary.Values)
        {
            if (musicPlayer.IsPlaying())
            {
                return true;
            }
        }

        return false;
    }

    public bool MusicIsPlaying(string name)
    {
        return musicDictionary[name].IsPlaying();
    }

    public void PlaySFX(string sfxName)
    {
        sfxDictionary[sfxName].PlaySFX();
    }

    #endregion

    #region IGameData
    public string gameDataName => "AudioSettings";

    public void SaveGameData()
    {
        List<object> gameData = new List<object>();
        {
            gameData.Add(masterVolume);
            gameData.Add(musicVolume);
            gameData.Add(sfxVolume);
        }

        GameDataManager.Instance.SaveGameData(this, gameData);
    }

    public void LoadGameData()
    {
        List<object> loadedGameData = GameDataManager.Instance.LoadGameData(this);

        SetMasterVolume((float)loadedGameData[0]);
        SetMusicVolume((float)loadedGameData[1]);
        SetSFXVolume((float)loadedGameData[2]);
    }
    #endregion
}
