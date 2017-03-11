using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;

namespace SB.Seed
{

    public static class FloorMenuActions
    {

        // Use this for initialization
        public static Dictionary<HandControllers.DIRECTION, MenuItem> InitMenu(MenuUIHandler ui)
        {

            MenuItem dectrans = new MenuItem("- Transp", "", "dectrans", true,
              (GameObject obj, ViveControllerModule.EventData edata) =>
              {
                  Color color = obj.GetComponent<Renderer>().material.color;
                  color.a -= 0.1f;
                  obj.GetComponent<Renderer>().material.color = color;
                
              }, null, ui, null);

            MenuItem addtrans = new MenuItem("+ Transp", "", "dectrans", true,
            (GameObject obj, ViveControllerModule.EventData edata) =>
            {
                Color color = obj.GetComponent<Renderer>().material.color;
                color.a += 0.1f;
                obj.GetComponent<Renderer>().material.color = color;

            }, null, ui, null);

            MenuItem scaleup = new MenuItem("Scale Up", "", "scaleup", true,
               (GameObject obj, ViveControllerModule.EventData edata) =>
               {
                   obj.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
               }, null, ui, null);

            MenuItem scaledown = new MenuItem("Scale Down", "", "scaledown", true,
              (GameObject obj, ViveControllerModule.EventData edata) =>
              {
                  Debug.Log("DOWN BITCH");
                  obj.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
              }, null, ui, null);



            //build all in a dictionary
           return new Dictionary<HandControllers.DIRECTION, MenuItem>()
           {
                {HandControllers.DIRECTION.NORTH, scaleup },
                {HandControllers.DIRECTION.SOUTH, scaledown },
               {HandControllers.DIRECTION.WEST, dectrans},
               {HandControllers.DIRECTION.EAST, addtrans }
           };
        }

    }

}
