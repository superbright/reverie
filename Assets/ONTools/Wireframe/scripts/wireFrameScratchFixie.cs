using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshFilter))]

public class wireFrameScratchFixie : MonoBehaviour {

	MeshRenderer MRend;
	MeshFilter MFilter;
	Mesh MMesh;
	
	Renderer rend;
	public bool frontFaces = true;
	public float lineWidth = 1;
	public float frontFaceLineWidthMult = 1;
	public float speed;
	public float[] colors;
	public float colorRandomize = .05f;
	public float saturation=1;
	public float brightness=1;
	
	public Material lineMat;
	Material privateLineMat;
	public Texture2D lineTex;
	public Texture2D shadowTex;
	Mesh mesh;
	Hashtable edges;
	Hashtable Verts;
	Hashtable VertNormCheck;
	public float textureTile = 100;
	
	Texture2D texture;
	Texture2D posTexture;
	
	List<Vector3> vertPositions;
	
	public GameObject wireFrame;
	public float normalOffset = .1f;
	
	public float pointSize = .5f;
	public Color pointColor = Color.white;
	
	float[] distances;
	
	private const int h1 = 12178051;
	private const int h2 = 12481319;
	private const int h3 = 15485863;
	
	public bool reposition = true;
	int pointCount;
	
	Vector3[] verts;
	int[] faces;
	
	Matrix4x4 prevMat;
	Vector3 prevScale;
	
	float prevNormalOffset;
	
	public float shadowSpeed = 1;
	public float shadowTile = 1;
	
	public float hueShift;
	public float minEdgeDistance = .0001f;
	public float maxEdgeDistance = 1e6f;
	
	// Use this for initialization
	void Start () {

		vertPositions = new List<Vector3>();
		
		MRend = GetComponent<MeshRenderer> ();
		MFilter = GetComponent<MeshFilter> ();
		privateLineMat = Instantiate (lineMat);
		MRend.sharedMaterial = privateLineMat;
		rend = GetComponent<Renderer> ();
		
		if (colors.Length < 1)
		colors = new float[]{.5f};
		
		edges = new Hashtable ();
		Verts = new Hashtable ();
		VertNormCheck = new Hashtable ();

		mesh = wireFrame.GetComponent<MeshFilter> ().mesh;
		
		texture = new Texture2D (100, 1, TextureFormat.RGBAFloat,false);
		texture.filterMode = FilterMode.Point;
		posTexture = new Texture2D (100, 1, TextureFormat.RGBAFloat,false);
		posTexture.filterMode = FilterMode.Point;
		privateLineMat.SetFloat("_Tile",textureTile);
		rend.sharedMaterial = privateLineMat;
		if (lineTex == null)
			lineTex = privateLineMat.GetTexture ("_SpriteTex") as Texture2D;
		if (shadowTex == null)
			shadowTex = lineTex;
		rend.sharedMaterial.SetTexture ("_MainTex", texture);
		rend.sharedMaterial.SetTexture ("_SpriteTex", lineTex);
		rend.sharedMaterial.SetTexture ("_ShadowTex", shadowTex);
		rend.sharedMaterial.SetTexture ("_PosTex", posTexture);
		
		verts = mesh.vertices;
		faces = mesh.triangles;

//		for (int i = 0; i < mesh.vertices.Length; i++) {
//			print (mesh.vertices[i]);
//		}
		
		makeEdges ();
		makeSimpleLines ();
		//		SetPoints (particlePos);
		makeTexture ();
		
	}
	
