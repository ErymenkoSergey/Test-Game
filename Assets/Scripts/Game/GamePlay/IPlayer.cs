using UnityEngine;

namespace Morkwa.Interface
{
    public interface IPlayer
    {
        Transform GetTransform();
        IMoveble GetMoveble();
    }
}