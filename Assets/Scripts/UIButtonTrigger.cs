using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using System;
using UnityEngine.UI;

namespace SB.Seed
{
    [RequireComponent(typeof(Receiver))]
    [RequireComponent(typeof(Collider))]
    public class UIButtonTrigger : MonoBehaviour, IPointerTriggerPressSetHandler
    {

        public TriggerMenuButton triggerHandler;
        public int index;


        void IPointerTriggerPressDownHandler.OnPointerTriggerPressDown(ViveControllerModule.EventData eventData)
        {
            triggerHandler.triggerPrefab(index, transform.position);
        }

        public void OnPointerTriggerPress(ViveControllerModule.EventData eventData)
        {
            //throw new NotImplementedException();
        }

        public void OnPointerTriggerPressUp(ViveControllerModule.EventData eventData)
        {
            //throw new NotImplementedException();
        }

    }
}