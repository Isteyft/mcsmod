using HarmonyLib;
using SkySwordKill.NextModEditor.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsMoDaoKuoZhanMain.Data;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Patch
{
    internal class ModPatch
    {
        [HarmonyPatch(nameof(ModEditorManager.Init)), HarmonyPostfix]
        private static void Init_Postfix()
        {
            AttackTypeMetaData.Inst.Load();
            StaticSkillTypeMetaData.Inst.Load();
        }
    }
}