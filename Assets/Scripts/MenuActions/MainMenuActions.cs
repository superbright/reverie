using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;

namespace SB.Seed
{

    public static class MainMenuActions
    {
        // Use this for initialization
        public static Dictionary<HandControllers.DIRECTION, MenuItem> InitMenu(MenuUIHandler ui)
        {

            MenuItem nextmenu = new MenuItem("Next Menu", "", "nextmenu", true,
               (GameObject obj, ViveControllerModule.EventData edata) =>
               {
                   obj.SendMessage("showNextMenu",edata.viveControllerModule.gameObject.transform);
               }, null, ui, null);

            MenuItem prevmenu = new MenuItem("Prev Menu", "", "prevmenu", true,
              (GameObject obj, ViveControllerModule.EventData edata) =>
              {
                  obj.SendMessage("showPrevMenu", edata.viveControllerModule.gameObject.transform);
              }, null, ui, null);

            MenuItem scrollUp = new MenuItem("Scroll Up", "", "scrollup", true,
              (GameObject obj, ViveControllerModule.EventData edata) =>
              {
                  obj.SendMessage("scrollUp", edata.viveControllerModule.gameObject.transform);
              }, null, ui, null);


            MenuItem scrolldown = new MenuItem("Scroll Down", "", "scrolldown", true,
              (GameObject obj, ViveControllerModule.EventData edata) =>
              {
                  obj.SendMessage("scrollDown", edata.viveControllerModule.gameObject.transform);
              }, null, ui, null);

            // submenuitems = new List<MenuItem> { movex, scaleup, movey, movex };

            //build all in a dictionary
            return new Dictionary<HandControllers.DIRECTION, MenuItem>()
           {
                {HandControllers.DIRECTION.WEST, prevmenu },
                {HandControllers.DIRECTION.EAST, nextmenu },
                {HandControllers.DIRECTION.NORTH,scrollUp },
                {HandControllers.DIRECTION.SOUTH,scrolldown }
              
           };
        }

    }

}
