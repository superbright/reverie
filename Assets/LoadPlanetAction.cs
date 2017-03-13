using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using System;
using UnityEngine.UI;

namespace SB.Seed
{

    public class LoadPlanetAction : MonoBehaviour, IPointerTriggerPressSetHandler
    {

        public string planetid = "";

        public void loadroom()
        {
            Debug.Log("load room " + planetid);
            WorldContentManager.Instance.LoadPlanet(planetid);
        }

        void IPointerTriggerPressDownHandler.OnPointerTriggerPressDown(ViveControllerModule.EventData eventData)
        {
            Debug.Log("load room");
            loadroom();
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