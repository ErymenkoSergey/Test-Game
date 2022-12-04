using Morkwa.MainData;
using UnityEngine;
using Zenject;

namespace Morkwa.DIData
{
    [CreateAssetMenu(fileName = "GameConfigurator", menuName = "ScriptableObjectInstaller/GameConfigurator")]
    public class GameConfigurator : ScriptableObjectInstaller
    {
        [SerializeField] private MainConfigurator _mainConfigurator;

        public override void InstallBindings()
        {
            Container.BindInstance(_mainConfigurator).AsSingle();
        }
    }
}