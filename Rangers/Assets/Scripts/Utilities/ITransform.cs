using System;
using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// An interface for an object that has a transform.
    /// </summary>
    public interface ITransform
    {
        /// <summary>
        /// Get the transform of the object.
        /// </summary>
        public Transform Transform { get; }
    }
}
