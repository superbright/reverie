using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Flock{
	[RequireComponent (typeof (FlockPopulator))]
	public class FlockManipulator_Centroid : FlockManipulator {

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


		public override void Init () {

			off = .5f;
			noiseVecIn = Vector3.zero;
			PNoise = new ImageTools.Core.PerlinNoise (1);
			
			populator = GetComponent<FlockPopulator> ();

			for (int i = 0; i < amount; i++) {
				targets.Add ( Random.insideUnitSphere * spread );
			}

			populator.Init ();

			for (int i = 0; i < amount; i++) {
				populator.boids [i].initialPosition = targets[i];
			}

		}

		public override void Animate () {
			
			for (int i = 0; i < targets.Count; i++) {
				Vector3 newPos = controller.transform.localToWorldMatrix.MultiplyVector (targets [i]) + controller.transform.position;
				populator.boids [i].target = newPos + getNoiseVec (populator.boids [i].initialPosition);
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