using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;

[RequireComponent(typeof(Receiver))]
public class MenuManager : MonoBehaviour, IGlobalTouchpadPressDownHandler
{
    // public TriggerMenuButton triggerHandler;
    // public int index;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGlobalTouchpadPressDown(ViveControllerModule.EventData eventData)
    {

        transform.position = eventData.module.gameObject.transform.position;
        Vector3 updatedrotation = transform.rotation.eulerAngles;
        updatedrotation.y = eventData.module.gameObject.transform.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Euler(updatedrotation);
        transform.LookAt(eventData.module.transform);


    }
}
