using UnityEngine;
using Zenject;

namespace ElMoro
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputManager>()
                .To<InputManager>()
                .AsSingle();

            Container.Bind<IPlayerSettings>()
                .To<PlayerSettings>()
                .FromResource("PlayerSettings")
                .AsSingle();

            Container.Bind<IPillowSettings>()
                .To<PillowSettings>()
                .FromResource("PillowSettings")
                .AsSingle();

            Container.Bind<IMainCamera>()
                .To<MainCamera>()
                .FromComponentOn(GameObject.FindGameObjectWithTag("MainCamera"))
                .AsSingle();
        }
    }
}
