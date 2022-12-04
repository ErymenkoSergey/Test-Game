using Morkwa.MainData;
using UnityEngine;

namespace Morkwa.Interface
{
    public interface IGame
    {
        IPlayer IPlayer { get; }
        void GameOver(bool isWin);
        IAudio GetAudioManager();
        Transform GetObjectAreFollowing();
        bool GetPlayGameStatus();
        ISpawning GetSpawner();
        void SetPlayer(IPlayer player);
        void StartGame();
        EnemyCharacterConfig GetEnemyConfig();
    }
}

public enum Controls
{
    None = 0,
    Shoot = 1,
    Up = 2,
    Down = 3,
    Left = 4,
    Right = 5
}

public enum AIStatus
{
    None = 0,
    Patrol = 1,
    Hunter = 2
}