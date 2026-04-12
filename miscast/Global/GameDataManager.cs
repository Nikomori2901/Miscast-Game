using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot.Collections;
using Miscast;

public partial class GameDataManager : Singleton<GameDataManager>, IDebuggable
{
    #region Variables
    // --- Export Variables ---
    [Export] public bool debugEnabled { get; set; } = false;

    // --- Variables ---
    private static string gameDataDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(OS.GetExecutablePath()), "Game Data");

    #endregion

    #region Events

    #endregion

    #region Inherited Methods
    // ----- Lifecycle Methods -----
    public override void _Ready()
    {
        DebugManager.DebugPrint(this, "GameDataManager Ready");
        base._Ready();
        LoadAllGameData();
    }

    public override void _ExitTree()
    {
        DebugManager.DebugPrint(this, "GameDataManager ExitTree");
        SaveAllGameData();
    }

    #endregion

    #region Methods

    public void SaveGameData(IGameData gameDataNode, List<object> gameData)
    {
        string gameDataPath = System.IO.Path.Combine(gameDataDirectory, $"{gameDataNode.gameDataName}.json");
        DirAccess.MakeDirRecursiveAbsolute(gameDataDirectory);

        string json = JsonSerializer.Serialize(gameData, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        using FileAccess file = FileAccess.Open(gameDataPath, FileAccess.ModeFlags.Write);
        file.StoreString(json);

        DebugManager.DebugPrint(this, "Data Saved");
    }

    public void SaveAllGameData()
    {

    }

    public List<object> LoadGameData(IGameData gameDataNode)
    {
        string gameDataPath = System.IO.Path.Combine(gameDataDirectory, $"{gameDataNode.gameDataName}.json");

        if (!FileAccess.FileExists(gameDataPath))
        {
            DebugManager.DebugPrint(this, $"{gameDataNode.gameDataName} Missing");
            return null;
        }

        using FileAccess file = FileAccess.Open(gameDataPath, FileAccess.ModeFlags.Read);
        string json = file.GetAsText();

        try
        {
            List<object> gameData = JsonSerializer.Deserialize<List<object>>(json);
            DebugManager.DebugPrint(this, $"{gameDataNode.gameDataName} Loaded");
            return gameData;
        }

        catch (Exception e)
        {
            GD.PrintErr($"{gameDataNode.gameDataName} Load Failed: {e.Message}");
            throw;
        }
    }

    public void LoadAllGameData()
    {

    }

    #endregion
}
