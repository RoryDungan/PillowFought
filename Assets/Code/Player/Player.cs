using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ElMoro.Player
{
    public interface IPlayer
    {
        Vector3 Position { get; set; }
        Vector3 Forward { get; }
        Transform GrabTarget { get; }

        int ControllerIndex { get; set; }

        void SetVelocity(Vector3 velocity);

        void SetRotation(Quaternion rotation);

        void SetState(PlayerState newState);

        void SetWalkAnim(bool walking);

        void SetPickupAnim();

        void SetPutDownAnim();

        void SetThrowAnim();

        int Layer { get; }

        void Die();
    }

    public class Player : MonoBehaviour, IPlayer
    {
        public const string PlayerTag = "Player";

        [SerializeField]
        [Tooltip("Which controller should control this player.")]
        private int index;

        [SerializeField]
        private ParticleSystem dieParticles;

        public int ControllerIndex
        {
            get => index;
            set => index = value;
        }

        private new Rigidbody rigidbody;
        private Transform grabTarget;
        private PlayerAnimationController animController;

        public Vector3 Position
        {
            get => rigidbody.position;
            set => rigidbody.position = value;
        }
        public Vector3 Forward => transform.forward;
        public Transform GrabTarget => grabTarget;
        public int Layer => gameObject.layer;

        private PlayerState currentState;

        [Inject]
        private WalkState.Factory walkStateFactory;

        [Inject]
        private IGameManager gameManager;

        private void Awake()
        {
            if (dieParticles == null)
            {
                throw new Exception("No dieParticles assigned to player.");
            }

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

        public void SetPickupAnim() => animController.Pickup();

        public void SetPutDownAnim() => animController.PutDown();

        public void SetThrowAnim() => animController.Throw();

        public void Die()
        {
            StartCoroutine(DieCoroutine());
        }

        private IEnumerator DieCoroutine()
        {
            var duration = 5f;

            var particles = Instantiate(dieParticles);
            particles.transform.position = transform.position;
            Destroy(gameObject);
            Destroy(particles, duration);

            yield return new WaitForSeconds(duration);

            gameManager.PlayerDefeated(index);
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, IPlayer>{};
    }
}
