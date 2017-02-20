using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldAsset
{
    public GameObject obj;
}

/// <summary>
/// Some global settings are done here
/// </summary>
public class WorldManager : MonoBehaviour {


    public List<WorldAsset> worldObjects;

	// Use this for initialization
	void Start () {
        Physics.IgnoreLayerCollision(9, 10, true);
        Physics.IgnoreLayerCollision(11, 12, true);
        Physics.IgnoreLayerCollision(9, 9, true);
    }

    public void NewWorld()
    {
        foreach(WorldAsset asset in worldObjects)
        {
            Destroy(asset.obj);
        }

    }

    void ParseAvailableData() {

    }
}
