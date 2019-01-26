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

        private Transform currentPillow = null;

        private void Update()
        {
            if (InputManager.GetGrabButtonDown(playerIndex))
            {
                var newPillow = AttemptGrab();
                if (newPillow != null)
                {
                    newPillow.transform.SetParent(transform);
                    currentPillow = newPillow;
                }
            }

            if (!InputManager.GetGrabButton(playerIndex) && currentPillow != null)
            {
                currentPillow.SetParent(null);
            }
        }

        /// <summary>
        /// Attempt to grab for a pillow. Returns the grabbed pillow, or null
        /// if none was found.
        /// </summary>
        private Transform AttemptGrab()
        {
            // TODO: play grab animation

            var hits = Physics.RaycastAll(
                new Ray(transform.position, transform.forward),
                PlayerSettings.PickupDistance
            );

            return hits.Select(h => h.transform)
                .Where(t => t.CompareTag(PillowTag))
                .OrderBy(t => (t.position - transform.position).sqrMagnitude)
                .FirstOrDefault();
        }
    }
}
