using UnityEngine;

namespace ElMoro
{
    /// <summary>
    /// Container for settings common to all pillows.
    /// </summary>
    public interface IPillowSettings
    {
        /// <summary>
        /// How long the grab animation lasts for.
        /// </summary>
        float GrabAnimDuration { get; }

        /// <summary>
        /// Particle system to trigger when pillow is destroyed.
        /// </summary>
        ParticleSystem FeatherPuff { get; }

        /// <summary>
        /// How long the feather puff particles should exist for before being
        /// destroyed.
        /// </summary>
        float FeatherPuffDuration { get; }
    }

    [CreateAssetMenu(fileName = "PillowSettings", menuName = "Pillow Fought/Pillow settings")]
    public class PillowSettings : ScriptableObject, IPillowSettings
    {
        [SerializeField]
        [Tooltip("How long the grab animation lasts for.")]
        private float grabAnimDuration = 0.2f;

        public float GrabAnimDuration => grabAnimDuration;

        [SerializeField]
        [Tooltip("Particle system to trigger when pillow is destroyed.")]
        private ParticleSystem featherPuff;

        public ParticleSystem FeatherPuff => featherPuff;

        [SerializeField]
        [Tooltip("How long the feather puff particles should exist for before being destroyed.")]
        private float featherPuffDuration = 5f;

        public float FeatherPuffDuration => featherPuffDuration;
    }
}
