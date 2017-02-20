using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Flock{
	[RequireComponent (typeof (FlockManipulator))]
	public class FlockPopulator : MonoBehaviour {

		public FlockBoid[] boidPrefabs;
		public List<FlockBoid> boids;
		FlockManipulator manip;

		public void Init () {
			boids = new List<FlockBoid> ();
			manip = GetComponent<FlockManipulator> ();
			int w = 0;
			for (int i = 0; i < manip.targets.Count; i++) {
				FlockBoid b = Instantiate (boidPrefabs [w]);
				b.Init ();
				boids.Add(b);
				boids [i].gameObject.name = boidPrefabs [w].gameObject.name;
				w++;
				if (w > boidPrefabs.Length-1)
					w = 0;
				boids [i].transform.SetParent (manip.container.transform);
			}
		}
	}
}