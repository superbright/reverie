using UnityEngine;
using System.Collections;

namespace Flock{
	public class FlockBoid_Simple : FlockBoid {

		Vector3 prevPos;
		public float lerpSpeed = .05f;

		// Use this for initialization
		public override void Init () {
			id = Random.value;
		}

		// Update is called once per frame
		public override void Animate () {
			this.transform.LookAt (target);
			this.transform.position = Vector3.Lerp(this.transform.position, prevPos,lerpSpeed);
			prevPos = target;
		}
	}
}
