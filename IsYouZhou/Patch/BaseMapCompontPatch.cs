using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.JiuZhou.Patch
{
    [HarmonyPatch(typeof(BaseMapCompont), "Awake")]
    public class BaseMapCompontPatch
    {
        [HarmonyPrefix]
        public static bool BaseMapCompont_Patch(BaseMapCompont __instance)
        {
            int.TryParse(__instance.gameObject.name, out __instance.NodeIndex);
            __instance.AllMapCastTimeJsonData = jsonData.instance.AllMapCastTimeJsonData;
            __instance.MapRandomJsonData = jsonData.instance.MapRandomJsonData;
            return false;
        }
    }
}
