using Godot;
using System;
using System.Collections.Generic;
using Miscast;

public partial class SFXSource : AudioStreamPlayer2D, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;
    [Export] public AudioStream[] audioStreams;
    [Export] public MultipleSFXMode multipleSFXMode;
    [Export] public float volumeDeviation;
    [Export] public float pitchDeviation;
    [Export] public bool cutoff;

    // --- Variables ---
    public enum MultipleSFXMode
    {
        Random, DontRepeat, Sequential
    }

    private List<AudioStream> activeStreams;
    private int streamIndex;
    private float defaultVolume;
    private float defaultPitch;

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "SFXSource Ready");
        activeStreams = new List<AudioStream>(audioStreams);
        streamIndex = 0;
        defaultVolume = VolumeDb;
        defaultPitch = PitchScale;
    }

    #endregion

    #region Methods

    public void PlaySFX()
    {
        DebugManager.DebugPrint(this, "Play SFX");
        Deviate();

        switch (multipleSFXMode)
        {
            case MultipleSFXMode.Random:
                if (audioStreams.Length > 1)
                {
                    Stream = audioStreams[GD.RandRange(0, audioStreams.Length - 1)];
                }

                else
                {
                    Stream = audioStreams[0];
                }
                break;

            case MultipleSFXMode.DontRepeat:
                AudioStream audioStream = activeStreams[GD.RandRange(0, activeStreams.Count - 1)];
                Stream = audioStream;
                activeStreams = new List<AudioStream>(audioStreams);
                activeStreams.Remove(audioStream);
                break;

            case MultipleSFXMode.Sequential:
                Stream = audioStreams[streamIndex];
                streamIndex++;
                if (streamIndex == audioStreams.Length)
                {
                    streamIndex = 0;
                }
                break;
        }

        if (cutoff && Playing)
        {
            Stop();
        }

        Play();
    }

    public void Deviate()
    {
        VolumeDb = (float)GD.RandRange(defaultVolume - volumeDeviation, defaultVolume + volumeDeviation);
        PitchScale = (float)GD.RandRange(defaultPitch - pitchDeviation, defaultPitch + pitchDeviation);
    }

    #endregion
}
