using Antlr4.Runtime.Atn;
using HarmonyLib;
using MaiJiu.MCS.HH.Scene;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine.UI;

namespace top.Isteyft.MCS.IsTools.Patch
{
    //[HarmonyPatch(typeof(SceneBtnMag))]
    public class SceneButtonPatch
    {
        /// <summary>
        /// 可进行“闭关”或“休息”操作的弟子居所或客房场景。
        /// 这些场景通常位于宗门弟子居所或各城客栈续费后的房间内。
        /// </summary>
        private static readonly HashSet<string> TargetScenes = new HashSet<string>
        {
            "S27005", // 天魔道 - 弟子居所
            "S27104", // 天雪剑宗 - 弟子居所
            "S27304", // 长风城 - 客房（客栈续费后）
            "S27312", // 长风城 - 苏府·居所
            "S27324", // 浅湾城 - 客房（客栈续费后）
            "S27332", // 浅湾城 - 白府·居所
            "S27344", // 幽篁城 - 客房（客栈续费后）
            "S27352", // 幽篁城 - 安府·居所
            "S27364", // 上雪城 - 客房（客栈续费后）
            "S27372", // 上雪城 - 任府·居所
            "S27384", // 天魔城 - 客房（客栈续费后）
            "S27392"  // 天魔城 - 朱府·居所
        };

        /// <summary>
        /// 可采集“灵田”的野外场景。
        /// 这些场景位于幽州各大山脉/区域的灵田点，用于资源采集。
        /// </summary>
        private static readonly HashSet<string> caijiScene = new HashSet<string>
        {
            "S27402", // 沽幽山 - 幽谷（灵田）
            "S27412", // 苍雾山 - 雾谷（灵田）
            "S27422", // 长暮山 - 日暮崖（灵田）
            "S27432", // 北雪九峰 - 千寒原（灵田）
            "S27442"  // 尘山 - 淤尘脉（灵田）
        };

        /// <summary>
        /// 可进入“裂隙”副本进行采集或战斗的洞窟/据点场景。
        /// 这些通常是矿洞、秘境或敌对势力据点，用于副本类资源获取。
        /// </summary>
        private static readonly HashSet<string> caikuangScene = new HashSet<string>
        {
            "S27401", // 沽幽山 - 幽泉洞（裂隙）
            "S27411", // 苍雾山 - 八峦脉（裂隙）
            "S27421", // 长暮山 - 落砚脉（裂隙）
            "S27431", // 北雪九峰 - 摩冬窟（裂隙）
            "S27441", // 尘山 - 沙泉（裂隙）
            "S27451", // 丘魏矿脉 - 叁乌洞（裂隙）
            "S27452", // 丘魏矿脉 - 维漠洞（裂隙）
            "S27453" // 丘魏矿脉 - 昆禹洞（裂隙）
        };

        //[HarmonyPatch(typeof(SceneBase), "SetSceneBtnList")]
        //[HarmonyPatch("Start")]
        [HarmonyPatch(typeof(Tools), "loadMapScenes")]
        [HarmonyPostfix]
        //public static void ReplaceTupoWithGanying(SceneBase __instance)
        public static void ReplaceTupoWithGanying()
        {
            if (!ModUtil.CheckModActive("3018994631")) return;
            try
            {
                if (SceneBase.Inst == null)
                {
                    IsToolsMain.LogInfo("[DEBUG] SceneBase.Inst 为 null！");
                    return; // 避免崩溃
                }

                SceneBase inst = SceneBase.Inst;
                IsToolsMain.LogInfo($"[DEBUG] 当前场景: {inst.sceneName}");
                if (!TargetScenes.Contains(inst.sceneName)) return;
                var btnDictField = AccessTools.Field(typeof(SceneBtnMag), "btnDictionary");
                var btnDict = btnDictField?.GetValue(SceneBtnMag.inst) as Dictionary<string, FpBtn>;
                if (btnDict == null)
                {
                    IsToolsMain.LogInfo("[SceneButtonPatch] btnDictionary 获取失败");
                    return;
                }

                if (btnDict.TryGetValue("tupo", out var btn) && PlayerEx.Player.getLevelType() == 5)
                {
                    SpriteRef component = btn.GetComponent<SpriteRef>();
                    btn.GetComponent<Image>().sprite = component.sprites[0];
                    btn.nomalSprite = component.sprites[0];
                    btn.mouseEnterSprite = component.sprites[1];
                    btn.mouseDownSprite = component.sprites[2];
                    btn.mouseUpSprite = component.sprites[0];
                    btn.stopClickSprite = component.sprites[0];
                    btn.transform.GetChild(0).GetComponent<Image>().sprite = component.sprites[3];
                }
            } catch (Exception e)
            {
                IsToolsMain.Error(e);
            }
        }

        //[HarmonyPatch("Start")]
        //[HarmonyPostfix]
        //public static void ReplaceCaiKuangWithGanying(SceneBtnMag __instance)
        //{
        //    SceneBase inst = SceneBase.Inst;
        //    if (!caikuangScene.Contains(inst.sceneName))
        //        return;
        //    var btnDictField = AccessTools.Field(typeof(SceneBtnMag), "btnDictionary");
        //    var btnDict = btnDictField?.GetValue(SceneBtnMag.inst) as Dictionary<string, FpBtn>;
        //    if (btnDict == null)
        //    {
        //        IsToolsMain.LogInfo("[SceneButtonPatch] btnDictionary 获取失败");
        //        return;
        //    }

        //    if (btnDict.TryGetValue("caiji", out var btn) && PlayerEx.Player.getLevelType() == 5)
        //    {
        //        SpriteRef component = btn.GetComponent<SpriteRef>();
        //        btn.GetComponent<Image>().sprite = component.sprites[0];
        //        btn.nomalSprite = component.sprites[0];
        //        btn.mouseEnterSprite = component.sprites[1];
        //        btn.mouseDownSprite = component.sprites[2];
        //        btn.mouseUpSprite = component.sprites[0];
        //        btn.stopClickSprite = component.sprites[0];
        //        btn.transform.GetChild(0).GetComponent<Image>().sprite = component.sprites[3];
        //    }
        //}
    }
}
