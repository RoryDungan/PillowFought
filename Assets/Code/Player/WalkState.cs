using Zenject;

namespace ElMoro.Player
{
    public class WalkState : PlayerState
    {
        private readonly IPlayer player;
        private readonly IPlayerMovement playerMovement;
        private readonly IInputManager inputManager;
        private readonly IPillowCarrier pillowCarrier;
        private readonly PillowCarryState.Factory pillowCarryStateFactory;

        public WalkState(
            IPlayer player,
            PlayerMovement.Factory playerMovementFactory,
            PillowCarrier.Factory pillowCarrierFactory,
            IInputManager inputManager,
            PillowCarryState.Factory pillowCarryStateFactory)
        {
            this.player = player;
            this.playerMovement = playerMovementFactory.Create(player);
            this.inputManager = inputManager;
            this.pillowCarrier = pillowCarrierFactory.Create(player);
            this.pillowCarryStateFactory = pillowCarryStateFactory;
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
                var pillow = pillowCarrier.AttemptGrab();

                // TODO: user factory
                if (pillow != null)
                {
                    player.SetState(pillowCarryStateFactory.Create(player, pillow));
                    player.SetPickupAnim();
                }
            }
        }

        public class Factory : PlaceholderFactory<IPlayer, WalkState>{}
    }
}
