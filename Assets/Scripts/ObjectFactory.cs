using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB.Seed
{

    public class ObjectFactory : MonoBehaviour
    {

        public GameObject parentTranform;
        public GameObject[] prefabs;
        public List<ReverieObject> onSceneObjects;
        public Transform parentScene;

        private static ObjectFactory _instance;
        public static ObjectFactory Instance { get { return _instance; } }

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        // Use this for initialization
        void Start()
        {
            //onSceneObjects = new List<GameObject>();
        }

        public void triggerPrefabAtZero(int index)
        {
            //triggerPrefab(index, Vector3.zero,index.ToString());
        }

        public void resetScene()
        {
           
            foreach(ReverieObject g in onSceneObjects)
            {
                Debug.Log(g._id);
                Destroy(g.obj);
            }

            onSceneObjects = new List<ReverieObject>();
        }

        public void triggerPrefab(int index, ReverieObject obj)
        {

            GameObject parent = Instantiate(parentTranform, obj.transform.position, Quaternion.identity);
            AssetWrapper wrapper = parent.GetComponent<AssetWrapper>();
            //TODO: organize these better
            // 9 is objects layer
            parent.layer = 9;

            GameObject objcreative = Instantiate(prefabs[index], Vector3.zero, Quaternion.identity);
            objcreative.name = index.ToString();
            objcreative.transform.parent = parent.transform;
            objcreative.transform.localPosition = Vector3.zero;
            wrapper.content = objcreative;

            obj.obj = parent;
            onSceneObjects.Add(obj);
            wrapper.Init();

            parent.transform.parent = this.parentScene;

            ReverieTimelineRecorder recorder = parent.GetComponentInChildren<ReverieTimelineRecorder>();
            recorder.objectid = obj._id;

         
        }

        public void loadObjectFromPlanet(ReverieObject obj)
        {
            //obj.assetid, obj.transform,obj._id.Split('/')[1]

            GameObject parent = Instantiate(parentTranform, obj.transform.position, Quaternion.identity);
            AssetWrapper wrapper = parent.GetComponent<AssetWrapper>();
            //TODO: organize these better
            // 9 is objects layer
            parent.layer = 9;

            GameObject objcreative = Instantiate(prefabs[int.Parse(obj.assetid)], Vector3.zero, Quaternion.identity);
            objcreative.name = obj._id.Split('/')[1];
            objcreative.transform.parent = parent.transform;
            objcreative.transform.localPosition = Vector3.zero;
            wrapper.content = objcreative;

            obj.obj = parent;
            onSceneObjects.Add(obj);
            wrapper.Init();

            parent.transform.parent = this.parentScene;
            parent.transform.localScale = obj.transform.scale;
            parent.transform.rotation = Quaternion.Euler(obj.transform.rotation);

            ReverieTimelineRecorder recorder = parent.GetComponentInChildren<ReverieTimelineRecorder>();
            recorder.objectid = obj._id.Split('/')[1];

     
        }

        public void deletePrefab(ReverieObject foundobj)
        {

            for (int i = 0; i < onSceneObjects.Count; i++)
            {
                if (onSceneObjects[i] == foundobj)
                {
                    onSceneObjects.RemoveAt(i);
                    Destroy(foundobj.obj);
                }
            }

        }
    }
}