using UnityEngine;
using Zenject;

namespace ElMoro.Player
{
    public class ThrowState : PlayerState
    {
        private readonly IPlayer player;
        private readonly IPillow pillow;
        private readonly IInputManager inputManager;
        private readonly IPlayerSettings playerSettings;
        private readonly IPlayerMovement playerMovement;
        private readonly WalkState.Factory walkStateFactory;

        private float startTime;

        public ThrowState(
            IPlayer player,
            IPillow pillow,
            IInputManager inputManager,
            IPlayerSettings playerSettings,
            PlayerMovement.Factory playerMovementFactory,
            WalkState.Factory walkStateFactory
        )
        {
            this.player = player;
            this.pillow = pillow;
            this.inputManager = inputManager;
            this.playerSettings = playerSettings;
            this.playerMovement = playerMovementFactory.Create(player);
            this.walkStateFactory = walkStateFactory;
        }

        public override void Start()
        {
			player.ThrowLine.enabled = true;
            startTime = Time.time;
        }

        public override void FixedUpdate()
        {
            playerMovement.FaceDirectionOfMovement();
        }

		private float throwForce;

        public override void Update()
        {
            var chargeAmount = Mathf.Clamp01(
                (Time.time - startTime) / playerSettings.ThrowChargeDuration
            );

			throwForce = playerSettings.MinThrowForce + (playerSettings.MaxThrowForce
			- playerSettings.MinThrowForce) * chargeAmount;

			RaycastHit hit;
			float distDown = 0;
			if (Physics.Raycast(player.GrabTarget.position, Vector3.down, out hit)) {
				distDown = hit.distance;
			}

			float forwardDelta = throwForce * Mathf.Sqrt(Mathf.Abs((2 * distDown) / Physics.gravity.y));

			player.ThrowLine.SetPosition(0, player.GrabTarget.position);
			player.ThrowLine.SetPosition(1, player.Position + player.Forward * forwardDelta + Vector3.down * hit.distance);

            if (!inputManager.GetThrowButton(player.ControllerIndex))
            {
                Throw();
                player.SetState(walkStateFactory.Create(player));
            }
        }

        private void Throw()
        {
            pillow.Throw(player.Forward * throwForce, player.Layer);
			player.ThrowLine.enabled = false;
        }

        public class Factory : PlaceholderFactory<IPlayer, IPillow, ThrowState>{}
    }
}
