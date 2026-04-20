using HarmonyLib;
using KBEngine;
using KillSystem;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.JiuZhou.GameData;
using top.Isteyft.MCS.JiuZhou.Utils;
using UnityEngine;
using YSGame;

namespace top.Isteyft.MCS.JiuZhou.Patch
{
    [HarmonyPatch(typeof(MainUICreateAvatar), "CreateFinsh")]
    public class StartGamePatch
    {
        [HarmonyPrefix]
        public static bool CreateFinsh(MainUICreateAvatar __instance)
        {
            Tools.instance.isNewAvatar = true;
            string scene = "AllMaps";

            //PlayerPrefs.SetString("sceneToLoad", scene);
            MusicMag.instance.stopMusic();
            YSNewSaveSystem.Save(YSNewSaveSystem.GetAvatarSavePathPre(__instance.curIndex, 0) + "/FirstSetAvatarRandomJsonData.txt", 0, autoPath: false);
            YSNewSaveSystem.CreatAvatar(10, 51, 100, new Vector3(-5f, 0f, 0f), new Vector3(0f, 0f, 80f));
            KBEngineApp.app.entity_id = 10;
            Avatar avatar = (Avatar)KBEngineApp.app.player();
            //__instance.setTianfuInfo(avatar);
            // 获取 private 方法
            var method = typeof(MainUICreateAvatar)
                .GetMethod("setTianfuInfo", BindingFlags.NonPublic | BindingFlags.Instance);

            // 调用方法
            method.Invoke(__instance, new object[] { avatar });
            KillManager.Inst.Restart();
            IsToolsMain.YouZhouData.Data["IsYouZhouBirth"] = "1";
            if (Tools.instance.CheckHasTianFu(720004))
            {
                // 天魔城 朱家
                scene = "S27390";
                MainUIPlayerInfo.inst.firstName = "朱";
            }
            else if (Tools.instance.CheckHasTianFu(720005))
            {
                // 长风城 苏家
                scene = "S27310";
                MainUIPlayerInfo.inst.firstName = "苏";
            }
            else if (Tools.instance.CheckHasTianFu(720006))
            {
                // 浅湾城 白家
                scene = "S27330";
                MainUIPlayerInfo.inst.firstName = "白";
            }
            else if (Tools.instance.CheckHasTianFu(720007))
            {
                // 幽篁城 安家
                scene = "S27350";
                MainUIPlayerInfo.inst.firstName = "安";
            }
            else if (Tools.instance.CheckHasTianFu(720008))
            {
                // 上雪城 任家
                scene = "S27370";
                MainUIPlayerInfo.inst.firstName = "任";
            }
            else if (Tools.instance.CheckHasTianFu(720000))
            {
                scene = "S27452";
            }
            else if (Tools.instance.CheckHasTianFu(720003))
            {
                scene = "S27510";
            }
            else if (Tools.instance.CheckHasTianFu(720009))
            {
                scene = "S27005";
            }
            else if (Tools.instance.CheckHasTianFu(720090))
            {
                if (!MyUtil.YingZhouinit)
                {
                    MyUtil.YingZhouinit = true;
                    string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/颍州.ab";
                    AssetBundle.LoadFromFile(path);
                }
                scene = "F颍州";
                avatar.fubenContorl["F颍州"].NowIndex = 916;
            }
            else
            {
                IsToolsMain.YouZhouData.Data["IsYouZhouBirth"] = "0";
            }
            PlayerPrefs.SetString("sceneToLoad", scene);

            FactoryManager.inst.createNewPlayerFactory.createPlayer(__instance.curIndex, 0, MainUIPlayerInfo.inst.firstName, MainUIPlayerInfo.inst.lastName, avatar);

            return false; 
        }
    }
}
