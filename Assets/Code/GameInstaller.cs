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

            Container.Bind<IMainCamera>()
                .To<MainCamera>()
                .FromComponentOn(GameObject.FindGameObjectWithTag("MainCamera"))
                .AsSingle();

			Container.Bind<IGameManager>()
				.To<GameManager>()
				.AsSingle();

			Container.Bind<IGameManagerSettings>()
				.To<GameManagerSettings>()
				.FromResource("GameManagerSettings")
				.AsSingle();

			Container.Bind<IAudioManager>()
				.To<AudioManager>()
				.AsSingle();

			Container.Bind<IAudioSettings>()
				.To<AudioSettings>()
				.FromResource("AudioSettings")
				.AsSingle();
        }
    }
}
