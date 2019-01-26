using System;
using UnityEngine;

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
        void Throw(Vector3 direction);

        Vector3 Position { get; }
    }

    public class Pillow : MonoBehaviour, IPillow
    {
        private new Rigidbody rigidbody;

        public Vector3 Position => transform.position;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                throw new Exception("Could not find Rigidbody component on Pillow.");
            }
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
        }

        public void Throw(Vector3 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
