using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.YouZhou.Patch
{
    [HarmonyPatch(typeof(clientApp), "InitVersion")]
    public class ClientApp_InitVersion_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(clientApp __instance)
        {
            // 原逻辑已执行完毕，现在覆盖窗口标题
            if (GameWindowTitle.Inst != null)
            {
                GameWindowTitle.Inst.SetTitle($"觅长生2");
            }
        }
    }
}
