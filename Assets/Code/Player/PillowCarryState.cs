using Zenject;

namespace ElMoro.Player
{
    public class PillowCarryState : PlayerState
    {
        private readonly IPlayer player;
        private readonly IPillow pillow;
        private readonly IPlayerMovement playerMovement;
        private readonly IInputManager inputManager;
        private readonly WalkState.Factory walkStateFactory;

        public PillowCarryState(
            IPlayer player,
            IPillow pillow,
            PlayerMovement.Factory playerMovementFactory,
            IInputManager inputManager,
            WalkState.Factory walkStateFactory
        )
        {
            this.player = player;
            this.pillow = pillow;
            this.playerMovement = playerMovementFactory.Create(player);
            this.inputManager = inputManager;
            this.walkStateFactory = walkStateFactory;
        }

        public override void Start()
        {
            pillow.Grab(player.GrabTarget);
        }

        public override void Dispose()
        {
            pillow.Drop();
        }

        public override void FixedUpdate()
        {
            playerMovement.MovePlayer();
            playerMovement.FaceDirectionOfMovement();
        }

        public override void Update()
        {
            if (inputManager.GetGrabButtonDown(player.ControllerIndex))
            {
                player.SetState(walkStateFactory.Create(player));
            }
        }

        public class Factory : PlaceholderFactory<IPlayer, IPillow, PillowCarryState>{}
    }
}
