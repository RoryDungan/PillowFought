using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ElMoro
{
    /// <summary>
    /// Handles moving a player around the world.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        private new Rigidbody rigidbody;

        [SerializeField]
        [Tooltip("Which controller should control this player.")]
        private int playerIndex;

        [SerializeField]
        [Tooltip("Default number of units to move per second.")]
        private float movementSpeed = 10f;

        [SerializeField]
        [Tooltip("How quickly the player should rotate to face the direction of movement")]
        private float maxRotationSpeed = 20f;

        [Inject]
        private IInputManager InputManager { get; set; }

        [Inject]
        private IMainCamera MainCamera { get; set; }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                throw new Exception("Could not find Rigidbody component on PlayerMovement object.");
            }
        }

        private void FixedUpdate()
        {
            var movement = InputManager.GetMovementDirection(playerIndex);
            MovePlayer(movement);
            FaceDirectionOfMovement(movement);
        }

        private void MovePlayer(Vector2 movementDirection)
        {
            var movementDelta = movementDirection
                * Time.fixedDeltaTime * movementSpeed;

            var inputDirection = new Vector3(
                movementDirection.x,
                0f,
                movementDirection.y
            ).normalized;
            var cameraRotation = Quaternion.Euler(0f, MainCamera.RotationEuler.y, 0f);

            rigidbody.velocity = (cameraRotation * inputDirection) * movementSpeed;
        }

        private void FaceDirectionOfMovement(Vector2 movementDirection)
        {
            var inputDirection = new Vector3(
                movementDirection.x,
                0f,
                movementDirection.y
            );

            if (inputDirection == Vector3.zero)
            {
                return;
            }

            var relativeToCamera = Quaternion.LookRotation(MainCamera.Forward)
                * inputDirection;

            var normalised = new Vector3(
                relativeToCamera.x,
                0f,
                relativeToCamera.z
            ).normalized;

            rigidbody.rotation = Quaternion.LookRotation(
                Vector3.Lerp(
                    transform.forward,
                    normalised,
                    maxRotationSpeed * Time.fixedDeltaTime
                )
            );
        }
    }
}
