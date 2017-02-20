using UnityEngine;
using System.Collections;

public class MeshCollisionDetector : MonoBehaviour {

	public int xoffset = 5;
	public int yoffset = 5;

	public Grid locationgrid;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other) {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.up, out hit))
		{
			Debug.Log("Point of contact: "+hit.point);
		}
	}

	void OnCollisionEnter(Collision collision) {

		foreach (ContactPoint contact in collision.contacts) {
		
			if (locationgrid != null) {
				//locationgrid.generate (Mathf.FloorToInt(contact.point.x) + xoffset, Mathf.FloorToInt(contact.point.z) + yoffset);
			}
			//Debug.DrawRay(contact.point, contact.normal, Color.blue);
		}


	}
}
