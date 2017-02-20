using UnityEngine;
using System.Collections;

namespace KiBVR.Utilities.VR {
    public class VRMasterConfig : MonoBehaviour {
        private static VRMasterConfig _instance;

        public static VRMasterConfig Instance {
            get {
                if(_instance == null) {
                    _instance = FindObjectOfType<VRMasterConfig>();
                    if(_instance == null) {
                        var go = new GameObject("VR Master Config");
                        _instance = go.AddComponent<VRMasterConfig>();
                    }
                }

                return _instance;
            }
        }

        [SerializeField]
        private VRCameraConfig config;
        public bool useExternalConfig;

        public VRCameraConfig CameraConfig { get { return config; } }
        public Camera ExternalCamera { get; set; }
        public Transform ExternalCameraTransform { get; set; }
    }
}