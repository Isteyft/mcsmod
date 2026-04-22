//using Boo.Lang;
//using GetWay;
//using HarmonyLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace top.Isteyft.MCS.JiuZhou.Scene
//{
//    [HarmonyPatch(typeof(MapGetWay))]
//    public class MapGetWayPatch
//    {
//        [HarmonyPatch("Init")]
//        public static void Postfix()
//        {
//            int childCount = MapNodeManager.inst.MapNodeParent.transform.childCount;
//            for (int i = 0; i < childCount; i++)
//            {
//                MapComponent component = MapNodeManager.inst.MapNodeParent.transform.GetChild(i).GetComponent<MapComponent>();
//                // --- 打印元数据 ---
//                IsToolsMain.LogInfo($"[元数据] GameObject 名字: '{component.gameObject.name}'");
//                IsToolsMain.LogInfo($"[元数据] NodeIndex: {component.NodeIndex}");
//                IsToolsMain.LogInfo($"[元数据] 位置坐标: ({component.transform.position.x}, {component.transform.position.y})");

//                // 打印 nextIndex 列表内容（如果为 null 要处理）
//                if (component.nextIndex != null)
//                {
//                    string nextIndexStr = string.Join(", ", component.nextIndex);
//                    IsToolsMain.LogInfo($"[元数据] nextIndex 内容: [{nextIndexStr}] | Count: {component.nextIndex.Count}");
//                }
//                else
//                {
//                    IsToolsMain.LogInfo("[元数据] nextIndex 为 null！");
//                }
//            }

//        }
//    }

//    [HarmonyPatch(typeof(MapComponent))]
//    public class MapComponentPatch
//    {
//        [HarmonyPatch("AvatarMoveToThis")]
//        public static void Postfix(MapComponent __instance)
//        {
//            UnityEngine.GameObject gameObject = UnityEngine.GameObject.Find("MapMoveNode");
//            MapMoveNode[] componentsInChildren = gameObject.GetComponentsInChildren<MapMoveNode>();
//            MapMoveNode[] array = componentsInChildren;
//            foreach (MapMoveNode mapMoveNode in array)
//            {
//                if (((mapMoveNode.StartNode == 74101) || (mapMoveNode.EndNode == 74101)) && ((mapMoveNode.StartNode == 74001) || (mapMoveNode.EndNode == 74001)))
//                {
//                    IsToolsMain.LogInfo(mapMoveNode.name);
//                }
//            }
//        }
//    }
//}