	void makeSimpleLines(){
		//
		int pnts = 4;
		if (frontFaces)
			pnts = 8;
		pointCount = edges.Count * pnts;
		
		int count = -1;
		distances = new float[edges.Count];
		int dCount = -1;
		int pCount = -1;
		int tCount = -1;
		int triCount = 0;
		
		int particleCount = -1;
		Verts.Clear ();
		
		if (MMesh == null){
			MFilter.mesh = new Mesh();
			MMesh = MFilter.sharedMesh;
		}
		MMesh.Clear();
		MMesh.vertices = new Vector3[pointCount];
		MMesh.triangles = new int[pointCount*3];
		MMesh.uv = new Vector2[pointCount];
		MMesh.uv2 = new Vector2[pointCount];
		
		Vector2[] uvs = MMesh.uv;
		Vector2[] uvs2 = MMesh.uv2;
		Vector3[] vs = new Vector3[pointCount];
		int[] tris = new int[pointCount * 3];
		
		
		foreach (DictionaryEntry entry in edges) {
			
			int[] b = entry.Value as int[];
			
			Vector3 p = verts [b [0]];
			Vector3 s = verts [b [1]];
			
			//make verts to instantiate dots
			if (!Verts.ContainsKey (getHashedCell (p))) {
				Vector3 qp = p;
				qp+=mesh.normals[b[0]].normalized*normalOffset;
				qp = wireFrame.transform.localToWorldMatrix.MultiplyPoint (qp);
				Verts.Add (getHashedCell (p), new float[]{qp.x,qp.y,qp.z});
				++particleCount;
			}
			
			
			if(reposition){
				p = wireFrame.transform.localToWorldMatrix.MultiplyPoint (p);
				s = wireFrame.transform.localToWorldMatrix.MultiplyPoint (s);
			}
			distances [++dCount] = Vector3.Distance (p, s);
			
			
			float off = (float) 1/(edges.Count);
			float offset = (float)dCount*off;


			Vector3 start = new Vector3(offset+off*.3f, -1,0);
			Vector3 start2 = new Vector3(offset+off*.3f,1,0);
			Vector3 end = new Vector3(offset+off*.7f, -1,0);
			Vector3 end2 = new Vector3(offset+off*.7f,1,0);

			Vector2[] uv2v = new Vector2[]{
			new Vector2(0,0),
			new Vector2(0,1),
			new Vector2(1,0),
			new Vector2(1,1)};

			Vector3 pN = mesh.normals[b[0]].normalized*normalOffset;
			Vector3 sN = mesh.normals[b[1]].normalized*normalOffset;

			vs[++count] = p-mesh.normals[b[0]].normalized*lineWidth + pN;
			uvs[count] = new Vector2(offset+off*.1f,0);
			uvs2[count] = uv2v[0];
			vs[++count] =  p+mesh.normals[b[0]].normalized*lineWidth +pN;
			uvs[count] = new Vector2(offset+off*.1f,1);
			uvs2[count] = uv2v[1];

			vs[++count] = s-mesh.normals[b[1]].normalized*lineWidth + sN;
			uvs[count] = new Vector2(offset+off*.2f,0);
			uvs2[count] = uv2v[2];
			vs[++count] = s+mesh.normals[b[1]].normalized*lineWidth + sN;
			uvs[count] = new Vector2(offset+off*.2f,1);
			uvs2[count] = uv2v[3];

			Vector3 vec1 = Vector3.LerpUnclamped(p,s,-1f);
			Vector3 vec2 = p;
			Vector3 vec3 = s;
			Vector3 vec4 = Vector3.LerpUnclamped(p,s,2f);

			vertPositions.Add(vec4);
			vertPositions.Add(vec1);
			vertPositions.Add(vec2);
			vertPositions.Add(vec3);


			tris[++tCount] = triCount+0;
			tris[++tCount] = triCount+1;
			tris[++tCount] = triCount+3;

			tris[++tCount] = triCount+0;
			tris[++tCount] = triCount+3;
			tris[++tCount] = triCount+2;

			triCount+=4;


			if(frontFaces){
			
				vs[++count] = p-Vector3.Cross(mesh.normals[b[0]].normalized*lineWidth,p.normalized-s.normalized)*frontFaceLineWidthMult+pN;
				uvs[count] = new Vector2(offset+off*.1f,0);
				uvs2[count] = uv2v[0];
				vs[++count] =  p+Vector3.Cross (mesh.normals[b[0]].normalized*lineWidth,p.normalized-s.normalized)*frontFaceLineWidthMult+pN;
				uvs[count] = new Vector2(offset+off*.1f,1);
				uvs2[count] = uv2v[1];

				vs[++count] = s-Vector3.Cross(mesh.normals[b[1]].normalized*lineWidth,p.normalized-s.normalized)*frontFaceLineWidthMult+sN;
				uvs[count] = new Vector2(offset+off*.2f,0);
				uvs2[count] = uv2v[2];
				vs[++count] = s+Vector3.Cross (mesh.normals[b[1]].normalized*lineWidth,p.normalized-s.normalized)*frontFaceLineWidthMult+sN;
				uvs[count] = new Vector2(offset+off*.2f,1);
				uvs2[count] = uv2v[3];
				
			
				

				
				vertPositions.Add(vec4);
				vertPositions.Add(vec1);
				vertPositions.Add(vec2);
				vertPositions.Add(vec3);
				
				
				tris[++tCount] = triCount+0;
				tris[++tCount] = triCount+1;
				tris[++tCount] = triCount+3;
				
				tris[++tCount] = triCount+0;
				tris[++tCount] = triCount+3;
				tris[++tCount] = triCount+2;
				
				triCount+=4;
			}

			
		}
		
		
		MMesh.vertices = vs;
		MMesh.triangles = tris;
		MMesh.uv = uvs;
		MMesh.uv2 = uvs2;
		
		MMesh.RecalculateNormals();
		MMesh.RecalculateBounds();
		//
		
	}
	
