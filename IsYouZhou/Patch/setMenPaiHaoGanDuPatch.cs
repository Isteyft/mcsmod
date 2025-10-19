using Fungus;
using HarmonyLib;
using JSONClass;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateSurvival;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Fungus.setMenPaiHaoGanDu;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(setMenPaiHaoGanDu))]
    public class setMenPaiHaoGanDuPatch
    {

        private static readonly string[] AllYZScenes = new string[]
       {
            "F幽州", "S27000", "S27001", "S27003", "S27004", "S27005", "S27006", "S27007",
            "S27008", "S27009", "S27010", "S27050", "S27100", "S27101", "S27102", "S27103", "S27104", "S27200",
            "S27210", "S27300", "S27301", "S27302", "S27303", "S27304", "S27305", "S27310", "S27311", "S27312",
            "S27313", "S27320", "S27321", "S27322", "S27323", "S27324", "S27325", "S27330", "S27331", "S27332",
            "S27333", "S27340", "S27341", "S27342", "S27343", "S27344", "S27345", "S27350", "S27351", "S27352",
            "S27353", "S27360", "S27361", "S27362", "S27363", "S27364", "S27365", "S27370", "S27371", "S27372",
            "S27373", "S27380", "S27381", "S27382", "S27383", "S27384", "S27385", "S27386", "S27387", "S27388",
            "S27390", "S27391", "S27392", "S27393", "S27400", "S27401", "S27402", "S27403", "S27410", "S27411",
            "S27412", "S27420", "S27421", "S27422", "S27430", "S27431", "S27432", "S27433", "S27440", "S27441",
            "S27442", "S27450", "S27451", "S27452", "S27453", "S27500", "S27510", "S27511", "S27512", "S27513"
       };

        public static bool IsInYZScene()
        {
            string nowScene = SceneManager.GetActiveScene().name;
            if (string.IsNullOrEmpty(nowScene))
            {
                return false;
            }
            return System.Array.IndexOf(AllYZScenes, nowScene) != -1;
        }

        /// <summary>
        /// 修改门派好感度设置入口方法
        /// 如果配置启用、值为负且NPC数量未达限制，则将其设为0
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch("OnEnter")]
        public static bool setMenPaiHaoGanDuPatchPrefix(setMenPaiHaoGanDu __instance)
        {
            // 使用 Traverse 获取私有字段值
            var traverse = Traverse.Create(__instance);
            // 获取 Type 字段（设置方式）
            SetType type = (SetType)traverse.Field("Type").GetValue();
            // 获取 MenPaiID 字段（门派ID变量）
            IntegerVariable menPaiID = traverse.Field("MenPaiID").GetValue<IntegerVariable>();
            int menPaiIdValue = menPaiID.Value; // 获取实际int值 
            // 获取 Value 字段（设置的值）
            int value = traverse.Field("Value").GetValue<int>();
            //if (menPaiID)

            //IsToolsMain.Log(menPaiIdValue.ToString());
            // 获取当前玩家对象
            Avatar player = Tools.instance.getPlayer();
            if (!(player.NowFuBen == "F幽州"))
            {
                return true;   
            }
            if (!IsInYZScene())
            {
                return true;
            }
            //if (player.OtherAvatar.AvatarType )

            if (menPaiID.Value == 0 || menPaiID.Value == 1)
            {
                menPaiID.Value = 750;
            }
            //IsToolsMain.Log(player.OtherAvatar.ToString());
            List<JSONObject> list = jsonData.instance.AvatarJsonData.list.Where(x => x["id"].I >= 20000 && !x.HasField("IsFly")).ToList();
            // 判断设置类型
            if (type == SetType.set)
            {
                // 直接设置指定门派的好感度值
                player.MenPaiHaoGanDu.SetField(menPaiID.ToString(), value);
            }
            else if (type == SetType.add)
            {
                // 获取当前门派的好感度值，如果没有则默认为0
                int currentValue = (player.MenPaiHaoGanDu.HasField(menPaiID.ToString())
                    ? player.MenPaiHaoGanDu[menPaiID.ToString()].I
                    : 0);

                // 设置新的好感度值（当前值 + 变化值）
                player.MenPaiHaoGanDu.SetField(menPaiID.ToString(), currentValue + value);

                // 根据值的变化显示不同的提示信息
                if (value > 0)
                {
                    // 好感度增加提示
                    UIPopTip.Inst.Pop(
                        "你在" + ShiLiHaoGanDuName.DataDict[menPaiID.Value].ChinaText + "的声望提升了" + value,
                        PopTipIconType.上箭头);
                }
                else if (value < 0)
                {
                    // 好感度减少提示（显示绝对值）
                    UIPopTip.Inst.Pop(
                        "你在" + ShiLiHaoGanDuName.DataDict[menPaiID.Value].ChinaText + "的声望降低了" + Mathf.Abs(value),
                        PopTipIconType.下箭头);
                }
            }

            // 继续执行下一个流程块
            __instance.Continue();
            return false;
        }
    }
}
