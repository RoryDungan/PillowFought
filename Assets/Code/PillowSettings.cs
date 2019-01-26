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
    }

    [CreateAssetMenu(fileName = "PillowSettings", menuName = "Pillow Fought/Pillow settings")]
    public class PillowSettings : ScriptableObject, IPillowSettings
    {
        [SerializeField]
        [Tooltip("How long the grab animation lasts for.")]
        private float grabAnimDuration = 0.2f;

        public float GrabAnimDuration => grabAnimDuration;
    }
}
