using HarmonyLib;
using SkySwordKill.NextModEditor.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsMoXinXiangJuan.Data;

namespace top.Isteyft.MCS.IsMoXinXiangJuan.Patch
{
    [HarmonyPatch(typeof(ModEditorManager))]
    public class ModPatch
    {
        [HarmonyPatch(nameof(ModEditorManager.Init)), HarmonyPostfix]
        private static void Init_Postfix()
        {
            AttackTypeMetaData.Inst.Load();
            StaticSkillTypeMetaData.Inst.Load();
        }
    }
}
