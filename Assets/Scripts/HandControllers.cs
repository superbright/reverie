using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FRL.IO;
using System;
using System.Linq;

namespace SB.Seed
{
    [RequireComponent(typeof(Receiver))]
    public class HandControllers : MonoBehaviour, IGlobalTouchpadPressDownHandler, IGlobalGripPressDownHandler, IGlobalTouchpadTouchHandler
    {
        public MenuUIHandler rightmenuui;
        public MenuUIHandler leftmenuui;
     
        MenuItem rightmenu;
    
        int leftcontrollerid;
        int rightcontrollerid;

        //all actions for different DOO types
        Dictionary<DOOType, MenuItem> leftMenuMap = new Dictionary<DOOType, MenuItem>();

        List<ViveControllerModule> controllers;

        private static HandControllers _instance;
        public static HandControllers Instance { get { return _instance; } }


        //mapping the direction of Vive PAD
        public enum DIRECTION
        {
            WEST,
            NORTHWEST,
            NORTH,
            NORTHEAST,
            EAST,
            SOUTHEAST,
            SOUTH,
            SOUTHWEST,
            NONE
        };

        //according to actions, these can unlock things
        public enum ACTIONTYPE
        {
            BASIC,
            MOVEX,
            MOVEY,
            MOVEZ
        };

        // some mmenu items just trigger an action, or they toggle on and off
        public enum MENUITEMTYPE
        {
            SIMPLE,
            TOGGLE
        };
        public HandControllers.ACTIONTYPE movementState = HandControllers.ACTIONTYPE.BASIC;

        // mapping our pads to 8 direction value, values are chosen from Radian rotations of ATAN of the pad
        Dictionary<DIRECTION, float> padValueMap = new Dictionary<DIRECTION, float>()
                {
                    {DIRECTION.NORTH, 1.5f},
                    {DIRECTION.NORTHEAST, 0.75f },
                    {DIRECTION.EAST, 0f },
                    {DIRECTION.SOUTHEAST, -0.75f },
                    {DIRECTION.SOUTH, -1.5f },
                    {DIRECTION.SOUTHWEST, -2.25f },
                    {DIRECTION.WEST, -3.0f },
                    {DIRECTION.NORTHWEST, 2.25f }

                };


        private void Awake()
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

        // Use this for initialization
        void Start()
        { 
            //find which controller we are addressing
            StartCoroutine(SetupController());
        }



        IEnumerator SetupController()
        {
            yield return new WaitForSeconds(5);

            leftcontrollerid = (int)leftmenuui.gameObject.GetComponentInChildren<SteamVR_TrackedObject>().index; 
            rightcontrollerid = (int)rightmenuui.gameObject.GetComponentInChildren<SteamVR_TrackedObject>().index;
            // Debug.Log("left " + leftcontrollerid + " right " + rightcontrollerid);

            // each menu item has a parent menu, and draws the menu items that are su
            //parent
            MenuItem objectmenu = new MenuItem("", "Point to trigger", "rightmenu", false,
               (GameObject obj, ViveControllerModule.EventData edata) =>
               {
               }, null, leftmenuui, null);
            leftMenuMap.Add(DOOType.OBJECT, objectmenu);

            //submenu items
            objectmenu.setSubMenuItems(ObjectMenuActions.InitMenu(leftmenuui));

            //parent
            MenuItem floormenu = new MenuItem("", "", "", false,
             (GameObject obj, ViveControllerModule.EventData edata) =>
             {
             }, null, leftmenuui, null);
            leftMenuMap.Add(DOOType.FLOOR, floormenu);
            //sub menu actions
            floormenu.setSubMenuItems(FloorMenuActions.InitMenu(leftmenuui));

          
            //setup right menu
           // Dictionary<DIRECTION, MenuItem> rightmenumap;
            rightmenu = new MenuItem("Menu On", "Menu Off", "mainmenu", false,
                (GameObject obj, ViveControllerModule.EventData edata) =>
                {
                    obj.SetActive(true);
                    obj.transform.parent = edata.viveControllerModule.gameObject.transform;
                    obj.transform.localPosition = new Vector3(0, 0, 0);
                    obj.transform.localRotation = Quaternion.Euler(Vector3.zero);

                }, (GameObject obj, ViveControllerModule.EventData edata) =>
                {
                    obj.SetActive(false);
                }, rightmenuui, null);

            rightmenu.setSubMenuItems(MainMenuActions.InitMenu(rightmenuui));
            rightmenu.drawMenuLabels();

            //hide everything until you wanna draw them
            leftmenuui.hideMenu();

        }

