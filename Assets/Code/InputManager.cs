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

        /// <summary>
        /// Returns whether the grab button was pressed this frame for the
        /// specified player index.
        /// </summary>
        bool GetGrabButtonDown(int playerIndex);

        /// <summary>
        /// Returns whether the grab button was released this frame for the
        /// specified player index.
        /// </summary>
        bool GetGrabButtonUp(int playerIndex);

        /// <summary>
        /// Returns whether the throw button was pressed this frame for the
        /// specified player index.
        /// </summary>
        bool GetThrowButtonDown(int playerIndex);

        /// <summary>
        /// Returns whether the throw button was released this frame for the
        /// specified player index.
        /// </summary>
        bool GetThrowButtonUp(int playerIndex);

        /// <summary>
        /// Returns whether the throw button is currently pressed for the
        /// specified player index.
        /// </summary>
        bool GetThrowButton(int playerIndex);
    }

    public class InputManager : IInputManager
    {
        const string Player0HorizontalAxis = "Horizontal 0";
        const string Player0VerticalAxis = "Vertical 0";
        const string Player0Grab = "B 0";
        const string Player0Throw = "A 0";
        const string Player1HorizontalAxis = "Horizontal 1";
        const string Player1VerticalAxis = "Vertical 1";
        const string Player1Grab = "B 1";
        const string Player1Throw = "A 1";

        public Vector2 GetMovementDirection(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0:
                {
                    // Keyboard controlls for testing.
                    var x = (Input.GetKey(KeyCode.D) ? 1f : 0f)
                        + (Input.GetKey(KeyCode.A) ? -1f : 0f);
                    var y = (Input.GetKey(KeyCode.W) ? 1f : 0f)
                        + (Input.GetKey(KeyCode.S) ? -1f : 0f);

                    // Joy-con (R)
                    x += Input.GetAxis(Player0HorizontalAxis);
                    y += Input.GetAxis(Player0VerticalAxis);

                    var movement = new Vector2(x, y);
                    movement.Normalize();
                    return movement;
                }
                case 1:
                {
                    // Joy-con (L)
                    var x = Input.GetAxis(Player1HorizontalAxis);
                    var y = Input.GetAxis(Player1VerticalAxis);

                    var movement = new Vector2(x, y);
                    movement.Normalize();
                    return movement;
                }
                default:
                    throw new NotImplementedException(
                        "Multiple controller support not implemented"
                    );
            }
        }

        public bool GetGrabButtonDown(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0:
                    return Input.GetButtonDown(Player0Grab) || Input.GetKeyDown(KeyCode.Space);

                case 1:
                    return Input.GetButtonDown(Player1Grab);

                default:
                    throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }
        }

        public bool GetGrabButtonUp(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0:
                    return Input.GetButtonUp(Player0Grab) || Input.GetKeyUp(KeyCode.Space);

                case 1:
                    return Input.GetButtonUp(Player1Grab);

                default:
                    throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }
        }

        public bool GetThrowButtonDown(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0:
                    return Input.GetButtonDown(Player0Throw) || Input.GetKeyDown(KeyCode.F);

                case 1:
                    return Input.GetButtonDown(Player1Throw);

                default:
                    throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }
        }

        public bool GetThrowButtonUp(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0:
                    return Input.GetButtonUp(Player0Throw) || Input.GetKeyUp(KeyCode.F);

                case 1:
                    return Input.GetButtonUp(Player1Throw);

                default:
                    throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }
        }

        public bool GetThrowButton(int playerIndex)
        {
            switch (playerIndex)
            {
                case 0:
                    return Input.GetButton(Player0Throw) || Input.GetKey(KeyCode.F);

                case 1:
                    return Input.GetButton(Player1Throw);

                default:
                    throw new ArgumentOutOfRangeException(nameof(playerIndex));
            }
        }
    }
}
