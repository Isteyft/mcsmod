using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.JiuZhou.Scene.sea
{
    [HarmonyPatch(typeof(Fungus.LoadFuBen))]
    public class LoadToOtherSea
    {
        [HarmonyPatch("loadfuben")]
        public static bool Prefix(string fubenName, int positon)
        {
            IsToolsMain.LogInfo(fubenName);
            IsToolsMain.LogInfo(positon);
            if (fubenName.StartsWith("Sea"))
            {
                // sea3 西宁海  Sea18无尽海渊 Sea11 雷鸣海 Sea8 蓬莎海域 Sea9 浪方海域
                if (
                    (fubenName == "Sea3" || fubenName == "Sea18" || fubenName == "Sea11" || fubenName == "Sea8" || fubenName == "Sea9") &&
                    positon > 18000
                    )
                {
                    positon -= 31521;

                    new Thread(Fungus.LoadFuBen.methodName).Start();
                    Tools.instance.getPlayer().fubenContorl[fubenName].setFirstIndex(positon);
                    Tools.instance.getPlayer().zulinContorl.kezhanLastScence = Tools.getScreenName();
                    Tools.instance.loadMapScenes(fubenName);
                    Tools.instance.getPlayer().lastFuBenScence = Tools.getScreenName();
                    Tools.instance.getPlayer().NowFuBen = fubenName;
                    return false;
                }
            }
            return true;
        }
    }
}