        //TODO: add left or right controller target
        /// <summary>
        /// This is to update the menu on the controllers according to which object you point to
        /// </summary>
        /// <param name="type"></param>
        public void setMenuItem(DOOType type)
        { 
            leftmenuui.hideMenu();

              // find matching action and trigger that
               MenuItem val;
                if(leftMenuMap.TryGetValue(type, out val)) {
                    val.drawMenuLabels();
         
                } else {
                    Debug.Log("no action bro");
                     leftmenuui.hideMenu();
                 }
         
        }

  
        public void OnGlobalTouchpadPressDown(ViveControllerModule.EventData eventData)
        {
           // Debug.Log(eventData.viveControllerModule.Controller.index);
           // Debug.Log("left " + leftcontrollerid + " right " + rightcontrollerid);


            //is left menu
            if (eventData.viveControllerModule.Controller.index == leftcontrollerid)
            {

                DOOType type = eventData.currentRaycast.gameObject.GetComponentInChildren<DOOObject>().type;
                // find matching main menu from DOO type
                MenuItem mainmenu;
               if(leftMenuMap.TryGetValue(type,out mainmenu))
                {
                    //find matching submenu from Direction
                    MenuItem submenu;
                    if (mainmenu.submenuitems.TryGetValue(ParseDirection(eventData), out submenu))
                    { 
                        submenu.trigger(eventData.currentRaycast.gameObject, eventData);
                        mainmenu.ui.setSelected(ParseDirection(eventData));
                    }
                    else
                    {
                        Debug.Log("no action bro");
                    }

                } else
                {
                    //no menu found
                }
            }
            // no its actually right menu
            else
            {
               
                //rightmenu.setState(!rightmenu.isActivated, eventData);
                //rightmenu.ui.setSelected(ParseDirection(eventData));
                //find matching submenu from Direction
                MenuItem submenu;
                if (rightmenu.submenuitems.TryGetValue(ParseDirection(eventData), out submenu))
                {
                    submenu.trigger(MainMenuManager.Instance.gameObject, eventData);
                    //rightmenu.ui.setSelected(ParseDirection(eventData));
                }
                else
                {
                    Debug.Log("no action bro");
                }
            }
        }

        public void OnGlobalTouchpadTouch(ViveControllerModule.EventData eventData)
        {

            if (eventData.viveControllerModule.Controller.index == leftcontrollerid)
            {
                //  Debug.Log("left");
                leftmenuui.setHover(ParseDirection(eventData));
                //left
            }
            else
            {
                //  Debug.Log("right");
                rightmenuui.setHover(ParseDirection(eventData));
            };
        }

        public void OnGlobalGripPressDown(ViveControllerModule.EventData eventData)
        {
            if (eventData.viveControllerModule.Controller.index == leftcontrollerid)
            {
                Debug.Log("left");

                //left
            }
            else
            {
                Debug.Log("right");
               
            };
        }

