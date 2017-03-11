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

        // Use this for initialization
        void Start()
        {
            onSceneObjects = new List<GameObject>();
        }

        public void triggerPrefabAtZero(int index)
        {
            triggerPrefab(0, Vector3.zero);
        }

        public void triggerPrefab(int index, Vector3 location)
        {

            GameObject parent = Instantiate(parentTranform, location, Quaternion.identity);
            AssetWrapper wrapper = parent.GetComponent<AssetWrapper>();
            //TODO: organize these better
            // 9 is objects layer
            parent.layer = 9;

            GameObject objcreative = Instantiate(prefabs[index], Vector3.zero, Quaternion.identity);
            objcreative.name = "object" + Random.Range(0, 100);
            objcreative.transform.parent = parent.transform;
            objcreative.transform.localPosition = Vector3.zero;
            wrapper.content = objcreative;
 
            onSceneObjects.Add(parent);
            wrapper.Init();

            parent.transform.parent = this.parentScene;

            ReverieTimelineRecorder recorder = parent.GetComponentInChildren<ReverieTimelineRecorder>();

            WorldContentManager.Instance.AddObject(objcreative.name, recorder);

        }

        public void loadObjectFromPlanet(string id, BasicTranform savedtransform)
        {
            GameObject parent = Instantiate(parentTranform, savedtransform.position, Quaternion.identity);
            AssetWrapper wrapper = parent.GetComponent<AssetWrapper>();
            //TODO: organize these better
            // 9 is objects layer
            parent.layer = 9;

            GameObject objcreative = Instantiate(prefabs[0], Vector3.zero, Quaternion.identity);
            objcreative.name = "object" + Random.Range(0, 100);
            objcreative.transform.parent = parent.transform;
            objcreative.transform.localPosition = Vector3.zero;
            wrapper.content = objcreative;

            onSceneObjects.Add(parent);
            wrapper.Init();

            parent.transform.parent = this.parentScene;
            parent.transform.localScale = savedtransform.scale;
            parent.transform.rotation = Quaternion.Euler(savedtransform.rotation);

            ReverieTimelineRecorder recorder = parent.GetComponentInChildren<ReverieTimelineRecorder>();

            WorldContentManager.Instance.AddObject(objcreative.name, recorder);
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