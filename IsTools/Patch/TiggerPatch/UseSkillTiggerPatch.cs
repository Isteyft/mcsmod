//using Boo.Lang;
//using DebuggingEssentials;
//using HarmonyLib;
//using KBEngine;
//using SkySwordKill.NextMoreCommand.DialogTrigger;
//using SkySwordKill.NextMoreCommand.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace top.Isteyft.MCS.IsTools.Patch.TiggerPatch
//{
//    [HarmonyPatch(typeof(Skill), "VirtualspellTarget")]
//    public static class UseSkillTiggerPatch
//    {
//        [HarmonyPostfix]
//        public static void Postfix(int skillID, int targetID, Spell __instance, string uuid = "")
//        {
//            //KBEngine.Avatar avatar = (KBEngine.Avatar)_attaker;
//            //KBEngine.Avatar avatar2 = (KBEngine.Avatar)_receiver;
//            __instance.onBuffTickByType(360);
//        }
//    }
//}
