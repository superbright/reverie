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
        public GameObject leftMenu;
        public MenuUIHandler rightmenuui;
        public MenuUIHandler leftmenuui;
        // MenuItem leftmenu;
        MenuItem rightmenu;
        MenuItem leftmenu;

        MenuItem objectmenu;
        MenuItem floormenu;

        int leftcontrollerid;
        int rightcontrollerid;

        //map of left hand pad
        Dictionary<DIRECTION, MenuItem> objectmenumap;
        Dictionary<DIRECTION, MenuItem> floormenumap;
        Dictionary<DIRECTION, MenuItem> rightmenumap;

        Dictionary<DOOType, Dictionary<DIRECTION, MenuItem>> leftmenuActions = new Dictionary<DOOType, Dictionary<DIRECTION, MenuItem>>();
      

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

            leftcontrollerid = (int)leftmenuui.gameObject.GetComponentInChildren<SteamVR_TrackedObject>().index; // SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
            rightcontrollerid = (int)rightmenuui.gameObject.GetComponentInChildren<SteamVR_TrackedObject>().index; //SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
           // Debug.Log("left " + leftcontrollerid + " right " + rightcontrollerid);

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
                    }, rightmenuui, leftMenu);


            //build all in a dictionary
            rightmenumap = new Dictionary<DIRECTION, MenuItem>()
           {
               {DIRECTION.NORTH, rightmenu },
               
           };

            //load default menu item

            objectmenu = new MenuItem("", "Point to trigger", "rightmenu", false,
               (GameObject obj, ViveControllerModule.EventData edata) =>
               {
               }, null, leftmenuui, null);

            floormenu = new MenuItem("", "", "", false,
             (GameObject obj, ViveControllerModule.EventData edata) =>
             {
             }, null, leftmenuui, null);


            objectmenumap = ObjectMenuActions.InitMenu(leftmenuui);
            objectmenu.setSubMenuItems(objectmenumap);
            objectmenu.drawMenuLabels();
            leftmenuui.hideMenu();

            floormenumap = FloorMenuActions.InitMenu(leftmenuui);
            floormenu.setSubMenuItems(floormenumap);

            leftmenuActions.Add(DOOType.FLOOR,floormenumap);
            leftmenuActions.Add(DOOType.OBJECT, objectmenumap);


            //setup right menu

            rightmenu.setSubMenuItems(rightmenumap);
            rightmenu.drawMenuLabels();

        }

        //TODO: add left or right controller target
        /// <summary>
        /// This is to update the menu on the controllers according to which object you point to
        /// </summary>
        /// <param name="type"></param>
        public void setMenuItem(DOOType type)
        {
            leftmenuui.hideMenu();
            switch (type)
            {
                case DOOType.OBJECT:
                    objectmenu.drawMenuLabels();
                    break;
                case DOOType.FLOOR:
                    floormenu.drawMenuLabels();
                    break;
                case DOOType.NONE:
                    leftmenuui.hideMenu();
                    break;
            }
        }

  
        public void OnGlobalTouchpadPressDown(ViveControllerModule.EventData eventData)
        {
           // Debug.Log(eventData.viveControllerModule.Controller.index);
           // Debug.Log("left " + leftcontrollerid + " right " + rightcontrollerid);

            DOOType type = eventData.currentRaycast.gameObject.GetComponentInChildren<DOOObject>().type;

            if (eventData.viveControllerModule.Controller.index == leftcontrollerid)
            {
            
                // find matching action and trigger that
               MenuItem val;
                if(leftmenuActions[type].TryGetValue(ParseDirection(eventData),out val)) {
                    
                    val.trigger(eventData.currentRaycast.gameObject, eventData);
                    leftmenu.ui.setSelected(ParseDirection(eventData));
                } else {
                    Debug.Log("no action bro");
                }
            }
            else
            {
               
                rightmenu.setState(!rightmenu.isActivated, eventData);
                rightmenu.ui.setSelected(ParseDirection(eventData));
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
