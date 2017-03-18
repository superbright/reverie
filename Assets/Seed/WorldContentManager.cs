using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace SB.Seed
{

    public class WorldContentManager : MonoBehaviour
    {

        public static string MENUBUTTON = "GenericMenuButton";
        public GameObject scrollView;
        public graphUniverse worldData;
       
        //GraphController mygraph;

        public ObjectFactory objFactory;

        public string currentPlanetid;
        public graphPlanet currentPlanet;
        public Text planetNameinMenu;

        private static WorldContentManager _instance;
        public static WorldContentManager Instance { get { return _instance; } }

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

        void Start()
        {
            //mygraph = FindObjectOfType<GraphController> ();
            GetPlanets();
        }

        public void Draw2DUIMenu(graphVertex[] data)
        {

            foreach (Transform child in scrollView.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (graphPlanet v in data)
            {
                if (v._id.Contains("multiverse"))
                    continue;

                GameObject go = (GameObject)Instantiate(Resources.Load("GenericMenuButton"));
                go.transform.SetParent(scrollView.transform, false);
                Vector3 pos = go.transform.GetComponent<RectTransform>().position;
                pos.z = 0;
                go.transform.localScale = new Vector3(1, 1, 1);
                Text textfield = go.GetComponentInChildren<Text>();
                textfield.text = v.name;
                LoadPlanetAction action = go.AddComponent<LoadPlanetAction>();
                action.planetid = v._id.Split('/')[1];
            }
        }


        public void EditObject(ObjectData transform)
        {
            if (currentPlanetid == "0")
                return;

            transform.planetid = currentPlanetid;
            NetworkHandler restapi = GetComponentInChildren<NetworkHandler>();
            restapi.SyncObject(transform);
        }

        public void DeleteObject(string objectid)
        {
            NetworkHandler restapi = GetComponentInChildren<NetworkHandler>();
            StartCoroutine(restapi.LoadStuff(restapi.methodforCall("POST"), NetworkHandler.DELETEOBJECT + "/" + objectid, (data) =>
            {
              
            }, (data) =>
            {
                Debug.Log(data);
            },null));
        }

        public void AddPlanet(string planetName)
        {
            //need a name
            if (planetName.Length == 0)
                return;

            graphPlanet planet = new graphPlanet();
            planet.name = planetName;

            //clean scene
            ObjectFactory.Instance.resetScene();

            NetworkHandler restapi = GetComponentInChildren<NetworkHandler>();
            StartCoroutine(restapi.LoadStuff(restapi.methodforCall("POST"), NetworkHandler.CREATE + "/universe/multiverse", (data) =>
            {
               // Debug.Log(data);
                graphPlanet nuplanet = JsonUtility.FromJson<graphPlanet>(data);
                currentPlanetid = nuplanet._id.Split('/')[1];
                currentPlanet = nuplanet;
                planetNameinMenu.text = nuplanet.name;

                GetPlanets();
            }, (data) =>
            {
                Debug.Log(data);
            }, JsonUtility.ToJson(planet)));

        }

        public void LoadPlanet(string planetid)
        {
            NetworkHandler restapi = GetComponentInChildren<NetworkHandler>();
            StartCoroutine(restapi.LoadStuff(restapi.methodforCall("GET"), NetworkHandler.GETOBJECTS + "/" + planetid, (data) =>
            {
             
                Debug.Log(data);
                Planet planet = JsonUtility.FromJson<Planet>(data);
                if (planet.id != null)
                    ObjectFactory.Instance.resetScene();
             
                Debug.Log(planet.id);
                currentPlanetid = planet.id;
                planetNameinMenu.text = planet.name;


                foreach (ReverieObject obj in planet.data)
                {
                    Debug.Log(obj.assetid);
                    if(obj.assetid != null)
                    {
                        objFactory.loadObjectFromPlanet(obj);
                    }
                }
            }));
        }

        public void AddObject(string objectid, int index =0, Transform obtrans = null)
        {
            Debug.Log(this.currentPlanetid);
            if (this.currentPlanetid == "0")
                return;

            NetworkHandler restapi = GetComponentInChildren<NetworkHandler>();
            StartCoroutine(restapi.LoadStuff(restapi.methodforCall("POST"), NetworkHandler.ADDOBJECT + "/" + this.currentPlanetid + "/" + objectid, (data) =>
            {
                //Debug.Log(data);
                ArangoEdge myObject = JsonUtility.FromJson<ArangoEdge>(data);
                Debug.Log(myObject._to + " " + myObject._from);
                string serverid = myObject._to.Split('/')[1];

                ReverieObject newObj = new ReverieObject();
                newObj._id = serverid;
                newObj.transform = new BasicTranform(obtrans);


                ObjectFactory.Instance.triggerPrefab(index, newObj);
                UIButtonTrigger trigger = obtrans.gameObject.GetComponentInChildren<UIButtonTrigger>();
                trigger.okDone();

            }));
        }

    

        public void GetPlanets()
        {
          
            NetworkHandler restapi = GetComponentInChildren<NetworkHandler>();
            StartCoroutine(restapi.LoadStuff(restapi.methodforCall("GET"), NetworkHandler.GRAPH + "/multiverse", (data) =>
            {
               
                worldData = JsonUtility.FromJson<graphUniverse>(data);

                Draw2DUIMenu(worldData.vertices);

                float i = 0.01f;
                foreach (graphPlanet v in worldData.vertices)
                {

                    //StartCoroutine(addNodeToGraph(v, i));
                    i += 0.02f;
                }
            }));
        }

        IEnumerator addNodeToGraph(graphPlanet v, float delay)
        {
            //This is a coroutine

            yield return new WaitForSeconds(delay);
            //mygraph.GenerateNode (v.name, v._id, v._rev);

        }
    }

    [Serializable]
    public class graphPlanet : graphVertex
    {
        public string name;
    }

    [Serializable]
    public class graphUniverse : graphData<graphEdge, graphPlanet>
    {

        public graphVertex[] searchVertices(string text)
        {

            return Array.FindAll(vertices, (graphVertex) =>
            {
                Debug.Log(graphVertex.name);
                if (graphVertex.name != null)
                    return graphVertex.name.Contains(text);
                else
                    return false;
            });
        }
    }

    [Serializable]
    public class ReverieObject : graphVertex
    {
        public string assetid;
        public BasicTranform transform;
        public GameObject obj;
    }

    [Serializable]
    public class Planet
    {
        public ReverieObject [] data;
        public string id;
        public string name;
    }

    [Serializable]
    public class ArangoEdge
    {
        public string _to;
        public string _from;
    }


    [Serializable]
    public class graphEdge
    {
        string _key; //"_key": "792613",
        string _id; //"_id": "contains/792613",
        string _from; //"_from": "multiverse/791907",
        string _to; //"_to": "universe/792601",
        string data;
    }

    [Serializable]
    public class graphVertex
    {
        public string _key;
        public string _id; //"_id": "multiverse/791907",
        public string _rev; //"_rev": "_UUoees6---",
    }


    [Serializable]
    public class graphPath
    {
        public graphEdge[] edges;
        public graphVertex[] vertices;
    }

    [Serializable]
    public abstract class graphData<T, Y>
    {
        public T[] paths { get; set; }
        public Y[] vertices;

    }
}
	
