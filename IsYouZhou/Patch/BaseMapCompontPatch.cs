using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(BaseMapCompont), "Awake")]
    internal class BaseMapCompontPatch
    {
        [HarmonyPrefix]
        private static bool BaseMapCompont_Patch(BaseMapCompont __instance)
        {
            int.TryParse(__instance.gameObject.name, out __instance.NodeIndex);
            __instance.AllMapCastTimeJsonData = jsonData.instance.AllMapCastTimeJsonData;
            __instance.MapRandomJsonData = jsonData.instance.MapRandomJsonData;
            return false;
        }
    }
}
