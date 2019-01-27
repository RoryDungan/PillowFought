using System;
using System.Collections;
using UnityEngine;
using Zenject;
using ElMoro.Player;

namespace ElMoro
{
    /// <summary>
    /// A throwable pillow.
    /// </summary>
    public interface IPillow
    {
        /// <summary>
        /// Pick up the pillow and attach it to the specified object. The
        /// pillow must not be being carried.
        /// </summary>
        void Grab(Transform newParent);

        /// <summary>
        /// Drop the pillow. The pillow must be being carried first.
        /// </summary>
        void Drop();

        /// <summary>
        /// Throw the pillow in the specified direction. The pillow must be
        /// being carried.
        /// </summary>
        void Throw(Vector3 direction, LayerMask layer);

        /// <summary>
        /// Explode this pillow in a burst of feathers.
        /// </summary>
        /// <param name="direction">
        /// The direction the featherparticles should shoot out in.
        /// </param>
        void Explode(Vector3 direction);

        Vector3 Position { get; set; }

        void ToggleButtonPrompt(bool active);
    }

    public class Pillow : MonoBehaviour, IPillow
    {
        private new Rigidbody rigidbody;
        private new Collider collider;

        public const string PillowTag = "Pillow";

        public Vector3 Position
        {
            get => rigidbody.position;
            set => rigidbody.position = value;
        }

        public WindowSchematic trackableButtonPrompt;
        public GameObject TrackableButtonPromotObject { get; private set; }

        [Inject]
        private IPillowSettings pillowSettings;

        [Inject]
        private IAudioManager audioManager;

        [SerializeField]
        private ParticleSystem explodeParticles;

        private bool deadly = false;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                throw new Exception("Could not find Rigidbody component on Pillow.");
            }

            collider = GetComponent<Collider>();
            if (collider == null)
            {
                throw new Exception("Could not find Collider component on Pillow.");
            }

            if (explodeParticles == null)
            {
                throw new Exception("No explodeParticles assigned to pillow.");
            }
        }

        private void Start()
        {
            TrackableButtonPromotObject =
                WindowCreator.instance.CreateTrackableWindow(trackableButtonPrompt, this.gameObject);
            ToggleButtonPrompt(false);
        }

        private IEnumerator SmoothLerpToPositionLocal(
            Vector3 targetPos,
            Quaternion targetRot,
            float duration
        )
        {
            var startingPos = transform.localPosition;
            var startingRot = transform.localRotation;
            var startTime = Time.time;
            // Disable collider while we are picking the object up so it doesn't
            // hit the player.
            collider.enabled = false;
            while (Time.time <= startTime + duration)
            {
                // Cancel if detached
                if (transform.parent == null)
                {
                    break;
                }

                // Move towards target position
                var currentPos = Mathf.SmoothStep(
                    0f,
                    1f,
                    Mathf.Clamp01((Time.time - startTime) / duration)
                );
                transform.localPosition = Vector3.Lerp(
                    startingPos,
                    targetPos,
                    currentPos
                );
                transform.localRotation = Quaternion.Lerp(
                    startingRot,
                    targetRot,
                    currentPos
                );
                yield return new WaitForEndOfFrame();
            }
            collider.enabled = true;
            yield return null;
        }

        public void ToggleButtonPrompt(bool active)
        {
            TrackableButtonPromotObject.GetComponent<TrackableUIElement>()
                .ToggleTracking(active);
        }

        public void Drop()
        {
            transform.SetParent(null);
            rigidbody.isKinematic = false;
        }

        public void Grab(Transform newParent)
        {
            transform.SetParent(newParent);
            rigidbody.isKinematic = true;
            StartCoroutine(SmoothLerpToPositionLocal(
                Vector3.zero,
                Quaternion.identity,
                pillowSettings.GrabAnimDuration
            ));
			audioManager.Play("Pickup");
        }

        public void Throw(Vector3 direction, LayerMask layer)
        {
            Drop();
            deadly = true;
            rigidbody.AddForce(direction, ForceMode.Impulse);
            gameObject.layer = layer;
			audioManager.Play("Throw");
        }

        public void Explode(Vector3 direction)
        {
            var particleRotation = Quaternion.LookRotation(direction, Vector3.up);

            var featherParticles = Instantiate(
                pillowSettings.FeatherPuff,
                transform.position,
                particleRotation
            );

            var pillowParticles = Instantiate(
                explodeParticles,
                transform.position,
                particleRotation
            );

            Destroy(featherParticles, pillowSettings.FeatherPuffDuration);
            Destroy(pillowParticles, pillowSettings.FeatherPuffDuration);
            Destroy(gameObject);
        }

        private bool IsDeadly()
        {
            return deadly;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsDeadly())
            {
                audioManager.Play("Hit Thud");
                return;
            }

            deadly = false;

            var particleBurstDirection = rigidbody.velocity.normalized;

            var otherCollider = collision.collider;
            if (otherCollider.CompareTag(PillowTag))
            {
                var otherPillow = otherCollider.GetComponent<IPillow>();
                if (otherPillow == null)
                {
                    throw new Exception("Collided with object with Pillow tag but no Pillow component!");
                }

                audioManager.Play("Hit Thud");
                otherPillow.Explode(particleBurstDirection);
                Explode(particleBurstDirection);
            }

            if (otherCollider.CompareTag(Player.Player.PlayerTag))
            {
                var player = otherCollider.GetComponent<Player.Player>();
                if (player == null)
                {
                    throw new Exception("Collided with object with Player tag but no Player component!");
                }
                audioManager.Play("Hit Thud");
                audioManager.Play("Hit Squeak", 0.3f);
                Explode(particleBurstDirection);
                player.Die();
            }
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, IPillow>{};
    }
}
