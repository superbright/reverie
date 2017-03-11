using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SB.Seed
{
    public class MenuUIHandler : MonoBehaviour
    {

        public Text west;
        public Text northwest;
        public Text north;
        public Text northeast;
        public Text east;
        public Text southeast;
        public Text south;
        public Text southwest;
        public Text center;

        public GameObject westguide;
        public GameObject northwestguide;
        public GameObject northguide;
        public GameObject northeastguide;
        public GameObject eastguide;
        public GameObject southeastguide;
        public GameObject southguide;
        public GameObject southwestguide;
       // public GameObject centerguide;

        string[] labels;

        Dictionary<HandControllers.DIRECTION, Text> UIMap;
        Dictionary<HandControllers.DIRECTION, GameObject> UIGuideMap;
        Dictionary<state, Color> UIState;

        enum state
        {
            normal,
            hover,
            selected
        };

        // Use this for initialization
        void Start()
        {

            //initialize directions with text labels
            UIMap = new Dictionary<HandControllers.DIRECTION, Text>()
                {
                    { HandControllers.DIRECTION.WEST, west },
                     { HandControllers.DIRECTION.NORTHWEST, northwest },
                      { HandControllers.DIRECTION.NORTH, north },
                       { HandControllers.DIRECTION.NORTHEAST, northeast },
                        { HandControllers.DIRECTION.EAST, east },
                         { HandControllers.DIRECTION.SOUTHEAST, southeast },
                          { HandControllers.DIRECTION.SOUTH, south },
                           { HandControllers.DIRECTION.SOUTHWEST,southwest }


                };
            UIGuideMap = new Dictionary<HandControllers.DIRECTION, GameObject>()
                {
                    { HandControllers.DIRECTION.WEST, westguide },
                     { HandControllers.DIRECTION.NORTHWEST, northwestguide },
                      { HandControllers.DIRECTION.NORTH, northguide },
                       { HandControllers.DIRECTION.NORTHEAST, northeastguide },
                        { HandControllers.DIRECTION.EAST, eastguide },
                         { HandControllers.DIRECTION.SOUTHEAST, southeastguide },
                          { HandControllers.DIRECTION.SOUTH, southguide },
                           { HandControllers.DIRECTION.SOUTHWEST,southwestguide }


                };

            UIState = new Dictionary<state, Color>()
            {
                {state.normal, Color.white },
                { state.hover, Color.magenta},
                {state.selected, Color.red }

            };

            foreach (KeyValuePair<HandControllers.DIRECTION, Text> t in UIMap)
            {
                t.Value.gameObject.SetActive(false);
            }

        }

        /// <summary>
        /// Updates the menu labels according to the passed down data
        /// </summary>
        /// <param name="submenuitems"></param>
        public void setMenuLabels(Dictionary<HandControllers.DIRECTION, MenuItem> submenuitems)
        {
            foreach (KeyValuePair<HandControllers.DIRECTION, Text> t in UIMap)
            {

                if (submenuitems.ContainsKey(t.Key))
                {
                    t.Value.text = submenuitems[t.Key].labelActive;
                    t.Value.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        public void setHover(HandControllers.DIRECTION dir)
        {
            foreach (KeyValuePair<HandControllers.DIRECTION, Text> t in UIMap)
            {
                if (t.Value.color == UIState[state.selected])
                    continue;

                if (t.Key == dir)
                {
                    t.Value.color = UIState[state.hover];
                }
                else
                {
                    t.Value.color = UIState[state.normal];
                }
            }
            foreach (KeyValuePair<HandControllers.DIRECTION, GameObject> t in UIGuideMap)
            {
                if (t.Value.GetComponent<Text>().color == UIState[state.selected])
                    continue;

                if (t.Key == dir)
                {
                    //t.Value.color = UIState[state.hover];
                    t.Value.GetComponent<Text>().color = UIState[state.hover];
                }
                else
                {
                    t.Value.GetComponent<Text>().color = UIState[state.normal];
                }
            }
        }

        public void setSelected(HandControllers.DIRECTION dir)
        {
            foreach (KeyValuePair<HandControllers.DIRECTION, Text> t in UIMap)
            {

                if (t.Key == dir)
                {
                    t.Value.color = UIState[state.selected];
                }
                else
                {
                    t.Value.color = UIState[state.normal];
                }
            }
            foreach (KeyValuePair<HandControllers.DIRECTION, GameObject> t in UIGuideMap)
            {
                if (t.Key == dir)
                {
                    t.Value.GetComponent<Text>().color = UIState[state.selected];
                }
                else
                {
                    t.Value.GetComponent<Text>().color = UIState[state.normal];
                }
            }
        }

        public void drawMenu()
        {
            foreach (KeyValuePair<HandControllers.DIRECTION, Text> t in UIMap)
            {
                t.Value.gameObject.SetActive(true);
                UIGuideMap[t.Key].SetActive(true);
            }
        }

        public void hideMenu()
        {
            foreach (KeyValuePair<HandControllers.DIRECTION, Text> t in UIMap)
            {
                t.Value.gameObject.SetActive(false);
               // UIGuideMap[t.Key].SetActive(false); 
            }  
        }

    }

}