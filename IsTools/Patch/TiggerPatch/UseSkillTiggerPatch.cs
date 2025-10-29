using Boo.Lang;
using DebuggingEssentials;
using HarmonyLib;
using KBEngine;
using SkySwordKill.NextMoreCommand.DialogTrigger;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Patch.TiggerPatch
{
    [HarmonyPatch(typeof(GUIPackage.Skill), "PutingSkill")]
    public static class UseSkillTiggerPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Entity _attaker, Entity _receiver, int type = 0)
        {
            KBEngine.Avatar avatar = (KBEngine.Avatar)_attaker;
            //KBEngine.Avatar avatar2 = (KBEngine.Avatar)_receiver;
            avatar.spell.onBuffTickByType(360);
        }
    }




    //[HarmonyPatch]
    //public class UsedSkillsPatch
    //{
    //    // 定位 Avatar 类的 UsedSkills set 方法
    //    [HarmonyTargetMethod]
    //    public static System.Reflection.MethodBase TargetMethod()
    //    {
    //        return AccessTools.PropertySetter(typeof(Avatar), "UsedSkills");
    //    }

    //    // 后置拦截器
    //    [HarmonyPostfix]
    //    public static void AfterSetUsedSkills(Avatar __instance, Boo.Lang.List<int> value)
    //    {
    //        __instance.spell.onBuffTickByType(360);
    //    }
    //}






}
