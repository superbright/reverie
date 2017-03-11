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

    /// <summary>
    /// Object transform to pass back to socket, must abstract and have different types
    /// </summary>
    [Serializable]
    public class ObjectData
    {
        public ObjectData(BasicTranform transform, string objectid)
        {
            this.objectid = objectid;
            this.transform = transform;

        }
        public string objectid;
        public string planetid;
        public BasicTranform transform;

         
    }
    [Serializable]
    public class BasicTranform
    {
        public BasicTranform(Transform transform)
        {
            position = transform.position;
            scale = transform.localScale;
            rotation = transform.rotation.eulerAngles;
        }
        public Vector3 position;
        public Vector3 scale;
        public Vector3 rotation;
    }

    
}
