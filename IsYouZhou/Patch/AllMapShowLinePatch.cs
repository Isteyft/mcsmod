using System;
using System.Collections.Generic;
using Fungus;
using GetWay;
using HarmonyLib;
using MaiJiu.MCS.HH.Util;
using SkySwordKill.Next.DialogSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using top.Isteyft.MCS.YouZhou.Utils;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(AllMapShowLine), "Start")]
    public class AllMapShowLinePatch
    {
        [HarmonyPostfix]
        private static void AddExtraNodes(AllMapShowLine __instance)
        {
            //IsToolsMain.Log("750");
            MaiJiu.MCS.HH.Util.Task task = __instance.gameObject.AddComponent<MaiJiu.MCS.HH.Util.Task>();
            Action action = delegate ()
            {
                if (MapNodeManager.inst.transform.Find("LevelsWorld0/750") == null)
                {
                    GameObject gameObject = MapNodeManager.inst.transform.Find("LevelsWorld0/13").gameObject;
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    gameObject2.name = "750";
                    gameObject2.transform.parent = MapNodeManager.inst.transform.Find("LevelsWorld0");
                    gameObject2.transform.localPosition = new Vector3(-6.8f, 15.2f, 0f);
                    gameObject2.gameObject.SetActive(true);
                    gameObject2.GetComponent<MapComponent>().gameObject.SetActive(true);
                    //gameObject2.transform.localPosition += new Vector3(-1f, 1f, 0f);
                    List<MapComponent> value = Traverse.Create(__instance).Field("maps").GetValue<List<MapComponent>>();
                    UnityEngine.Object.Destroy(gameObject2.gameObject.GetComponent<AllMapNodeClick>());
                    gameObject2.transform.Find("ImageName/Canvas/Text").GetComponent<Text>().text = "幽州";
                    gameObject2.transform.Find("Level1Move").GetComponent<SpriteRenderer>().sprite = IsToolsMain.I.UIManagerHandle.spriteBank["YouZhou.png"];
                    gameObject2.transform.Find("Level1Move").localScale = new Vector3(1f, 1f, 0f);
                    gameObject2.transform.Find("Level1Move").localPosition = new Vector3(-0.362f, -2.82f, 0f);
                    gameObject2.transform.Find("flowchat/enter/Canvas/Button").GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        //DialogAnalysis.StartDialogEvent("幽州-进入幽州", null);
                        MyUtil.LoadYouZhou();
                    });
                    Block[] componentsInChildren = gameObject2.transform.Find("flowchat").GetComponentsInChildren<Block>();
                    componentsInChildren[0]._EventHandler = null;
                    componentsInChildren[0].commandList = null;
                    gameObject2.GetComponent<MapComponent>().NodeIndex = 750;
                    gameObject2.GetComponent<MapComponent>().NodeGroup = 750;
                    value.Add(gameObject2.GetComponent<MapComponent>());
                }
            };
            task.TaskInvoke(action, 1f);
        }
    }
}