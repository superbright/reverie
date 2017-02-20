using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Flock{
	[RequireComponent (typeof (FlockPopulator))]
	public class FlockManipulator_CentroidLerp : FlockManipulator_Centroid {

		public GameObject controller2;
		Vector3 previousPos;

		public List<Vector3> targets2;
		public List<float> targetLerp;

		float speed;

		ImageTools.Core.PerlinNoise PNoise;


		public override void Init () {

			noiseVecIn = Vector3.zero;
			PNoise = new ImageTools.Core.PerlinNoise (1);
			
			populator = GetComponent<FlockPopulator> ();
			targetLerp = new List<float>();
			targets = new List<Vector3> ();

			for (int i = 0; i < amount; i++) {
				targets.Add ( Random.insideUnitSphere * spread );

			}

			populator.Init ();

			for (int i = 0; i < amount; i++) {
				populator.boids [i].initialPosition = targets[i];
				targets2.Add ( targets[i]);
			}

		}

		void controllerSpeed(){
			speed = speed * 10;
			speed += Vector3.Distance (controller2.transform.position, previousPos);
			speed /= 11;
			Debug.Log (Vector3.Distance (controller2.transform.position, previousPos));
			previousPos = new Vector3(controller2.transform.position.x,controller2.transform.position.y,controller2.transform.position.z);
		}

		public override void Animate () {

//			controllerSpeed ();

			
			for (int i = 0; i < targets.Count; i++) {

					
				Vector3 newPos =   controller.transform.localToWorldMatrix.MultiplyVector (targets [i]) + controller.transform.position;
//				Vector3 newPos2 = controller2.transform.localToWorldMatrix.MultiplyVector (targets2 [i]) + controller2.transform.position;

//				populator.boids [i].target = Vector3.Lerp(newPos,newPos2,speed) + getNoiseVec (populator.boids [i].initialPosition);
				populator.boids [i].target = newPos + getNoiseVec (populator.boids [i].initialPosition);

				populator.boids [i].Animate ();
			}

		}


	}
}