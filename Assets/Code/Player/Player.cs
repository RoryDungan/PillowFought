using System;
using UnityEngine;
using Zenject;

namespace ElMoro.Player
{
    public interface IPlayer
    {
        Vector3 Position { get; }
        Vector3 Forward { get; }
        Transform GrabTarget { get; }

        int ControllerIndex { get; }

        void SetVelocity(Vector3 velocity);

        void SetRotation(Quaternion rotation);

        void SetState(PlayerState newState);

        void SetWalkAnim(bool walking);

        int Layer { get; }

        void Die();
    }

    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField]
        [Tooltip("Which controller should control this player.")]
        private int index;

        public int ControllerIndex => index;

        private new Rigidbody rigidbody;
        private Transform grabTarget;
        private PlayerAnimationController animController;

        public Vector3 Position => transform.position;
        public Vector3 Forward => transform.forward;
        public Transform GrabTarget => grabTarget;
        public int Layer => gameObject.layer;

        private PlayerState currentState;

        [Inject]
        private WalkState.Factory walkStateFactory;

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

            animController = GetComponent<PlayerAnimationController>();
            if (animController == null)
            {
                throw new Exception("Can'd find the Animation Controller");
            }

            SetState(walkStateFactory.Create(this));
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
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState));
            }

            currentState?.Dispose();
            currentState = newState;
            currentState.Start();
        }

        public void SetVelocity(Vector3 value) => rigidbody.velocity = value;

        public void SetRotation(Quaternion value) => rigidbody.rotation = value;

        public void SetWalkAnim(bool walking) => animController.Walk(walking);

        public void Die()
        {
            // TODO
        }
    }
}
