using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace KiBVR.Utilities.VR {
    using Input = UnityEngine.Input;

    /// <summary>
    /// GUI Menu for configuring the camera settings ingame instead of heirarchy editing
    /// </summary>
    public class VRCameraConfigMenu : MonoBehaviour {
        [SerializeField]
        private VRCameraConfig config;

        [SerializeField]
        private KeyCode toggleKey;

        private bool isEditingConfig;
        private GUIStyle vectorStyle;


        private void Start() {
            Assert.IsNotNull(config);
            isEditingConfig = false;

            vectorStyle = new GUIStyle();
            vectorStyle.alignment = TextAnchor.LowerRight;
            vectorStyle.normal.textColor = Color.white;
        }

        private void Update() {
            if(Input.GetKeyUp(toggleKey)) {
                isEditingConfig = !isEditingConfig;
                Cursor.visible = isEditingConfig;
                Cursor.lockState = ( isEditingConfig ) ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        private void OnGUI() {
            if(isEditingConfig) {
                GUILayout.BeginArea(new Rect(0, 0, Screen.width / 2f, Screen.height * 0.75f));
                GUI.Box(new Rect(0, 0, Screen.width / 2f, Screen.height * 0.75f), "");
                GUILayout.Label("Camera Config");
                GUILayout.Space(20);

                // Camera Transform Properties
                GUILayout.Label("Camera Transform");
                DrawOffSetPosition();
                DrawOffSetRotation();
                DrawFieldOfView();
                DrawNearValue();
                DrawFarValue();
                DrawNearClipOffset();
                DrawHMDOffsetValue();

                if(GUILayout.Button("Close")) {
                    isEditingConfig = false;
                    Cursor.visible = isEditingConfig;
                    Cursor.lockState = ( isEditingConfig ) ? CursorLockMode.None : CursorLockMode.Locked;
                }

                GUILayout.EndArea();
            }
        }

        private void DrawOffSetPosition() {
            GUILayout.BeginHorizontal();

            var x = config.offsetPosition.x;
            var y = config.offsetPosition.y;
            var z = config.offsetPosition.z;

            GUILayout.Label("Offset Position");
            GUILayout.Label("x", vectorStyle, GUILayout.Width(20));
            float.TryParse(GUILayout.TextField(x.ToString("F2")), out x);
            GUILayout.Label("y", vectorStyle, GUILayout.Width(20));
            float.TryParse(GUILayout.TextField(y.ToString("F2")), out y);
            GUILayout.Label("z", vectorStyle, GUILayout.Width(20));
            float.TryParse(GUILayout.TextField(z.ToString("F2")), out z);

            config.offsetPosition = new Vector3(x, y, z);
            
            GUILayout.EndHorizontal();
        }

        private void DrawOffSetRotation() {
            GUILayout.BeginHorizontal();

            var x = config.offsetRotation.x;
            var y = config.offsetRotation.y;
            var z = config.offsetRotation.z;

            GUILayout.Label("Offset Position", GUILayout.Width(150f));
            GUILayout.Label("x", vectorStyle, GUILayout.Width(20));
            float.TryParse(GUILayout.TextField(x.ToString("F2")), out x);
            GUILayout.Label("y", vectorStyle, GUILayout.Width(20));
            float.TryParse(GUILayout.TextField(y.ToString("F2")), out y);
            GUILayout.Label("z", vectorStyle, GUILayout.Width(20));
            float.TryParse(GUILayout.TextField(z.ToString("F2")), out z);

            config.offsetRotation = new Vector3(x, y, z);
            
            GUILayout.EndHorizontal();
        }

        private void DrawFieldOfView() {
            GUILayout.BeginHorizontal();

            GUILayout.Label(string.Format("Field Of View: {0}", config.fieldOfView), GUILayout.Width(150f));
            config.fieldOfView = GUILayout.HorizontalSlider(config.fieldOfView, 5f, 180f);

            GUILayout.EndHorizontal();
        }

        private void DrawNearValue() {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Near Distance", GUILayout.Width(150f));
            float.TryParse(GUILayout.TextField(config.near.ToString("F2")), out config.near);

            GUILayout.EndHorizontal();
        }

        private void DrawFarValue() {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Far Distance", GUILayout.Width(150f));
            float.TryParse(GUILayout.TextField(config.far.ToString("F2")), out config.far);

            GUILayout.EndHorizontal();
        }

        private void DrawNearClipOffset() {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Near Clip Offset", GUILayout.Width(150f));
            float.TryParse(GUILayout.TextField(config.nearClipOffset.ToString("F2")), out config.nearClipOffset);

            GUILayout.EndHorizontal();
        }

        private void DrawHMDOffsetValue() {
            GUILayout.BeginHorizontal();

            GUILayout.Label("HMD Offset Distance", GUILayout.Width(150f));
            float.TryParse(GUILayout.TextField(config.hmdOffset.ToString("F2")), out config.hmdOffset);

            GUILayout.EndHorizontal();
        }
    }
}