	void makeEdges(){
		edges.Clear ();
		int q = 0;
		int qc = 0;
		for(int i = 0 ; i < faces.Length ; i++){
			if(q<faces.Length){
				if(!edges.ContainsKey(hasher(verts, faces[q],faces[q+1]))){
					if(!edges.ContainsKey((hasher(verts, faces[q+1],faces[q])))){
						float dist = Vector3.Distance (verts[faces[q]],verts[faces[q+1]]);
						if(dist>minEdgeDistance&&dist<maxEdgeDistance)
							edges.Add(hasher(verts, faces[q],faces[q+1]), new int[]{faces[q],faces[q+1]});
					}
				}
				
				q++;
				qc++;
				if(q>0&&qc==2){
					if(!edges.ContainsKey(hasher(verts, faces[q],faces[q-2]))){
						if(!edges.ContainsKey(faces[(q-2)]+","+faces[q])){
							float dist = Vector3.Distance (verts[faces[q-2]],verts[faces[q]]);
							if(dist>minEdgeDistance&&dist<maxEdgeDistance)
								edges.Add(hasher(verts, faces[q],faces[q-2]), new int[]{faces[q],faces[q-2]});
							
							
						}
					}
				}
				
				if(qc>1){
					qc=0;
					q++;
				}
			}
			
		}
	}
	
	
	float frac(float t){
		return t-Mathf.Floor(t);
	}
	
	public void makeTexture(){
		
		int detail = edges.Count;
		//		detail -= 1;
		texture.Resize ((int)detail, 1);
		texture.filterMode = FilterMode.Point;
		posTexture.Resize ((int)detail*4, 1);
		posTexture.filterMode = FilterMode.Point;
		int on = 0;
		int off = 0;
		int q = -1;
		Vector3 vp;
		
		print (detail * 4);
		for (int i = 0; i < detail; i++) {
			
			float colA = colors[(int)Mathf.Floor(Random.value*colors.Length)];
			float colC = colA+Random.Range(-colorRandomize,colorRandomize);// Mathf.Lerp(colA,colB,(float)i/detail);
			texture.SetPixel (i,0,
			                  new Color(
				Vector3.Distance(vertPositions[i*4+2],vertPositions[i*4+3]),
				(1+Mathf.Sin ((float)i/detail-1)*Mathf.PI)/2,
				colC,
				1));
			
		}
		for (int i = 0; i < (int)detail*4; i++) {
			vp = vertPositions[i];
			posTexture.SetPixel(i,0,new Color(vp.x,vp.y,vp.z,.1f));
		}
		
		texture.Apply ();
		posTexture.Apply ();
		
	}
	
	public int hasher(Vector3[] verts, int a, int b){
		return getHashedCell(verts[a])+getHashedCell(verts[b]);
	}
	public int getHashedCell(Vector3 pos) {
		int x = Mathf.FloorToInt (pos.x / .1f);
		int y = Mathf.FloorToInt (pos.y / .1f);
		int z = Mathf.FloorToInt (pos.z / .1f);
		return x * h1 + y * h2 + z * h3;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		MRend.sharedMaterial.SetFloat ("_Tile", textureTile);
		MRend.sharedMaterial.SetFloat ("_Speed", speed);
		MRend.sharedMaterial.SetFloat ("_Saturation", saturation);
		MRend.sharedMaterial.SetFloat ("_Brightness", brightness);
		MRend.sharedMaterial.SetFloat ("_ShadowSpeed", shadowSpeed);
		MRend.sharedMaterial.SetFloat ("_ShadowTile", shadowTile);
		MRend.sharedMaterial.SetFloat ("_UNPnts", edges.Count*4);
		MRend.sharedMaterial.SetFloat ("_LineWidth", lineWidth);
		MRend.sharedMaterial.SetFloat ("_HueShift", hueShift);
		
//			Transform camTransform = Camera.main.transform;
//			float distToCenter = (Camera.main.farClipPlane - Camera.main.nearClipPlane) / 2.0f;
//			Vector3 center = camTransform.position + camTransform.forward * distToCenter;
//			float extremeBound = 5000.0f;
//			MeshFilter filter = GetComponent<MeshFilter> ();
//			filter.sharedMesh.bounds = new Bounds (center, Vector3.one * extremeBound);
		
	}
}
