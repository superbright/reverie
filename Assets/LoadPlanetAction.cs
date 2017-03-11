using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB.Seed
{

    public class LoadPlanetAction : MonoBehaviour {

        public string planetid = "";

        public void loadroom()
        {
            WorldContentManager.Instance.LoadPlanet(planetid);
        }

    }
}