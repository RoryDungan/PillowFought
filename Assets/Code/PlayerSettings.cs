using UnityEngine;

namespace ElMoro
{
    public interface IPlayerSettings
    {
        /// <summary>
        /// How far away the player can be from a pillow to grab it.
        /// </summary>
        float PickupDistance { get; }

        /// <summary>
        /// How fast to move in units per second
        /// </summary>
        float MovementSpeed { get; }

        /// <summary>
        /// How quickly the player should rotate to face the direction of movement.
        /// </summary>
        float RotationSpeed { get; }

        /// <summary>
        /// The initial throw force.
        /// </summary>
        float MinThrowForce { get; }

        /// <summary>
        /// Throw force after charging up.
        /// </summary>
        float MaxThrowForce { get; }
    }

    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Pillow Fought/Player settings")]
    public class PlayerSettings : ScriptableObject, IPlayerSettings
    {
        [SerializeField]
        [Tooltip("How far away the player can be from a pillow to grab it.")]
        private float pickupDistance = 1f;

        public float PickupDistance => pickupDistance;

        [SerializeField]
        [Tooltip("Default number of units to move per second.")]
        private float movementSpeed = 10f;

        public float MovementSpeed => movementSpeed;

        [SerializeField]
        [Tooltip("How quickly the player should rotate to face the direction of movement")]
        private float rotationSpeed = 20f;

        public float RotationSpeed => rotationSpeed;

        [SerializeField]
        private float minThrowForce = 10f;

        public float MinThrowForce => minThrowForce;

        [SerializeField]
        private float maxThrowForce = 30f;

        public float MaxThrowForce => maxThrowForce;
    }
}
