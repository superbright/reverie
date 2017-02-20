using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using System;

namespace SB.Seed
{

    public enum DOOType
    {
        OBJECT,
        FLOOR,
        MENU,
        NONE
    }

    public class DOOObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public HandControllers handController;
        public DOOType type;

        public void OnPointerEnter(PointerEventData eventData)
        {
             handController.setMenuItem(type);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            handController.setMenuItem(type);
        }

        // Use this for initialization
        void Start()
        {
            handController = HandControllers.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
