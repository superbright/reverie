using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB.Seed
{

    public class ReverieTimelineRecorder : MonoBehaviour
    {

        public string objectid;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (transform.hasChanged)
            {
             
                WorldContentManager.Instance.EditObject(new ObjectData(new BasicTranform(transform),objectid));

                transform.hasChanged = false;
            }

        }
    }
}