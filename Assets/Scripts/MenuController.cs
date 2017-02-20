using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;


[RequireComponent(typeof(Receiver))]
public class MenuController : MonoBehaviour, IGlobalTouchpadPressHandler
{
    private Receiver receiver;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void IGlobalTouchpadPressHandler.OnGlobalTouchpadPress(ViveControllerModule.EventData eventData)
    {
        Debug.Log("menu");
    }
}
