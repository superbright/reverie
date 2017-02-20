using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManager : MonoBehaviour {

    public Material lMat;
	public SteamVR_TrackedObject trackedObj;
	private GraphicsLineRenderer currline;
	private int numClicks = 0;
    private ArrayList lines;
        
    // Use this for initialization

    private void Awake()
    {
        lines = new ArrayList();
    }

    private void OnGUI()
    {
        if(GUILayout.Button("clear")) {
            clear();
        }
    }

    // Update is called once per frame
    void Update () {
		SteamVR_Controller.Device device = SteamVR_Controller.Input ((int)trackedObj.index);
		if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			GameObject go = new GameObject ();
            lines.Add(go);
            go.AddComponent<MeshFilter>();  
            go.AddComponent<MeshRenderer>();

            currline = go.AddComponent<GraphicsLineRenderer> ();
            currline.lmat = lMat;
            currline.SetWidth(.1f);
          numClicks = 0; 
        } 
		else if (device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) 
		{
            currline.AddPoint(trackedObj.transform.position);
			numClicks++;
		}
	}

    public void clear()
    {
        numClicks = 0;
        foreach(GameObject line in lines) {
            Destroy(line);
        }
        lines.Clear();
    }
}
