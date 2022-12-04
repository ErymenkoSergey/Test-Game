using Morkwa.MainData;
using UnityEngine;
using Zenject;

namespace Morkwa.Mechanics.CommonConfigBehaviours
{
    public abstract class CommonConfigBehaviour : MonoBehaviour
    {
        [Inject] protected MainConfigurator MainConfigurator;
    }
}