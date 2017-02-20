using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class WorldContentManager : MonoBehaviour {

	public static string MENUBUTTON = "GenericMenuButton";
	public GameObject scrollView;
	public graphUniverse worldData;
	//GraphController mygraph;


	void Start() {
		//mygraph = FindObjectOfType<GraphController> ();
		GetPlanets ();
	}

	public void Draw2DUIMenu(graphVertex[] data) {

		foreach (Transform child in scrollView.transform) {
			GameObject.Destroy(child.gameObject);
		}

		foreach (graphVertex v in data) {
            Debug.Log(v);
			GameObject go = (GameObject)Instantiate(Resources.Load("GenericMenuButton")); 
			go.transform.SetParent (scrollView.transform,false);
            Vector3 pos = go.transform.GetComponent<RectTransform>().position;
            pos.z = 0;
           // go.transform.GetComponent<RectTransform>().position = pos;
            go.transform.localScale = new Vector3(1, 1, 1);
		}
	}

	public void AddObjectsToMenu() {
		
	}

	public void AddPlanet() {
		NetworkHandler restapi = GetComponentInChildren<NetworkHandler> ();
		StartCoroutine(restapi.LoadStuff(restapi.methodforCall("POST"), NetworkHandler.CREATE + "/universe/multiverse", (data) => { 
			Debug.Log(data); 
			GetPlanets();
		}));

	}

	public void GetPlanets() {
		NetworkHandler restapi = GetComponentInChildren<NetworkHandler> ();
		StartCoroutine(restapi.LoadStuff(restapi.methodforCall("GET"), NetworkHandler.GRAPH + "/multiverse", (data) => {
            Debug.Log(data);
			worldData = JsonUtility.FromJson<graphUniverse>(data);

			Draw2DUIMenu (worldData.vertices);

			//mygraph.ResetWorld ();

			float i = 0.01f;
			foreach (graphPlanet v in worldData.vertices) {

				StartCoroutine(addNodeToGraph(v,i));
				i += 0.02f;
			}
		}));
	}

	IEnumerator addNodeToGraph(graphPlanet v, float delay)
	{
		//This is a coroutine

		yield return new WaitForSeconds(delay);
		//mygraph.GenerateNode (v.name, v._id, v._rev);

	}
}

[Serializable] 
public class graphPlanet : graphVertex {
	public string name;
}

[Serializable]
public class graphUniverse : graphData <graphEdge,graphPlanet>{

	public graphVertex[] searchVertices(string text) {

		return Array.FindAll (vertices, (graphVertex) => {
			Debug.Log(graphVertex.name);
			if(graphVertex.name !=null)
				return graphVertex.name.Contains(text);
			else 
				return false;
		});
	}
}

[Serializable]
public class graphEdge {
	string _key; //"_key": "792613",
	string _id; //"_id": "contains/792613",
	string _from; //"_from": "multiverse/791907",
	string _to; //"_to": "universe/792601",
	string data;
}

[Serializable] 
public class graphVertex {
	public string _key;
	public string _id; //"_id": "multiverse/791907",
	public string _rev; //"_rev": "_UUoees6---",
}

[Serializable]
public class graphPath {
	public graphEdge[] edges;
	public graphVertex[] vertices;

}

[Serializable]
public abstract class graphData<T,Y> {
	public  T[] paths { get; set; }
	public  Y[] vertices;

}
	
