using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationConfig : MonoBehaviour {


    private static ApplicationConfig _instance;
    public static ApplicationConfig Instance { get { return _instance; } }


    public Color MENUCOLOR = Color.white;
    public Color MENULOADINGCOLOR = Color.red;
    public Color MENUHOVERCOLOR = Color.blue;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
