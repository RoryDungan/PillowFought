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
        private float movementSpeed = 1;

        [Inject]
        private IInputManager InputManager { get; set; }

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
            var movementDelta = InputManager.GetMovementDirection(playerIndex)
                * Time.deltaTime * movementSpeed;
            rigidbody.velocity = new Vector3(movementDelta.x, 0f, movementDelta.y);
        }
    }
}
