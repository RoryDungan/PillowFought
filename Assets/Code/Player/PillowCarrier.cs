using System;
using System.Linq;
using ElMoro.Player;
using UnityEngine;
using Zenject;

namespace ElMoro
{
    /// <summary>
    /// Object that can pick up pillows.
    /// </summary>
    public interface IPillowCarrier
    {
        IPillow AttemptGrab();
    }

    public class PillowCarrier : IPillowCarrier
    {
        private readonly IPlayerSettings playerSettings;
        private readonly IPlayer player;

        private const string PillowTag = "Pillow";

        public PillowCarrier(IPlayer player, IPlayerSettings playerSettings)
        {
            this.player = player;
            this.playerSettings = playerSettings;
        }

        /// <summary>
        /// Attempt to grab for a pillow. Returns the grabbed pillow, or null
        /// if none was found.
        /// </summary>
        public IPillow AttemptGrab()
        {
            // TODO: play grab animation

            var rayStart = player.Position;
            var rayEnd = player.Forward * playerSettings.PickupDistance;

            Debug.DrawRay(rayStart, rayEnd, Color.green, 1f);

            var hits = Physics.RaycastAll(
                new Ray(rayStart, rayEnd),
                playerSettings.PickupDistance
            );

            return hits.Select(h => h.transform)
                .Where(t => t.CompareTag(PillowTag))
                .Select(t => t.GetComponent<IPillow>())
                .Where(p => p != null)
                .OrderBy(p => (p.Position - player.Position).sqrMagnitude)
                .FirstOrDefault();
        }
    }
}
