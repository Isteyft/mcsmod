using HarmonyLib;
using KBEngine;
using KillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YSGame;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(MainUICreateAvatar), "CreateFinsh")]
    public class StartGamePatch
    {
        [HarmonyPrefix]
        public static bool CreateFinsh(MainUICreateAvatar __instance)
        {
            Avatar avatar = (Avatar)KBEngineApp.app.player();
            Tools.instance.isNewAvatar = true;
            string scene = "AllMaps";

            PlayerPrefs.SetString("sceneToLoad", scene);
            MusicMag.instance.stopMusic();
            YSNewSaveSystem.Save(YSNewSaveSystem.GetAvatarSavePathPre(__instance.curIndex, 0) + "/FirstSetAvatarRandomJsonData.txt", 0, autoPath: false);
            YSNewSaveSystem.CreatAvatar(10, 51, 100, new Vector3(-5f, 0f, 0f), new Vector3(0f, 0f, 80f));
            KBEngineApp.app.entity_id = 10;
            //instance.setTianfuInfo(avatar);
            // 获取 private 方法
            var method = typeof(MainUICreateAvatar)
                .GetMethod("setTianfuInfo", BindingFlags.NonPublic | BindingFlags.Instance);

            // 调用方法
            method.Invoke(__instance, new object[] { avatar });
            KillManager.Inst.Restart();
            FactoryManager.inst.createNewPlayerFactory.createPlayer(__instance.curIndex, 0, MainUIPlayerInfo.inst.firstName, MainUIPlayerInfo.inst.lastName, avatar);
            return false;
        }
    }
}
