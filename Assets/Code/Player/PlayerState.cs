using System;

namespace ElMoro.Player
{
    /// <summary>
    /// Base class for player states.
    /// </summary>
    public abstract class PlayerState : IDisposable
    {
        public virtual void Update() {}

        public virtual void FixedUpdate() {}

        public virtual void Start() {}

        public virtual void Dispose() {}
    }
}
