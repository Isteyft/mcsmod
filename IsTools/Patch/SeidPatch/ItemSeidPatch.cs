using GUIPackage;
using HarmonyLib;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Patch.SeidPatch
{
    [HarmonyPatch(typeof(item))]
    public class ItemSeidPatch
    {
        [HarmonyPatch("realizeSeid")]
        [HarmonyPrefix]
        public static bool RealizeSeid(item __instance, int seid)
        {
            if (seid < 360)
            {
                return true;
            }
            else
            {
                switch (seid)
                {
                    case 360:
                        ItemSeidPatch.RealizeSeid360(__instance, seid);
                        return false;
                }
            }
            return true;
        }
        private static void RealizeSeid360(item __instance, int seid)
        {
            string str = __instance.getSeidJson(seid)["value1"].str;
            DialogAnalysis.StartTestDialogEvent(str, null);
        }
    }
}
