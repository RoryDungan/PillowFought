using System;
using UnityEngine;

namespace ElMoro.Player
{
    public interface IPlayer
    {
        Vector3 Position { get; }
        Vector3 Forward { get; }

        int ControllerIndex { get; }

        void SetVelocity(Vector3 velocity);

        void SetRotation(Quaternion rotation);

        void SetState(PlayerState newState);
    }

    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField]
        [Tooltip("Which controller should control this player.")]
        private int index;

        public int ControllerIndex => index;

        private new Rigidbody rigidbody;
        private Transform grabTarget;

        public Vector3 Position => transform.position;
        public Vector3 Forward => transform.forward;

        private PlayerState currentState;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                throw new Exception("Player is missing Rigidbody component.");
            }

            grabTarget = transform.Find("GrabTarget");
            if (grabTarget == null)
            {
                throw new Exception("PillowCarrier requires a child named GrabTarget but it could not be found!");
            }
        }

        private void Update()
        {
            currentState.Update();
        }

        private void FixedUpdate()
        {
            currentState.FixedUpdate();
        }

        public void SetState(PlayerState newState)
        {
            // TODO
        }

        public void SetVelocity(Vector3 value) => rigidbody.velocity = value;

        public void SetRotation(Quaternion value) => rigidbody.rotation = value;
    }
}
