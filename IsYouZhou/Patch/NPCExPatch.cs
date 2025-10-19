using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(NPCEx))]
    public class NPCExPatch
    {
        /// <summary>
        /// 修改请求声望变化方法
        /// 如果配置启用且声望为负，则将其设为0
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch("SuoQuChangeShengWang")]
        public static bool SuoQuChangeShengWang_Patch(ref int shengWang)
        {
            string text = "";
            string text2 = "";
            if (NPCEx.SuoQuNpc.NPCType >= 750 && NPCEx.SuoQuNpc.NPCType <= 760)
            {
                PlayerEx.AddShengWang(750, shengWang, true);
                text = "幽州";
            }
            else
            {
                return true;
            }

            text2 = ((shengWang > 0) ? "增加" : "减少");
            PopTipIconType iconType = ((shengWang > 0) ? PopTipIconType.上箭头 : PopTipIconType.下箭头);
            if (shengWang != 0)
            {
                UIPopTip.Inst.Pop($"{text}声望{text2}了{shengWang}", iconType);
            }
            return false;
        }
    }
}
