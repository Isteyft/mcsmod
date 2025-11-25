using DebuggingEssentials;
using Fungus;
using SkySwordKill.Next;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;
using top.Isteyft.MCS.YouZhou.Scene;
using UnityEngine;

namespace top.Isteyft.MCS.YouZhou.Utils
{
    public class MyUtil
    {
        public static bool init = false;
        public static bool XJYinit = false;
        public static bool ZZinit = false;
        public static bool HasMod(string steamId)
        {
            return (from directory in WorkshopTool.GetAllModDirectory()
                    where directory.Name.Equals(steamId)
                    select directory).Any((DirectoryInfo directory) => !WorkshopTool.CheckModIsDisable(steamId));
        }
        public static void LoadYouZhou(int index = 1)
        {
            if (!init)
            {
                init = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/幽州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F幽州"))
            {
                Tools.instance.loadMapScenes("F幽州", false);
            }
            else
            {
                LoadFuBen.loadfuben("F幽州", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }

        public static void LoadYouZhouNoMapScenes(int index = 1)
        {
            if (!init)
            {
                init = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/幽州.ab";
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("F幽州", index);
            AllMapBase.RefreshMarksFromStaticData();
        }

        public static void LoadXJY(int index = 1000)
        {
            if (!XJYinit)
            {
                XJYinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/雪剑域.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F雪剑域"))
            {
                Tools.instance.loadMapScenes("F雪剑域", false);
            }
            else
            {
                LoadFuBen.loadfuben("F雪剑域", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }
        public static void LoadXJYNoMapScenes(int index = 1000)
        {
            if (!XJYinit)
            {
                XJYinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/雪剑域.ab";
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("F雪剑域", index);
            AllMapBase.RefreshMarksFromStaticData();
        }

        public static void LoadZZ(int index = 310)
        {
            if (!ZZinit)
            {
                ZZinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/中州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F中州"))
            {
                Tools.instance.loadMapScenes("F中州", false);
            }
            else
            {
                LoadFuBen.loadfuben("F中州", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }
        public static void LoadZZNoMapScenes(int index = 310)
        {
            if (!ZZinit)
            {
                ZZinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/中州.ab";
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("F中州", index);
            AllMapBase.RefreshMarksFromStaticData();
        }
    }

}
