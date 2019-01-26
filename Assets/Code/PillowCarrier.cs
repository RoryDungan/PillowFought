using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ElMoro
{
    /// <summary>
    /// Object that can pick up pillows.
    /// </summary>
    public class PillowCarrier : MonoBehaviour
    {
        [SerializeField]
        private int playerIndex = 0;

        [Inject]
        private IPlayerSettings PlayerSettings { get; set; }

        [Inject]
        private IInputManager InputManager { get; set; }

        private const string PillowTag = "Pillow";

        private IPillow currentPillow = null;

        private Transform grabTarget;

        private void Awake()
        {
            grabTarget = transform.Find("GrabTarget");
            if (grabTarget == null)
            {
                throw new Exception("PillowCarrier requires a child named GrabTarget but it could not be found!");
            }
        }

        private void Update()
        {
            if (InputManager.GetGrabButtonDown(playerIndex))
            {
                if (currentPillow == null)
                {
                    var newPillow = AttemptGrab();
                    if (newPillow != null)
                    {
                        newPillow.Grab(grabTarget);
                        currentPillow = newPillow;
                    }
                }
                else
                {
                    currentPillow.Drop();
                    currentPillow = null;
                }
            }

            if (InputManager.GetThrowButtonUp(playerIndex) && currentPillow != null)
            {
                currentPillow.Throw(transform.forward * PlayerSettings.MinThrowForce, gameObject.layer);
            }
        }

        /// <summary>
        /// Attempt to grab for a pillow. Returns the grabbed pillow, or null
        /// if none was found.
        /// </summary>
        private IPillow AttemptGrab()
        {
            // TODO: play grab animation

            var rayStart = transform.position;
            var rayEnd = transform.forward * PlayerSettings.PickupDistance;

            Debug.DrawRay(rayStart, rayEnd, Color.green, 1f);

            var hits = Physics.RaycastAll(
                new Ray(rayStart, rayEnd),
                PlayerSettings.PickupDistance
            );

            return hits.Select(h => h.transform)
                .Where(t => t.CompareTag(PillowTag))
                .Select(t => t.GetComponent<IPillow>())
                .Where(p => p != null)
                .OrderBy(p => (p.Position - transform.position).sqrMagnitude)
                .FirstOrDefault();
        }
    }
}
