using UnityEngine;
using System.Collections;

namespace KiBVR.Utilities.VR {
    /// <summary>
    /// An configuration object for modifying the mixed reality camera ingame.  Changes persist after changing
    /// the configuration.
    /// </summary>
    [CreateAssetMenu(menuName = "Steam VR/Camera Config")]
    public class VRCameraConfig : ScriptableObject {
        [Header("Camera Transform")]
        public Vector3 offsetPosition;
        public Vector3 offsetRotation;

        [Header("Camera Config")]
        public float fieldOfView = 45f;
        /// <summary>
        /// The near clipping distance.  Any object closer than this distance will not be rendered
        /// </summary>
        public float near = 0.1f;

        /// <summary>
        /// The far clipping distance.  Any object farther than this distance will not be rendered
        /// </summary>
        public float far = 100f;

        /// <summary>
        /// The offset for the near clipping distance.  Used to adjust what's considered "close"
        /// </summary>
        public float nearClipOffset = 0f;

        /// <summary>
        /// The offset for the head mount display.  Used to adjust the distance between the head and the
        /// external camera
        /// </summary>
        public float hmdOffset = 0f;

        [Header("Rendering")]
        public LayerMask nearRenderingMask;
        public LayerMask farRenderingMask;

        [Header("Materials")]
        public Material chromaKey;
    }
}