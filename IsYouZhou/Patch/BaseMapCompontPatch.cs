using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(BaseMapCompont), "Awake")]
    public class BaseMapCompontPatch
    {
        [HarmonyPrefix]
        public static bool BaseMapCompont_Patch(BaseMapCompont __instance)
        {
            //int.TryParse(__instance.gameObject.name, out __instance.NodeIndex);
            //__instance.AllMapCastTimeJsonData = jsonData.instance.AllMapCastTimeJsonData;
            //__instance.MapRandomJsonData = jsonData.instance.MapRandomJsonData;
            //return false;
            UnityEngine.GameObject gameObject = __instance.gameObject;
            if (gameObject.name.EndsWith("(Clone)"))
            {
                gameObject.name = gameObject.name.Remove(gameObject.name.Length - 7);
                if (Tools.getScreenName() == "AllMaps" && gameObject.name == "13" && __instance.transform.parent.Find("750") == null)
                {
                    gameObject.name = "750";
                    __instance.NodeIndex = 750;
                }

                __instance.AllMapCastTimeJsonData = jsonData.instance.AllMapCastTimeJsonData;
                __instance.MapRandomJsonData = jsonData.instance.MapRandomJsonData;
            }

            return true;
        }
    }
}
