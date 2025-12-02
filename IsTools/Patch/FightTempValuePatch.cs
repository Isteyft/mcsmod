//using HarmonyLib;
//using KBEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace top.Isteyft.MCS.IsTools.Patch
//{
//    [HarmonyPatch(typeof(KBEngine.FightTempValue))]
//    public class FightTempValuePatch
//    {
//        [HarmonyPatch("SetAttackTypeRoundDamage")]
//        [HarmonyPrefix]
//        public static bool SetAttackTypeRoundDamage_prefix(ref int attackType, int damage)
//        {
//            IsToolsMain.LogInfo("属性为1");
//            attackType = 1;
//            return true;
//        }
//    }
//} 
