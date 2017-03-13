using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB.Seed
{

    public class ObjectFactory : MonoBehaviour
    {

        public GameObject parentTranform;
        public GameObject[] prefabs;
        public List<GameObject> onSceneObjects;
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
            triggerPrefab(index, Vector3.zero,index.ToString());
        }

        public void resetScene()
        {
            Debug.Log("reset");
            foreach(GameObject g in onSceneObjects)
            {
                Debug.Log(g.name);
                Destroy(g);
            }
            onSceneObjects = new List<GameObject>();
        }

        public void triggerPrefab(int index, Vector3 location,string serverid)
        {

            GameObject parent = Instantiate(parentTranform, location, Quaternion.identity);
            AssetWrapper wrapper = parent.GetComponent<AssetWrapper>();
            //TODO: organize these better
            // 9 is objects layer
            parent.layer = 9;

            GameObject objcreative = Instantiate(prefabs[index], Vector3.zero, Quaternion.identity);
            objcreative.name = index.ToString();
            objcreative.transform.parent = parent.transform;
            objcreative.transform.localPosition = Vector3.zero;
            wrapper.content = objcreative;
 
            onSceneObjects.Add(parent);
            wrapper.Init();

            parent.transform.parent = this.parentScene;

            ReverieTimelineRecorder recorder = parent.GetComponentInChildren<ReverieTimelineRecorder>();
            recorder.objectid = serverid;

         
        }

        public void loadObjectFromPlanet(string id, BasicTranform savedtransform)
        {
            GameObject parent = Instantiate(parentTranform, savedtransform.position, Quaternion.identity);
            AssetWrapper wrapper = parent.GetComponent<AssetWrapper>();
            //TODO: organize these better
            // 9 is objects layer
            parent.layer = 9;

            GameObject objcreative = Instantiate(prefabs[int.Parse(id)], Vector3.zero, Quaternion.identity);
            objcreative.name = id;
            objcreative.transform.parent = parent.transform;
            objcreative.transform.localPosition = Vector3.zero;
            wrapper.content = objcreative;

            onSceneObjects.Add(parent);
            wrapper.Init();

            parent.transform.parent = this.parentScene;
            parent.transform.localScale = savedtransform.scale;
            parent.transform.rotation = Quaternion.Euler(savedtransform.rotation);

            ReverieTimelineRecorder recorder = parent.GetComponentInChildren<ReverieTimelineRecorder>();
            recorder.objectid = id;

           // WorldContentManager.Instance.AddObject(objcreative.name, recorder);
        }

        public void deletePrefab(GameObject foundobj)
        {

            for (int i = 0; i < onSceneObjects.Count; i++)
            {
                if ((GameObject)onSceneObjects[i] == foundobj)
                {
                    onSceneObjects.RemoveAt(i);
                    Destroy(foundobj);
                }
            }

        }
    }
}