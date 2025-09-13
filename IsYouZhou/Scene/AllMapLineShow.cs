using System;
using System.Collections.Generic;
using UnityEngine;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    public class AllMapLineShow : MonoBehaviour
    {
        private List<MapComponent> maps = new List<MapComponent>();
        private void Start()
        {
            for (int i = 0; i < base.transform.childCount; i++)
            {
                this.maps.Add(base.transform.GetChild(i).GetComponent<AllMapComponent>());
            }
            foreach (MapComponent mapComponent in this.maps)
            {
                mapComponent.showDebugLine();
            }
        }
    }
}
