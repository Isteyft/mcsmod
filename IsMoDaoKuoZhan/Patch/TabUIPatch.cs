using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Antlr4.Runtime.Misc;
using Fungus;
using Google.Protobuf.WellKnownTypes;
using GUIPackage;
using HarmonyLib;
using KBEngine;
using SkySwordKill.NextMoreCommand.NextEnvExtension.Utils;
using Tab;
using top.Isteyft.MCS.IsMoDaoKuoZhanMain.Utils;
using UnityEngine;
using UnityEngine.UI;
using YSGame.Fight;
using Avatar = KBEngine.Avatar;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
{
    [HarmonyPatch(typeof(TabShuXingPanel))]
    public class TabUIPatch
    {

        [HarmonyPostfix]
        [HarmonyPatch("Init")]
        private static void TabUIMag_Init_Patch(TabShuXingPanel __instance)
        {
            UnityEngine.GameObject gameObject = typeof(TabShuXingPanel).GetField("_go", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) as UnityEngine.GameObject;
            Transform transform = gameObject.transform.Find("LingGen/金灵根");
            Transform transform2 = UnityEngine.Object.Instantiate<Transform>(transform, transform.transform.parent);
            transform2.name = "魔灵根";
            transform2.localPosition = new Vector3(388.8f, 5.7f, 0f);
            transform2.Find("BG/BG").GetComponent<Image>().sprite = IsMoDaoKuoZhanMain.I.UIManagerHandle.spriteBank["mo.png"];
        }

        [HarmonyPostfix]
        [HarmonyPatch("UpdateUI")]
        private static void TabUIMag_UpdateUIPatch(TabShuXingPanel __instance)
        {
            UnityEngine.GameObject gameObject = typeof(TabShuXingPanel).GetField("_go", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) as UnityEngine.GameObject;
            Avatar player = Tools.instance.getPlayer();
            int quanzhong = 0;
            quanzhong = quanzhong + (GetQuanZhong(272006) * 2);
            if (Tools.instance.CheckHasTianFu(319)) { quanzhong = quanzhong + 5; }
            quanzhong = quanzhong + CalculateJieDanSkillValue(player);
            gameObject.transform.Find("LingGen/魔灵根/Value").GetComponent<Text>().text = quanzhong.ToString();
        }
        public static int GetQuanZhong(int targetBuffId)
        {
            Avatar player = Tools.instance.getPlayer();
            int count = 0;
            string tianFuKey = 16.ToString();
            if (player.TianFuID.HasField(tianFuKey))
            {
                JSONObject buffArray = player.TianFuID[tianFuKey];
                for (int i = 0; i < buffArray.Count; i++)
                {
                    if ((int)buffArray[i].i == targetBuffId)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public static int CalculateJieDanSkillValue(Avatar player)
        {
            if (player.hasJieDanSkillList == null || player.hasJieDanSkillList.Count == 0)
                return 0; // 如果没有技能，返回 -1 或其他默认值

            // 只取第一个技能
            int jindanquanzhong = 0;
            foreach (SkillItem hasJieDanSkill in player.hasJieDanSkillList)
            {
                int jiedanid = hasJieDanSkill.itemId;
                if (jiedanid >= 151 && jiedanid <= 159)
                {
                    jindanquanzhong = jindanquanzhong + (jiedanid - 150) * 2 + 2;
                }
            }
            return jindanquanzhong;
        }

    }
}
