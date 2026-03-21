using Bag;
using HarmonyLib;
using JSONClass;
using SkySwordKill.NextMoreCommand.Custom.RealizeSeid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static InvGameItem;

namespace top.Isteyft.MCS.IsTools.Patch.UI.EquipSlot
{
    /// <summary>
    /// 修复 EquipItem 的 CanPutSlotType，使其支持 type=18（灵兽）
    /// </summary>
    [HarmonyPatch(typeof(EquipItem))]
    public class EquipItemPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("SetItem", new Type[] { typeof(int), typeof(int), typeof(JSONObject) })]
        public static void Postfix(EquipItem __instance)
        {
            //IsToolsMain.LogInfo("EquipItem.SetItem" + __instance.Type);
            // 只处理 type == 18 的物品（灵兽）
            if (__instance.Type != 18) return;

            try
            {
                // 安全获取 CanSlotType 枚举类型（即使它是 internal）
                var canSlotType = AccessTools.TypeByName("CanSlotType");
                if (canSlotType == null)
                {
                    IsToolsMain.Error("无法找到 CanSlotType 枚举类型！");
                    return;
                }

                // 创建值为 18 的枚举实例
                var enumValue = Enum.ToObject(canSlotType, 18);

                // 通过反射设置 CanPutSlotType 字段
                var field = AccessTools.Field(typeof(EquipItem), "CanPutSlotType");
                if (field != null)
                {
                    field.SetValue(__instance, enumValue);
                }
            }
            catch (Exception ex)
            {
                IsToolsMain.Error(ex);
            }
        }

        //[HarmonyPrefix]
        //[HarmonyPatch("GetImgQuality")]
        //public static int GetImgQualityPatch(EquipItem __instance)
        //{
        //    if (__instance.Type == 18)
        //    {
        //        FieldInfo field = typeof(EquipItem).GetField("Quality", BindingFlags.NonPublic | BindingFlags.Instance);
        //        int currentQuality = (int)field.GetValue(__instance);
        //        return currentQuality;
        //    }
        //    return 0;
        //}

        [HarmonyPostfix]
        [HarmonyPatch("GetImgQuality")]
        public static void GetImgQualityPatch(EquipItem __instance, ref int __result)
        {
            if (__instance.Type == 18)
            {
                __result = __result - 1;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("GetEquipType")]
        public static bool GetEquipTypePatch(EquipItem __instance, EquipType __result)
        {
            if (__instance.Type == 18)
            { 
                __result = (EquipType)18;
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("GetQualityName")]
        public static bool GetQualityNamePatch(EquipItem __instance, ref string __result)
        {
            if (__instance.Type == 18)
            {
                int qualityValue = Traverse.Create(__instance).Field("Quality").GetValue<int>();
                __result = qualityValue.ToCNNumber() + "品";
                return false;
            }
            return true;
        }
    }
}
