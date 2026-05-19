using HarmonyLib;
using MaiJiu.MCS.HH.Scene;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UltimateSurvival.ItemProperty;

namespace top.Isteyft.MCS.JiuZhou.Patch
{

    //[HarmonyPatch(typeof(Tools), "loadMapScenes")]
    //[HarmonyPatch(typeof(SceneBtnMag), "AutoHide")]
    [HarmonyPatch(typeof(SceneBtnMag), "Start")]
    //[HarmonyPatch(typeof(SceneBase), "SetSceneBtnList")]
    public class SceneButtonPatch
    {

        public static Dictionary<string, string> SceneBtns = new Dictionary<string, string>
        {
            { "离开", "likai" },
            { "传送阵", "DFMode" },
            { "石台", "shitai" },
            { "炼器", "DFLianQi" },
            { "炼丹", "DFLianDan" },
            { "灵田", "DFLingTian" },
            { "裂隙", "liexi" },
            { "上楼", "shanglou" },
            { "出海", "chuhai" },
            { "神兵阁", "shenbingge" },
            { "药房", "yaofang" },
            { "客房", "kefang" },
            { "静室", "jingshi" },
            { "藏经阁", "shop" },
            { "突破", "tupo" },
            { "感应", "ganying" },
            { "装扮", "zhuangban" },
            { "派发", "paifa" },
            { "交易会", "jiaoyihui" },
            { "闭关", "biguan" },
            { "休息", "xiuxi" },
            { "气旋", "qixuan" },
            { "告示", "gaoshi" },
            { "修船", "xiuchuan" },
            { "比试", "dabi" },
            { "藏宝阁", "cangbaoge" },
            { "采集", "caiji" },
            { "副本采集", "caiji" }
        };

        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        public static void ReplaceTupoWithGanying()
        {

            if (FuBenBase.Inst != null || SeaBase.Inst != null)
            {
                return;
            }

            if (SceneEx.NowSceneName != "S74200") return;
            Dictionary<string, FpBtn> fpValue = Traverse.Create(SceneBtnMag.inst).Field("btnDictionary").GetValue<Dictionary<string, FpBtn>>();

            if (DialogAnalysis.GetInt("颍州-潜鳞城开启")==1)
            {
                FpBtn fpBtn = fpValue["liexi"];
                fpBtn.gameObject.SetActive(value: true);
                fpBtn.mouseUpEvent.RemoveAllListeners();
                fpBtn.mouseUpEvent.AddListener(delegate
                {
                    Tools.instance.loadMapScenes("S74100");
                });
            }
        }

    }
}
