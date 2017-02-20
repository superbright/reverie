using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB.Seed
{

    public class TriggerMenuButton : MonoBehaviour
    {

        public GameObject parentTranform;
        public GameObject[] prefabs;
        public ArrayList onSceneObjects;

        // Use this for initialization
        void Start()
        {
            onSceneObjects = new ArrayList();
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
            objcreative.transform.parent = parent.transform;
            objcreative.transform.localPosition = Vector3.zero;
            wrapper.content = objcreative;
 
            onSceneObjects.Add(parent);
            wrapper.Init();

            foreach (GameObject obj in onSceneObjects)
            {
                AudioSource audio = obj.GetComponentInChildren<AudioSource>();
                if (audio != null)
                {
                    audio.Stop();
                    audio.Play();
                }

            }
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