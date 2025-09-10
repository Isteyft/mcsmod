using GUIPackage;
using HarmonyLib;
using KBEngine;
using PaiMai;
using System.Collections.Generic;


namespace top.Isteyft.MCS.IsTools.Patch.SeidPatch
{
    [HarmonyPatch(typeof(StaticSkill))]
    internal class StaticSkillSeidPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("realizeSeid")]
        public static bool Prefix(StaticSkill __instance, int seid, List<int> flag, Entity _attaker, Entity _receiver, int type)
        {
            Avatar avatar = (Avatar)_attaker;
            Avatar avatar2 = (Avatar)_receiver;
            //Avatar receiver = (Avatar)_receiver;
            switch (seid)
            {
                case 360:
                    RealizeSeid360(__instance, seid, flag, avatar, avatar2, type);
                    return false;
                case 361:
                    RealizeSeid361(__instance, seid, flag, avatar, avatar2, type);
                    return false;
                default:
                    return true;
            }
        }
        private static void RealizeSeid360(StaticSkill __instance, int seid, List<int> damage, Avatar attaker, Avatar receiver, int type)
        {
            JSONObject seidJson = __instance.getSeidJson(seid);
            JSONObject jSONObject = seidJson["value1"];
            JSONObject jSONObject2 = seidJson["value2"];
            JSONObject jSONObject3 = seidJson["value3"];
            JSONObject jSONObject4 = seidJson["value4"];
            for (int i = 0; i < jSONObject.Count; i++)
            {
                attaker.spell.addDBuff(jSONObject[i].I, jSONObject2[i].I);
            }
            try
            {
                for (int j = 0; j < jSONObject3.Count; j++)
                {
                    attaker.OtherAvatar.spell.addDBuff(jSONObject3[j].I, jSONObject4[j].I);
                }
            }
            catch
            {
                // 发生任何异常都静默吞掉，什么都不做
            }
        }

        private static void RealizeSeid361(StaticSkill __instance, int seid, List<int> damage, Avatar attaker, Avatar receiver, int type)
        {
            __instance.resetSeidFlag(seid, attaker);
            if (!attaker.isPlayer())
            {
                attaker.setHP(attaker.HP + attaker.HP * ((int)__instance.getSeidJson(seid)["value1"].n / 100));
            }
        }
    }
}