using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.TianNan.Utils;
using UnityEngine;

namespace top.Isteyft.MCS.TianNan.TianNan.Patch
{
    [HarmonyPatch(typeof(LoadingScreen))]
    public class LoadingScreenPatch
    {
        [HarmonyPatch("LoadScene")]
        public static void Prefix()
        {
            if (!TianNanMapUtils.init)
            {
                TianNanMapUtils.init = true;
                string path = Main.dll + "/BaizeAssets/Map/天南.ab";
                Main.LogInfo($"加载天南大地图,路径:{path}");
                AssetBundle.LoadFromFile(path);
            }
        }
    }
}
