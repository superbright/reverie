using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {




	public int xsize, ysize;
	public Vector3[] vertices;

	public int targetx = 0;
	public int targety = 0;


	private Mesh mesh;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI () {

		if(GUILayout.Button("generate")) {
			generate (targetx, targety);
		}
	}

	void Awake() {
		//xsize = (int)transform.parent.transform.localScale.x / 10;
		//ysize = (int)transform.parent.transform.localScale.z / 10;
		generate (10, 10);
			
	}



//	public void generate(int x, int y) {
//		Generate (x,y);
//	}

	/// <summary>
	/// Generate grid with a 0.005 s for eac vertice.
	/// </summary>
	public void generate(int _targetx, int _targety) {
		//WaitForSeconds wait = new WaitForSeconds (0.000f);

		//generate a new mesh and assign to the component
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedural Grid";

		vertices = new Vector3[(xsize + 1) * (ysize + 1)];

		//generate UV array
		Vector2[] uv = new Vector2[vertices.Length];

		for (int i = 0, y = 0; y <= ysize; y++) {
			for (int x = 0; x <= xsize; x++, i++) {
				vertices [i] = new Vector3 (x, y);
				uv[i] = new Vector2((float)x / xsize, (float)y / ysize);

			}
		}
		mesh.vertices = vertices;
		mesh.uv = uv;

		int[] triangles = new int[xsize * ysize * 6];
		for (int ti = 0, vi = 0, y = 0; y < ysize; y++, vi++) {
			for (int x = 0; x < xsize; x++, ti += 6, vi++) {
				if (x == _targetx && y == _targety) {
					
					triangles [ti] = vi;
					triangles [ti + 3] = triangles [ti + 2] = vi + 1;
					triangles [ti + 4] = triangles [ti + 1] = vi + xsize + 1;
					triangles [ti + 5] = vi + xsize + 2;
				}
				//yield return wait;
				mesh.triangles = triangles;
				mesh.RecalculateNormals();
			}
		}

		targetx = _targetx;
		targety = _targety;

		//yield return wait;
	}

	/// <summary>
	/// Raises the draw gizmos event.
	/// </summary>
	private void OnDrawGizmos() {
		
		if (vertices == null)
			return;

		//visualize the vertices
		//Gizmos.color = Color.red;
		//for( int i = 0; i < vertices.Length; i++) {
		//	Gizmos.DrawSphere(vertices[i], 0.1f);

		//}
	}
}
