using UnityEngine;
using System.Collections;

namespace KiBVR.Utilities.VR {
    public class PointTowardsExternal : MonoBehaviour {
        public Transform head;
        public WebCamResolution resolution;

        public enum WebCamResolution { HalfHD, FullHD }

        private VRMasterConfig masterConfig;
        private bool isWebCamReady;


        private void Start() {
            masterConfig = VRMasterConfig.Instance;
        }

        private void Update() {
            if(masterConfig.ExternalCameraTransform != null) {
                if(!isWebCamReady) {
                    var texture = GetWebCamTexture();
                    if(texture != null) {
                        masterConfig.CameraConfig.chromaKey.SetTexture("_MainTex", texture);
                        texture.Play();
                        isWebCamReady = true;

                        Debug.LogWarningFormat("Connected to {0}", texture.deviceName);
                    }
                }

                transform.LookAt(masterConfig.ExternalCameraTransform);

                // Scale based on pov & distance
                var distance = Vector3.Distance(head.position, masterConfig.ExternalCameraTransform.position);
                var camera = masterConfig.ExternalCamera;

                transform.position = masterConfig.ExternalCameraTransform.position + masterConfig.ExternalCameraTransform.forward * distance;
                
                var frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
                var frustumWidth = frustumHeight * camera.aspect;

                transform.localScale = new Vector3(frustumWidth, frustumHeight, 1);
            }
        }


        private WebCamTexture GetWebCamTexture() {
            Debug.LogWarningFormat("Number of cameras: {0}", WebCamTexture.devices.Length);
            for(var i = 0; i < WebCamTexture.devices.Length; ++i) {
                if(WebCamTexture.devices[i].name != "MMP SDK") {
                    return new WebCamTexture(WebCamTexture.devices[i].name, 
                        (resolution == WebCamResolution.FullHD) ? 1920 : 1280, 
                        (resolution == WebCamResolution.FullHD) ? 1080 : 720, 
                        (resolution == WebCamResolution.FullHD) ? 30 : 24);
                }
            }

            return null;
        }
    }
}