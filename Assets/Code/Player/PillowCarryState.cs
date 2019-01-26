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
        private readonly ThrowState.Factory throwStateFactory;

        public PillowCarryState(
            IPlayer player,
            IPillow pillow,
            PlayerMovement.Factory playerMovementFactory,
            IInputManager inputManager,
            WalkState.Factory walkStateFactory,
            ThrowState.Factory throwStateFactory
        )
        {
            this.player = player;
            this.pillow = pillow;
            this.playerMovement = playerMovementFactory.Create(player);
            this.inputManager = inputManager;
            this.walkStateFactory = walkStateFactory;
            this.throwStateFactory = throwStateFactory;
        }

        public override void Start()
        {
            pillow.Grab(player.GrabTarget);
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
                pillow.Drop();
                player.SetState(walkStateFactory.Create(player));
            }

            if (inputManager.GetThrowButtonDown(player.ControllerIndex))
            {
                player.SetState(throwStateFactory.Create(player, pillow));
            }
        }

        public class Factory : PlaceholderFactory<IPlayer, IPillow, PillowCarryState>{}
    }
}
