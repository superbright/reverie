using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencySetter : MonoBehaviour {

    public Material mat;
	// Use this for initialization
	void Start () {
        Color color = mat.color;
        color.a = 1.0f;
        GetComponent<Renderer>().material.color = color;
        mat.SetColor("_Albedo", Color.red);
    }
	
	// Update is called once per frame
	void Update () {
        Color color = mat.color;
       // color.a += 0.1f;
       // GetComponent<Renderer>().material.color = color;
    }
}
