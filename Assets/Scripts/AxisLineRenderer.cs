using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FRL.IO;
using System;

namespace SB.Seed
{
  
    public class AxisLineRenderer : MonoBehaviour
    {
        public enum AXIS
        {
            ALL,
            X,
            Y,
            Z,
            NONE
        }

        public GameObject parentObject;
        public Material mat;

        int range = 100;
        Color[] colors;

        //line renderers for axis
        List<LineRenderer> axis;

        //planes for collusion
        List<GameObject> collisionAxis;

       
        // Use this for initialization
        void Awake()
        {
            parentObject = new GameObject();
            parentObject.name = "axis";
            parentObject.transform.parent = transform;
            parentObject.transform.localPosition = Vector3.zero;
        }

        public void Init(Action<Vector3,AXIS> _moveGuide, Action<Vector3, AXIS> _moveObject)
        {
            axis = new List<LineRenderer>();
            collisionAxis = new List<GameObject>();

            int k = 1;
            foreach (Vector3[] obj in getAxis())
            {

                GameObject go = new GameObject();
                go.transform.parent = parentObject.transform;
                go.name = Enum.GetName(typeof(AXIS), k);
                LineRenderer axisrenderer = go.AddComponent<LineRenderer>();
                axisrenderer.SetWidth(0.1f, 0.1f);
                axisrenderer.SetPositions(obj);
                axisrenderer.useWorldSpace = false;
                axis.Add(axisrenderer);
                go.transform.localPosition = Vector3.zero;
                k++;
            }
            Material material = new Material(Shader.Find("Ciconia Studio/Double Sided/Transparent/Diffuse Bump"));
            material.color = Color.green;

            GameObject planex = GameObject.CreatePrimitive(PrimitiveType.Cube);
            planex.transform.localScale = new Vector3(50f, 0.01f, 0.8f);
            planex.transform.parent = axis[0].gameObject.transform;
            planex.transform.Rotate(90, 0, 0);
            Rigidbody bod = planex.AddComponent<Rigidbody>();
            bod.isKinematic = true;
            bod.useGravity = false;
            bod.detectCollisions = false;

            GameObject planey = GameObject.CreatePrimitive(PrimitiveType.Cube);
            planey.transform.localScale = new Vector3(50f, 0.8f, 0.01f);
            planey.transform.parent = axis[1].gameObject.transform;
            planey.transform.Rotate(0, 90, 90);
            Rigidbody bod1 =  planey.AddComponent<Rigidbody>();
            bod1.isKinematic = true;
            bod1.useGravity = false;
            bod1.detectCollisions = false;

            GameObject planez = GameObject.CreatePrimitive(PrimitiveType.Cube);
            planez.transform.localScale = new Vector3(50f, 0.8f, 0.01f);
            planez.transform.parent = axis[2].gameObject.transform;
            planez.transform.Rotate(0, 90, 0);
            Rigidbody bod2 = planez.AddComponent<Rigidbody>();
            bod2.isKinematic = true;
            bod2.useGravity = false;
            bod2.detectCollisions = false;

            planex.GetComponent<Renderer>().material = material;
            planey.GetComponent<Renderer>().material = material;
            planez.GetComponent<Renderer>().material = material;

            planex.transform.localPosition = Vector3.zero;
            planey.transform.localPosition = Vector3.zero;
            planez.transform.localPosition = Vector3.zero;

            GuideRuler rulerx = planex.AddComponent<GuideRuler>();
            GuideRuler rulery = planey.AddComponent<GuideRuler>();
            GuideRuler rulerz = planez.AddComponent<GuideRuler>();
            
            //rulerx
            rulerx.moveGuide = _moveGuide;
            rulerx.moveObject = _moveObject;
            rulery.moveGuide = _moveGuide;
            rulery.moveObject = _moveObject;
            rulerz.moveGuide = _moveGuide;
            rulerz.moveObject = _moveObject;

            rulerx.axis = AXIS.X;
            rulery.axis = AXIS.Y;
            rulerz.axis = AXIS.Z;

            collisionAxis.Add(planex);
            collisionAxis.Add(planey);
            collisionAxis.Add(planez);

          



        }

        /// <summary>
        /// Enable locked axis mode and collisions to mode objects on pointing planes
        /// </summary>
        /// <param name="_axis"></param>
        public void LockMode(AXIS _axis)
        {
            for (int k = 0; k <= axis.Count; k++)
            {
                if (_axis == AXIS.ALL)
                {
                    axis[k].gameObject.SetActive(true);
                    collisionAxis[k].SetActive(true);
                    collisionAxis[k].GetComponent<Rigidbody>().detectCollisions = false;
                }
                else if (k == ((int)_axis))
                {
                    axis[k - 1].gameObject.SetActive(true);
                    collisionAxis[k - 1].SetActive(true);
                    collisionAxis[k - 1].GetComponent<Rigidbody>().detectCollisions = true;
                }
                else if (k > 0)
                {
                    axis[k - 1].gameObject.SetActive(false);
                    collisionAxis[k - 1].SetActive(false);
                    collisionAxis[k - 1].GetComponent<Rigidbody>().detectCollisions = false;
                }
            }

          
    
        }

        private Vector3[][] getAxis()
        {

            return new Vector3[][] {
            new Vector3 [] { new Vector3(-range,0,0),new Vector3(range,0,0) } ,
            new Vector3 [] { new Vector3(0,-range,0),new Vector3(0,range,0) },
            new Vector3 [] { new Vector3(0,0,-range),new Vector3(0,0,range) } };

        }

        private void refresh()
        {

            int i = 0;

            foreach (Vector3[] obj in getAxis())
            {
                LineRenderer lr = (LineRenderer)axis[i];
                lr.SetPositions(obj);

            }
        }

    }
}


