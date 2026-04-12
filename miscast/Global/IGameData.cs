using System.Collections.Generic;

namespace Miscast;

public interface IGameData
{
    string gameDataName { get; }
    void SaveGameData();
    void LoadGameData();
}