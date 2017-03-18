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
    public class UIButtonTrigger : MonoBehaviour, IPointerTriggerPressSetHandler, IPointerEnterHandler,IPointerExitHandler
    {
        public int index;
        bool inUse = false;

        public void okDone()
        {
            GetComponent<Image>().color = ApplicationConfig.Instance.MENUCOLOR;
            inUse = false;
        }

        void IPointerTriggerPressDownHandler.OnPointerTriggerPressDown(ViveControllerModule.EventData eventData)
        {
            if (!inUse)
            {
                GetComponent<Image>().color = ApplicationConfig.Instance.MENULOADINGCOLOR;
               
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

        public void OnPointerEnter(PointerEventData eventData)
        {
          
            GetComponent<Image>().color = ApplicationConfig.Instance.MENUHOVERCOLOR;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
           
            if (!inUse)
            {
                GetComponent<Image>().color = ApplicationConfig.Instance.MENUCOLOR;
            }
        }
    }
}