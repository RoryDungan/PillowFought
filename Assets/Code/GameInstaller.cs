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
        }
    }
}