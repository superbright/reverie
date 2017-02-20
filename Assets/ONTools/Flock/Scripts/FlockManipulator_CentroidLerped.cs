using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Flock{
	
	[RequireComponent (typeof (FlockPopulator))]
	public class FlockManipulator_CentroidLerped : FlockManipulator {

		public float spread;
		public FlockPopulator populator;
		public GameObject controller;

		//noise

		public float noiseMultiply;
		public float noiseScale;
		public float noiseSpeed;
		float noiseCounter;

		public Vector3 noiseVecIn;

		float off;

		ImageTools.Core.PerlinNoise PNoise;

		//*noise

		public GameObject controller2;
		Vector3 previousPos;

		public List<Vector3> targets2;
		public List<float> targetLerp;

		float speed;

		public float distanceMultiplier;


		public override void Init () {

			off = .5f;
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
				targets2.Add (targets[i] );
				targetLerp.Add (0);
			}

			Debug.Log (targets.Count);

		}

		void controllerSpeed(){
			speed = speed * 1000;
			speed += Vector3.Distance (controller2.transform.position, previousPos) * distanceMultiplier;
			speed /= 1005;
			previousPos = new Vector3(
				controller2.transform.position.x,
				controller2.transform.position.y,
				controller2.transform.position.z);
		}


		public override void Animate () {
			
			controllerSpeed ();

			for (int i = 0; i < targets.Count; i++) {

				float clampSpeed = Mathf.Clamp (speed, 0, 1) * targets.Count;

				if (i < clampSpeed && i + 1 < clampSpeed)
					targetLerp [i] = 1;
				else if (i < clampSpeed && i + 1 > clampSpeed)
					targetLerp [i] = clampSpeed - i;
				else if (i > clampSpeed)
					targetLerp [i] = 0;

//				Debug.Log (targetLerp.Count+" , "+ targets.Count);
//				Debug.Log (clampSpeed);
//				Debug.Log (targetLerp[i]);


				Vector3 newPos = controller.transform.localToWorldMatrix.MultiplyVector (targets [i]) + controller.transform.position;
				Vector3 newPos2 = controller2.transform.localToWorldMatrix.MultiplyVector (targets2 [i]) + controller2.transform.position;
				populator.boids [i].target = Vector3.Lerp(newPos,newPos2,targetLerp[i]) + getNoiseVec (populator.boids [i].initialPosition);
				populator.boids [i].Animate ();
			}

		}

		public Vector3 getNoiseVec(Vector3 newPos){
			
			float scale = noiseMultiply;
			float wScale = noiseScale;
			noiseCounter += noiseSpeed * Time.deltaTime;

			noiseVecIn.Set(
				scale * (float)PNoise.Noise (wScale * newPos.x + noiseCounter + off, noiseCounter + newPos.y * wScale, noiseCounter + wScale * newPos.z),
				scale * (float)PNoise.Noise (wScale * newPos.x + noiseCounter, wScale * newPos.y + noiseCounter + off, noiseCounter + wScale * newPos.z),
				scale * (float)PNoise.Noise (wScale * newPos.x + noiseCounter, wScale * newPos.y + noiseCounter, noiseCounter + wScale * newPos.z + off));
				
			return noiseVecIn;
		}
	}
}