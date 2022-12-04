using Morkwa.MainData;
using System.Collections.Generic;
using UnityEngine;

namespace Morkwa.Interface
{
    public interface ISpawning
    {
        void SetSpawningConfiguration(IGame game, GameFieldConfig config);
        IReadOnlyList<Vector3> GetWallsList();
    }
}