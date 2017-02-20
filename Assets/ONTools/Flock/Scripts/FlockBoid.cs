using UnityEngine;
using System.Collections;

namespace Flock{
	public class FlockBoid : MonoBehaviour {

		public Vector3 target;
		public Vector3 initialPosition;
		public float id;

		public virtual void Init(){
		}
		public virtual void Animate(){
		}
		
	}
}