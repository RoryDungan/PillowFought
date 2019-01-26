using System;
using System.Collections;
using UnityEngine;
using Zenject;

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

        Vector3 Position { get; }
    }

    public class Pillow : MonoBehaviour, IPillow
    {
        private new Rigidbody rigidbody;
        private new Collider collider;

        public Vector3 Position => transform.position;

        [Inject]
        private IPillowSettings PillowSettings { get; set; }

		[Inject]
		private IAudioManager audioManager;

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
                PillowSettings.GrabAnimDuration
            ));
        }

        public void Throw(Vector3 direction, LayerMask layer)
        {
            Drop();
            rigidbody.AddForce(direction, ForceMode.Impulse);
			gameObject.layer = layer;
        }

		private void OnCollisionEnter (Collision coll)
		{
			Collider other = coll.collider;
			LayerMask playersLayer = LayerMask.GetMask("Player1", "Player2");

			if (LayerMask.LayerToName(gameObject.layer) != "Default") {
				if (other.CompareTag("Floor")) {
					gameObject.layer = LayerMask.NameToLayer("Default");
				} else if (((1<<other.gameObject.layer) & playersLayer) != 0) {
					audioManager.Play("Hit Thud");
					if (!other.CompareTag("Pillow")) {
						audioManager.Play("Hit Squeak");
						other.GetComponent<Player.Player>().Die();
					}
				}
			}
		}
    }
}
