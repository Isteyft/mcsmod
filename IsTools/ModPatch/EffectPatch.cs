using HarmonyLib;
using SkySwordKill.Next;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.IsTools.ModPatch
{
    [HarmonyPatch(typeof(ResManager))]
    public class EffectPatch
    {
        public static Dictionary<string, UnityEngine.Object> effectdata = new Dictionary<string, UnityEngine.Object>();

        [HarmonyPatch("LoadBuffEffect")]
        [HarmonyPrefix]
        public static bool LoadBuffEffect_Prefix(string path, ref UnityEngine.Object __result)
        {
            AssetBundle assetBundle;
            // 尝试从已加载的AssetBundle中获取资源
            if (IsToolsMain.EffectAssetBundles.TryGetValue(path, out assetBundle))
            {
                // 使用缓存避免重复加载
                if (!effectdata.ContainsKey(path))
                {
                    effectdata[path] = assetBundle.LoadAsset(path);
                }
                __result = effectdata[path];
                return false;
            }
            else
            {
                return true;
            }
        }
        [HarmonyPatch("LoadSkillEffect")]
        [HarmonyPrefix]
        public static bool PrefixLoadSkillEffect(string path, ref UnityEngine.Object __result)
        {
            //HH_MaiJiu.Patch.PathPatch.PathPatch
            if (path.StartsWith("Baize"))
            {
                __result = IsToolsMain.AssetBundles["shentongeffect"].LoadAsset(path) as GameObject;
                return false;
            }
            return true;
        }
        [HarmonyPatch("CheckHasSkillEffect")]
        [HarmonyPostfix]
        public static void PostfixCheckHasSkillEffect(string path, ref bool __result)
        {
            if (!__result && path.StartsWith("Baize"))
            {
                __result = IsToolsMain.AssetBundles["shentongeffect"].LoadAsset(path) != null;
            }

        }
    }
}
