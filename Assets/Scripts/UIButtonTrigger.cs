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
        public int index;
        bool inUse = false;

        public void okDone()
        {
            inUse = true;
        }

        void IPointerTriggerPressDownHandler.OnPointerTriggerPressDown(ViveControllerModule.EventData eventData)
        {
            if (!inUse)
            {
                WorldContentManager.Instance.AddObject(index.ToString(), index, transform);
                inUse = true;
            }
            //ObjectFactory.Instance.triggerPrefab(index, transform.position);
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