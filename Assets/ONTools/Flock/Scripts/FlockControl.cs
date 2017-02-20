using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Flock{
	public class FlockControl : MonoBehaviour {

		public List<FlockManipulator> manipulators;

		// Use this for initialization
		void Start () {

			for (int i = 0; i < manipulators.Count; i++) {
				manipulators [i].Init ();
			}
		}
		
		// Update is called once per frame
		void Update () {
			for (int i = 0; i < manipulators.Count; i++) {
				if(manipulators[i]!=null)
					manipulators [i].Animate ();
			}
		}
	}
}
