using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HarmonyLib;
using UnityEngine;


namespace top.Isteyft.MCS.IsTools.Patch
{

    //获取场景信息
    //[HarmonyPatch(typeof(CmdSetBG))]
    //[HarmonyPatch("OnEnter")]
    //public class CmdSetBG_OnEnter_Patch
    //{
    //    [HarmonyPrefix]
    //    static void Prefix(CmdSetBG __instance)
    //    {
    //        if (__instance == null) return;

    //        Type type = typeof(CmdSetBG);

    //        // 使用反射获取字段
    //        var bgNameField = type.GetField("BGName", BindingFlags.NonPublic | BindingFlags.Instance);
    //        var isSaveField = type.GetField("IsSave", BindingFlags.NonPublic | BindingFlags.Instance);
    //        var isNowSceneField = type.GetField("IsNowScene", BindingFlags.NonPublic | BindingFlags.Instance);
    //        var sceneNameField = type.GetField("SceneName", BindingFlags.NonPublic | BindingFlags.Instance);

    //        string bgName = bgNameField?.GetValue(__instance) as string;
    //        bool isSave = (bool)isSaveField?.GetValue(__instance);
    //        bool isNowScene = (bool)isNowSceneField?.GetValue(__instance);
    //        string sceneName = sceneNameField?.GetValue(__instance) as string;

    //        IsToolsMain.Log($"[反射] BGName = {bgName}, IsSave = {isSave}, IsNowScene = {isNowScene}, SceneName = {sceneName}");
    //    }
    //}
}
