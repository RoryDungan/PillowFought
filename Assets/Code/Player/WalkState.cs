namespace ElMoro.Player
{
    public class WalkState : PlayerState
    {
        private readonly IPlayer player;
        private readonly IPlayerMovement playerMovement;
        private readonly IInputManager inputManager;

        public WalkState(
            IPlayer player,
            IPlayerMovement playerMovement,
            IInputManager inputManager)
        {
            this.player = player;
            this.playerMovement = playerMovement;
            this.inputManager = inputManager;
        }

        public override void FixedUpdate()
        {
            playerMovement.MovePlayer();
            playerMovement.FaceDirectionOfMovement();
        }

        public override void Update()
        {
        }
    }
}
