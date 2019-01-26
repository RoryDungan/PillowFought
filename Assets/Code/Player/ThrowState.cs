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
            startTime = Time.time;
        }

        public override void FixedUpdate()
        {
            playerMovement.FaceDirectionOfMovement();
        }

        public override void Update()
        {
            if (inputManager.GetThrowButtonUp(player.ControllerIndex))
            {
                Throw();
                player.SetState(walkStateFactory.Create(player));
            }
        }

        private void Throw()
        {
            var chargeAmount = Mathf.Clamp01(
                (Time.time - startTime) / playerSettings.ThrowChargeDuration
            );

            var throwForce = playerSettings.MinThrowForce
                + (playerSettings.MaxThrowForce - playerSettings.MinThrowForce)
                    * chargeAmount;

            pillow.Throw(player.Forward * throwForce, player.Layer);
        }

        public class Factory : PlaceholderFactory<IPlayer, IPillow, ThrowState>{}
    }
}
