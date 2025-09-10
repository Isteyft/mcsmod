using HarmonyLib;
using SkySwordKill.Next;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextModEditor.Mod.Data;
using System.Collections.Generic;
using top.Isteyft.MCS.IsTools.Data;

namespace top.Isteyft.MCS.IsTools.Patch
{

    [HarmonyPatch(typeof(ModEditorManager))]
    public class ModEditorManagerPatch
    {
        private static bool isInit;
        [HarmonyPatch("Init")]
        [HarmonyPostfix]
        public static void Postfix(ModEditorManager __instance)
        {
            if (!isInit)
            {
                if (Main.Instance.CurrentLanguage.Config.LanguageName != "中文")
                {
                    IsToolsMain.LogInfo("Next语言不为中文，取消添加Seid说明。");
                }
                else
                {
                    MetaData.Init();
                   AddItems<ModSeidMeta>(__instance.BuffSeidMetas, MetaData.BuffSeidMetas);
                    //ModEditorManagerPatch.AddItems<ModSeidMeta>(__instance.ItemEquipSeidMetas, MetaData.ItemEquipSeidMetas);
                   AddItems<ModSeidMeta>(__instance.SkillSeidMetas, MetaData.SkillSeidMetas);
                   AddItems<ModSeidMeta>(__instance.ItemUseSeidMetas, MetaData.ItemUseSeidMetas);
                   AddItems<ModSeidMeta>(__instance.StaticSkillSeidMetas, MetaData.StaticSkillSeidMeta);
                    //__instance.BuffDataTriggerTypes.AddRange(MetaData.BuffTriggerType);
                    IsToolsMain.LogInfo("已为编辑器新增Seid显示");
                   isInit = true;
                }
            }
        }

        private static void AddItems<T>(Dictionary<int, T> olddata, Dictionary<int, T> newdata)
        {
            foreach (KeyValuePair<int, T> keyValuePair in newdata)
            {
                olddata.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

    }
}
