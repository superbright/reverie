using UnityEngine;
using System.Collections;

namespace Flock{
	public class FlockBoid_Bird : FlockBoid {

		Vector3 prevPos;
		public float lerpSpeed;

		public Vector2 lerpMinMax;

		public GameObject wingLeft;
		public GameObject wingRight;
		public float flapSpeed;
		public float flapAmount;

		Vector3 leftFlap;
		Vector3 rightFlap;
	
		// Use this for initialization
		public override void Init () {
			id = Random.value;
			lerpSpeed = Random.Range (lerpMinMax.x, lerpMinMax.y);
		}
		
		// Update is called once per frame
		public override void Animate () {

			leftFlap.Set (0, 0,  flapAmount * Mathf.Sin (10*id + Time.time * flapSpeed) *  Mathf.Sin(10*id+(Time.time*flapSpeed*.1f)));
			rightFlap.Set (0, 0, flapAmount * Mathf.Sin (10*id + Time.time * flapSpeed) * -Mathf.Sin(10*id+(Time.time*flapSpeed*.1f)));

			wingLeft.transform.localEulerAngles = leftFlap;
			wingRight.transform.localEulerAngles = rightFlap;

			this.transform.LookAt (target);
			this.transform.position = Vector3.Lerp(this.transform.position, prevPos,lerpSpeed);
			prevPos = target;
		}
	}
}
