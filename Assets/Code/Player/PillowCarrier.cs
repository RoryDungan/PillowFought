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

        private RaycastHit[] hitBuffer = new RaycastHit[5];

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
            var rayStart = player.Position;
            var rayEnd = player.Forward * playerSettings.PickupDistance;

            var numHits = Physics.RaycastNonAlloc(
                new Ray(rayStart, rayEnd),
                hitBuffer,
                playerSettings.PickupDistance
            );

            IPillow hit = null;
            var hitDistance = float.MaxValue;
            for (var i = 0; i < numHits; i++)
            {
                if (!hitBuffer[i].transform.CompareTag(PillowTag))
                {
                    continue;
                }
                var pillow = hitBuffer[i].transform.GetComponent<IPillow>();
                if (pillow == null)
                {
                    continue;
                }
                var distance = (pillow.Position - player.Position).sqrMagnitude;
                if (distance < hitDistance)
                {
                    hit = pillow;
                }
            }

            return hit;
        }

        public class Factory : PlaceholderFactory<IPlayer, IPillowCarrier>{}
    }
}
