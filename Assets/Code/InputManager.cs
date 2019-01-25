using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ElMoro
{
    /// <summary>
    /// Provides an interface for controller/keyboard input.
    /// </summary>
    public interface IInputManager
    {
        /// <summary>
        /// Return the directional input for the specified player.
        /// </summary>
        Vector2 GetMovementDirection(int playerIndex);
    }

    public class InputManager : IInputManager
    {
        public Vector2 GetMovementDirection(int playerIndex)
        {
            if (playerIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }
            if (playerIndex == 0)
            {
                var x = (Input.GetKey(KeyCode.D) ? 1 : 0)
                    + (Input.GetKey(KeyCode.A) ? -1 : 0);
                var y = (Input.GetKey(KeyCode.W) ? 1 : 0)
                    + (Input.GetKey(KeyCode.S) ? -1 : 0);

                var movement = new Vector2(x, y);
                movement.Normalize();
                return movement;
            }

            throw new NotImplementedException(
                "Multiple controller support not implemented"
            );
        }
    }
}