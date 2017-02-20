using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Flock{
	public class FlockManipulator : MonoBehaviour {

		public List<Vector3> targets;
		public int amount;
		public GameObject container;
		public virtual void Init () {
		}
		public virtual void Animate () {
		}
	}
}