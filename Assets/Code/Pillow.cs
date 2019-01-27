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
        void Explode();

        Vector3 Position { get; }
    }

    public class Pillow : MonoBehaviour, IPillow
    {
        private new Rigidbody rigidbody;
        private new Collider collider;

        private const string PillowTag = "Pillow";

        public Vector3 Position => transform.position;

        [Inject]
        private IPillowSettings pillowSettings;

        [Inject]
        private IAudioManager audioManager;

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
        }

        public void Throw(Vector3 direction, LayerMask layer)
        {
            Drop();
            deadly = true;
            rigidbody.AddForce(direction, ForceMode.Impulse);
            gameObject.layer = layer;
        }

        public void Explode()
        {
            var featherParticles = Instantiate(
                pillowSettings.FeatherPuff,
                transform.position,
                Quaternion.identity
            );
            Destroy(featherParticles, pillowSettings.FeatherPuffDuration);
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

            var otherCollider = collision.collider;
            if (otherCollider.CompareTag(PillowTag))
            {
                var otherPillow = otherCollider.GetComponent<IPillow>();
                if (otherPillow == null)
                {
                    throw new Exception("Collided with object with Pillow tag but no Pillow component!");
                }

                audioManager.Play("Hit Squeak");
                otherPillow.Explode();
                Explode();
            }

            if (otherCollider.CompareTag(Player.Player.PlayerTag))
            {
                var player = otherCollider.GetComponent<Player.Player>();
                if (player == null)
                {
                    throw new Exception("Collided with object with Player tag but no Player component!");
                }
                audioManager.Play("Hit Thud");
                Explode();
                player.Die();
            }
        }
    }
}
