using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB.Seed
{

    public class DebugMenu : MonoBehaviour
    {
        public UnityEngine.UI.InputField roomname;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateObject(int i)
        {

        }

        public void CreateRoom()
        {
            if (roomname.text.Length < 1)
                return;

            WorldContentManager.Instance.AddPlanet(roomname.text);
        }

        public void JoinRoom(string roomid)
        {
            WorldContentManager.Instance.LoadPlanet(roomid);
        }
    }
}