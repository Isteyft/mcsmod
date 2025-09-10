using HarmonyLib;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch { 

    [HarmonyPatch(typeof(Buff))]
    public class BuffPatch
    {
        [HarmonyPatch("onAttachRealizeSeid109")]
        [HarmonyPrefix]
        public static bool Prefix(Buff __instance, int seid, KBEngine.Avatar avatar)
        {
            bool flag = JieDanManager.instence != null;
            bool result;
            if (flag)
            {
                int endIndex = 13;
                foreach (JSONObject jsonobject in __instance.getSeidJson(seid)["value1"].list)
                {
                    avatar.FightAddSkill((int)jsonobject.n, 0, endIndex);
                }
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}
