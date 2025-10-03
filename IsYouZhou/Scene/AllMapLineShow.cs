using System;
using System.Collections.Generic;
using UnityEngine;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    /// <summary>
    /// 大地图线路显示控制器，负责显示地图中各路点之间的连接线
    /// </summary>
    public class AllMapLineShow : MonoBehaviour
    {
        // 存储所有地图组件的列表
        private List<MapComponent> maps = new List<MapComponent>();
        /// <summary>
        /// Unity Start生命周期方法，初始化并显示所有路点连接线
        /// </summary>
        private void Start()
        {
            for (int i = 0; i < base.transform.childCount; i++)
            {
                // 获取每个子物体的AllMapComponent组件并添加到列表
                this.maps.Add(base.transform.GetChild(i).GetComponent<AllMapComponent>());
            }
            // 为每个地图组件调用显示调试连线的方法
            foreach (MapComponent mapComponent in this.maps)
            {
                mapComponent.showDebugLine();
            }
        }
    }
}
