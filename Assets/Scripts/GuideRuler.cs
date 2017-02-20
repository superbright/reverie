using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using System;

namespace SB.Seed {

    public class GuideRuler : MonoBehaviour, IPointerStayHandler,IPointerTriggerPressDownHandler {

        public Action<Vector3,AxisLineRenderer.AXIS> moveGuide;
        public Action<Vector3, AxisLineRenderer.AXIS> moveObject;
        public AxisLineRenderer.AXIS axis;

        public void OnPointerStay(PointerEventData eventData)
        {

            moveGuide(eventData.worldPosition,axis);
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void OnPointerTriggerPressDown(ViveControllerModule.EventData eventData)
        {
            moveObject(eventData.worldPosition, axis);
        }
    }
}
