using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ElMoro.Player
{
    /// <summary>
    /// Handles moving a player around the world.
    /// </summary>
    public interface IPlayerMovement
    {
        void MovePlayer();
        void FaceDirectionOfMovement();
    }

    public class PlayerMovement : IPlayerMovement
    {
        private readonly IPlayer player;
        private readonly IPlayerSettings playerSettings;
        private readonly IInputManager inputManager;
        private readonly IMainCamera mainCamera;

        public PlayerMovement(
            IPlayer player,
            IPlayerSettings playerSettings,
            IInputManager inputManager,
            IMainCamera mainCamera)
        {
            this.player = player;
            this.playerSettings = playerSettings;
            this.inputManager = inputManager;
            this.mainCamera = mainCamera;
        }

        private Vector2 GetMovementDirection()
        {
            return inputManager.GetMovementDirection(player.ControllerIndex);
        }

        public void MovePlayer()
        {
            var movementDirection = GetMovementDirection();

            var inputDirection = new Vector3(
                movementDirection.x,
                0f,
                movementDirection.y
            ).normalized;
            var cameraRotation = Quaternion.Euler(0f, mainCamera.RotationEuler.y, 0f);

            player.SetVelocity((cameraRotation * inputDirection)
                * Time.fixedDeltaTime
                * playerSettings.MovementSpeed);
        }

        public void FaceDirectionOfMovement()
        {
            var movementDirection = GetMovementDirection();

            var inputDirection = new Vector3(
                movementDirection.x,
                0f,
                movementDirection.y
            );

            if (inputDirection == Vector3.zero)
            {
                return;
            }

            var relativeToCamera = Quaternion.LookRotation(mainCamera.Forward)
                * inputDirection;

            var normalised = new Vector3(
                relativeToCamera.x,
                0f,
                relativeToCamera.z
            ).normalized;

            player.SetRotation(Quaternion.LookRotation(
                Vector3.Lerp(
                    player.Forward,
                    normalised,
                    playerSettings.RotationSpeed * Time.fixedDeltaTime
                )
            ));
        }
    }
}
