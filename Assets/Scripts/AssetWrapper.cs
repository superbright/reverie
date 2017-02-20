using UnityEngine;
using System.Collections;
using FRL.IO;
using System;

namespace SB.Seed
{

    public class AssetWrapper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        public GameObject selectionObject;
        public GameObject movementGuide;
        public GameObject visualCue;
        public GameObject content;
        public bool isHolding = false;
        public bool isLocked = false;

        Vector3 center = Vector3.zero;

        public void OnPointerEnter(PointerEventData eventData)
        {
                selectionObject.SetActive(true);
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isHolding)
            {
               selectionObject.SetActive(false);
            }
        }

        /// <summary>
        /// Move the visual guide to the targetted position
        /// </summary>
        /// <param name="_pos"></param>
        public void moveGuide(Vector3 _pos,AxisLineRenderer.AXIS _axis)
        {

            Vector3 pos = transform.position;
            switch(_axis)
            {
                case AxisLineRenderer.AXIS.X:
                    pos.x = _pos.x;
                    break;
                case AxisLineRenderer.AXIS.Y:
                    pos.y = _pos.y;
                    break;
                case AxisLineRenderer.AXIS.Z:
                    pos.z = _pos.z;
                    break;
            }
            
            visualCue.transform.localPosition = transform.InverseTransformPoint(pos);
        }

        /// <summary>
        /// Move the visual guide to the targetted position
        /// </summary>
        /// <param name="_pos"></param>
        public void movObject(Vector3 _pos, AxisLineRenderer.AXIS _axis)
        {

            Vector3 pos = transform.position;
            switch (_axis)
            {
                case AxisLineRenderer.AXIS.X:
                    pos.x = _pos.x;
                    break;
                case AxisLineRenderer.AXIS.Y:
                    pos.y = _pos.y;
                    break;
                case AxisLineRenderer.AXIS.Z:
                    pos.z = _pos.z;
                    break;
            }
           
            transform.position = pos;
            visualCue.transform.localPosition = movementGuide.transform.localPosition;
        }

        // Use this for initialization
        public void Init()
        {

            Bounds bounds = new Bounds(this.transform.position, Vector3.zero);
            // Renderer[] renderers = GetComponentsInChildren<Renderer>();

            int k = 0;
            foreach (Renderer renderer in content.GetComponentsInChildren<Renderer>())
            {
                if (renderer.name == "seedwrapper")
                    continue;

                if (k == 0) { 
                bounds = new Bounds(this.transform.position, Vector3.zero);
            }

                bounds.Encapsulate(renderer.bounds);
                k++;
            }

            BoxCollider collider = gameObject.GetComponent<BoxCollider>();
            if (collider == null)
                collider = gameObject.AddComponent<BoxCollider>();

            collider.size = bounds.size;
            selectionObject.transform.localScale = bounds.size;

            Vector3 localCenter = bounds.center - this.transform.position;

            collider.center = localCenter;
            selectionObject.transform.localPosition = localCenter;
            movementGuide.transform.localPosition = localCenter;

            // selectionObject.SetActive(false);
            AxisLineRenderer axis = movementGuide.AddComponent<AxisLineRenderer>();     
            axis.transform.localPosition = localCenter;
            axis.Init((Vector3 _pos, AxisLineRenderer.AXIS _axis) => {
                moveGuide(_pos,_axis);
            }, (Vector3 _pos, AxisLineRenderer.AXIS _axis) => {
                movObject(_pos, _axis);
            });

            center = localCenter;

            visualCue.transform.localPosition = localCenter;

        }

        private void CalculateLocalBounds()
        {
            Quaternion currentRotation = this.transform.rotation;
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            Bounds bounds = new Bounds(this.transform.position, Vector3.zero);

            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }

            Vector3 localCenter = bounds.center - this.transform.position;
            bounds.center = localCenter;
            Debug.Log("The local bounds of this model is " + bounds);

            this.transform.rotation = currentRotation;
        }
    }

}