        /// <summary>
        /// Take the event data values from PAD Touch and use radians to return a mapped from DIRECTION ENUM
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public DIRECTION ParseDirection(ViveControllerModule.EventData eventData)
        {
            // 0.1875
            double rad = Math.Atan2(eventData.touchpadAxis.y, eventData.touchpadAxis.x);

           // value used to map ranges for each direction
           // TODO: move this somewhere
            double range = 0.375;
            double radianDelta = 2.75;

            // sort dictionary according to ranges
            // we are using a range +- range variable above and finding if our radian is between that range.
            foreach (var item in padValueMap.Where(item =>
            {
                var bottomvalue = (item.Value - range);
                var topvalue = (item.Value + range);
                //Debug.Log("key " + item.Key + " val " + item.Value + " rad " + rad + "bottom " + bottomvalue + " top " + topvalue);

                // if we reach the top reverse the ranges only applies to one value
                if (rad > radianDelta)
                {
                    return (-topvalue < rad && rad < -bottomvalue);
                } else
                {
                    return (bottomvalue < rad && rad < topvalue);
                }

            }).ToList()) {
              //  Debug.Log(item.Key);
                return item.Key;
            };
            return DIRECTION.NONE;

        }

     
    }

    /// <summary>
    /// Class that creates a menu item with an action
    /// </summary>
    [Serializable]
    public class MenuItem
    {
        //TODO : move it to a const
        static int SUBMENULENGTH = 4;


        //name of the menu item
        public string labelActive;
        public string labelInActive;

        //prefab name to init
        public string gameobjectName;
        //submenu items, a menu item does not submenu items it could mean they just execute actions
        public Dictionary<HandControllers.DIRECTION, MenuItem> submenuitems;
        //is this active
        public bool isActivated;
        //things that will happen to a gameoject when it is turned on
        Dictionary<bool, Action<GameObject, ViveControllerModule.EventData>> pressPairings = new Dictionary<bool, Action<GameObject, ViveControllerModule.EventData>>();

        //targeting object in the scene;
        GameObject obj;
        public MenuUIHandler ui;

        public MenuItem(string _name)
        {
            labelActive = _name;
        }
        public MenuItem(string _labelActive, string _labelDisabled, string _gameobjectname, bool _isActivated, Action<GameObject, ViveControllerModule.EventData> on, Action<GameObject, ViveControllerModule.EventData> off,MenuUIHandler _ui = null, GameObject _obj = null )
        {

            labelActive = _labelActive;
            labelInActive = _labelDisabled;
            gameobjectName = _gameobjectname;
            isActivated = _isActivated;
            pressPairings.Add(false, off);
            pressPairings.Add(true, on);

            if(_ui)
            {
                ui = _ui;
            }
            if(_obj)
            {
                obj = _obj;
            }
           
        }

        /// <summary>
        /// set the sub menu items around vive controller according to the directions available
        /// </summary>
        /// <param name="menuitems"></param>
        public void setSubMenuItems(Dictionary<HandControllers.DIRECTION, MenuItem> _submenuitems) {
            submenuitems = _submenuitems;
        }


        /// <summary>
        /// Collect all the label names from menu and sub menu items
        /// </summary>
        /// <returns></returns>
        public void drawMenuLabels()
        {
            ui.setMenuLabels(submenuitems);
        }

        // trigger the action
        public void trigger(GameObject _obj, ViveControllerModule.EventData eventData)
        {
            if (!isActivated) return;

            Action<GameObject, ViveControllerModule.EventData> value;
            // execute the function
            if (pressPairings.TryGetValue(true, out value))
            {
                if (value != null)
                {
                    value(_obj, eventData);
                }
            }
            else
            {
                Console.WriteLine("Key =" + true + " is not found.");
            }

        }

        // turn control on and off
        public void setState(bool state, ViveControllerModule.EventData eventData)
        {
            if (isActivated == state) return;

            Action<GameObject, ViveControllerModule.EventData> value;
            // execute the function
            if (pressPairings.TryGetValue(state, out value))
            {
                if(value != null)
                {
                    value(obj,eventData);
                }  
            }
            else
            {
                Console.WriteLine("Key =" + state +  " is not found.");
            }

            isActivated = state;
            drawMenuLabels();
        }

        private string currentLabel()
        {
            if (isActivated)
                return labelInActive;
            else
                return labelActive;
        }

    }
}
