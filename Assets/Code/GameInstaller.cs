using ElMoro.Player;
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

            Container.BindFactory<IPlayer, IPlayerMovement, PlayerMovement.Factory>()
                .To<PlayerMovement>();
            Container.BindFactory<IPlayer, IPillowCarrier, PillowCarrier.Factory>()
                .To<PillowCarrier>();
            Container.BindFactory<IPlayer, WalkState, WalkState.Factory>();
            Container.BindFactory<IPlayer, IPillow, PillowCarryState, PillowCarryState.Factory>();
        }
    }
}
