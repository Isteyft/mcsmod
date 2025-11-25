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
        }
    }
}
