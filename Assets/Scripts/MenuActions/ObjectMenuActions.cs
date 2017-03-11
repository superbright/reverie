using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;

namespace SB.Seed
{

    public static class ObjectMenuActions
    {

        // Use this for initialization
        public static Dictionary<HandControllers.DIRECTION, MenuItem> InitMenu(MenuUIHandler ui)
        {

            MenuItem scaleup = new MenuItem("Scale Up", "", "scaleup", true,
               (GameObject obj, ViveControllerModule.EventData edata) =>
               {
                   obj.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
               }, null, ui, null);

            MenuItem scaledown = new MenuItem("Scale Down", "", "scaledown", false,
              (GameObject obj, ViveControllerModule.EventData edata) =>
              {
                  obj.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
              }, null, ui, null);


            //movementState
            MenuItem lockX = new MenuItem("LockX", "", "LockX", true,
             (GameObject obj, ViveControllerModule.EventData edata) =>
             {
             //movementState = ACTIONTYPE.MOVEX;
             obj.GetComponentInChildren<AxisLineRenderer>().LockMode(AxisLineRenderer.AXIS.X);
                 obj.GetComponentInChildren<AssetWrapper>().isLocked = true;
             //update menu
         }, null, ui, null);

            //movementState
            MenuItem lockY = new MenuItem("LockY", "", "LockY", true,
             (GameObject obj, ViveControllerModule.EventData edata) =>
             {
                 obj.GetComponentInChildren<AxisLineRenderer>().LockMode(AxisLineRenderer.AXIS.Y);
                 obj.GetComponentInChildren<AssetWrapper>().isLocked = true;
             //update menu
         }, null, ui, null);

            //movementState
            MenuItem lockZ = new MenuItem("LockZ", "", "LockZ", true,
             (GameObject obj, ViveControllerModule.EventData edata) =>
             {
                 obj.GetComponentInChildren<AxisLineRenderer>().LockMode(AxisLineRenderer.AXIS.Z);
                 obj.GetComponentInChildren<AssetWrapper>().isLocked = true;
             //update menu
         }, null, ui, null);

            MenuItem doneedit = new MenuItem("Done", "", "Done", true,
            (GameObject obj, ViveControllerModule.EventData edata) =>
            {

                obj.GetComponentInChildren<AssetWrapper>().isLocked = false;
                obj.GetComponentInChildren<AxisLineRenderer>().LockMode(AxisLineRenderer.AXIS.ALL);

            }, null, ui, null);

            MenuItem destroy = new MenuItem("Destroy", "", "Destroy", true,
            (GameObject obj, ViveControllerModule.EventData edata) =>
            {
                GameObject.Destroy(obj);
            }, null, ui, null);

            // submenuitems = new List<MenuItem> { movex, scaleup, movey, movex };

            //build all in a dictionary
           return new Dictionary<HandControllers.DIRECTION, MenuItem>()
           {
                {HandControllers.DIRECTION.NORTH, doneedit },
                {HandControllers.DIRECTION.EAST, destroy },
                {HandControllers.DIRECTION.NORTHWEST,lockX },
                {HandControllers.DIRECTION.WEST, lockY },
                {HandControllers.DIRECTION.SOUTHWEST, lockZ }
           };
        }

    }

}
