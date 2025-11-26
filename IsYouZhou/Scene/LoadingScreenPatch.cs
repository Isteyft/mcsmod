using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.Utils;
using UnityEngine;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    [HarmonyPatch(typeof(LoadingScreen))]
    public class LoadingScreenPatch
    {
        [HarmonyPatch("LoadScene")]
        public static void Prefix()
        {
            if (!MyUtil.init)
            {
                MyUtil.init = true;
                string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/幽州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (!MyUtil.XJYinit)
            {
                MyUtil.init = true;
                string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/雪剑域.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (!MyUtil.ZZinit)
            {
                MyUtil.init = true;
                string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/中州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (!MyUtil.HZinit)
            {
                MyUtil.HZinit = true;
                string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/衡州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (!MyUtil.BZinit)
            {
                MyUtil.BZinit = true;
                string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/灞州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (!MyUtil.YingZhouinit)
            {
                MyUtil.YingZhouinit = true;
                string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/颍州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (!MyUtil.JZinit)
            {
                MyUtil.JZinit = true;
                string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/靖州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (!MyUtil.YuZhouinit)
            {
                MyUtil.YuZhouinit = true;
                string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/渝州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (!MyUtil.YongZhouinit)
            {
                MyUtil.YongZhouinit = true;
                string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/雍州.ab";
                AssetBundle.LoadFromFile(path);
            }
        }
    }
}
