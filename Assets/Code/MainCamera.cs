using UnityEngine;

namespace ElMoro
{
    /// <summary>
    /// Dependency-injectable class for interfacing with the camera.
    /// </summary>
    public interface IMainCamera
    {
        Vector3 Forward { get; }

        Vector3 RotationEuler { get; }
    }

    public class MainCamera : MonoBehaviour, IMainCamera
    {
        public Vector3 Forward => transform.forward;
        public Vector3 RotationEuler => transform.eulerAngles;
    }
}
