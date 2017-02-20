using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SearchHandler : MonoBehaviour {

	//search inputfield to parse
	public InputField search;
	WorldContentManager worldManager;

	// Use this for initialization
	void Start () {
		search.onValueChange.AddListener (delegate {ValueChangeCheck ();});
		worldManager = FindObjectOfType<WorldContentManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ValueChangeCheck()
	{
		Debug.Log ("Value Changed");
		worldManager.Draw2DUIMenu (worldManager.worldData.searchVertices (search.text));
	}
}
