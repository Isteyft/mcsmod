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
using top.Isteyft.MCS.JiuZhou.Scene;
using UnityEngine;

namespace top.Isteyft.MCS.JiuZhou.Utils
{
    public class MyUtil
    {
        public static bool init = false;
        public static bool XJYinit = false;
        public static bool ZZinit = false;
        public static bool HZinit = false;
        public static bool BZinit = false;
        public static bool YingZhouinit = false;
        public static bool JZinit = false;
        public static bool YuZhouinit = false;
        public static bool YongZhouinit = false;
        public static Dictionary<string, bool> MapInit = new Dictionary<string, bool>
        {
            { "幽州初始化", false },
            { "雪剑域初始化", false },
            { "中州初始化", false },
            { "衡州初始化", false },
            { "灞州初始化", false },
            { "颍州初始化", false },
            { "靖州初始化", false },
            { "渝州初始化", false },
            { "雍州初始化", false },
            { "灵颖城初始化", false },
            { "绯妖林初始化", false },
        };
        public static bool HasMod(string steamId)
        {
            return (from directory in WorkshopTool.GetAllModDirectory()
                    where directory.Name.Equals(steamId)
                    select directory).Any((DirectoryInfo directory) => !WorkshopTool.CheckModIsDisable(steamId));
        }

        #region 九州进入
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
        
        // 衡州
        public static void LoadHZ(int index = 510)
        {
            if (!HZinit)
            {
                HZinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/衡州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F衡州"))
            {
                Tools.instance.loadMapScenes("F衡州", false);
            }
            else
            {
                LoadFuBen.loadfuben("F衡州", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }
        public static void LoadHZNoMapScenes(int index = 510)
        {
            if (!HZinit)
            {
                HZinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/衡州.ab";
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("F衡州", index);
            AllMapBase.RefreshMarksFromStaticData();
        }
        
        // 灞州
        public static void LoadBZ(int index = 710)
        {
            if (!BZinit)
            {
                BZinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/灞州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F灞州"))
            {
                Tools.instance.loadMapScenes("F灞州", false);
            }
            else
            {
                LoadFuBen.loadfuben("F灞州", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }
        public static void LoadBZNoMapScenes(int index = 710)
        {
            if (!BZinit)
            {
                BZinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/灞州.ab";
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("F灞州", index);
            AllMapBase.RefreshMarksFromStaticData();
        }
        
        // 颍州
        public static void LoadYingZhou(int index = 74012)
        {
            if (!YingZhouinit)
            {
                YingZhouinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/颍州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F颍州"))
            {
                Tools.instance.loadMapScenes("F颍州", false);
            }
            else
            {
                LoadFuBen.loadfuben("F颍州", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }
        public static void LoadYingZhouNoMapScenes(int index = 901)
        {
            if (!YingZhouinit)
            {
                YingZhouinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/颍州.ab";
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("F颍州", index);
            AllMapBase.RefreshMarksFromStaticData();
        }
        
        // 靖州
        public static void LoadJZ(int index = 1101)
        {
            if (!JZinit)
            {
                JZinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/靖州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F靖州"))
            {
                Tools.instance.loadMapScenes("F靖州", false);
            }
            else
            {
                LoadFuBen.loadfuben("F靖州", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }
        public static void LoadJZNoMapScenes(int index = 1101)
        {
            if (!JZinit)
            {
                JZinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/靖州.ab";
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("F靖州", index);
            AllMapBase.RefreshMarksFromStaticData();
        }
        
        // 渝州
        public static void LoadYuZhou(int index = 1301)
        {
            if (!YuZhouinit)
            {
                YuZhouinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/渝州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F渝州"))
            {
                Tools.instance.loadMapScenes("F渝州", false);
            }
            else
            {
                LoadFuBen.loadfuben("F渝州", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }
        public static void LoadYuZhouNoMapScenes(int index = 1310)
        {
            if (!YuZhouinit)
            {
                YuZhouinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/渝州.ab";
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("F渝州", index);
            AllMapBase.RefreshMarksFromStaticData();
        }
        
        // 雍州
        public static void LoadYongZhou(int index = 1510)
        {
            if (!YongZhouinit)
            {
                YongZhouinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/雍州.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F雍州"))
            {
                Tools.instance.loadMapScenes("F雍州", false);
            }
            else
            {
                LoadFuBen.loadfuben("F雍州", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }
        public static void LoadYongZhouNoMapScenes(int index = 1501)
        {
            if (!YongZhouinit)
            {
                YongZhouinit = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/雍州.ab";
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("F雍州", index);
            AllMapBase.RefreshMarksFromStaticData();
        }
        #endregion

        #region 一级地图进入
        public static void LoadS74000(int index = 740006)
        {
            if (!MapInit["灵颖城初始化"])
            {
                MapInit["灵颖城初始化"] = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/s74000.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("S74000"))
            {
                Tools.instance.loadMapScenes("S74000", false);
            }
            else
            {
                LoadFuBen.loadfuben("S74000", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }

        public static void LoadS74000NoMapScenes(int index = 740006)
        {
            if (!MapInit["灵颖城初始化"])
            {
                MapInit["灵颖城初始化"] = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/s74000.ab";
                IsToolsMain.LogInfo(path);
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("S74000", index);
            AllMapBase.RefreshMarksFromStaticData();
        }
        
        public static void LoadS74200(int index = 742102)
        {
            if (!MapInit["绯妖林初始化"])
            {
                MapInit["绯妖林初始化"] = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/s74200.ab";
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("S74200"))
            {
                Tools.instance.loadMapScenes("S74200", false);
            }
            else
            {
                LoadFuBen.loadfuben("S74200", index);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }

        public static void LoadS74200NoMapScenes(int index = 742102)
        {
            if (!MapInit["绯妖林初始化"])
            {
                MapInit["绯妖林初始化"] = true;
                string path = IsToolsMain.dll + "/BaizeAssets/AssetBundle/Scene/s74200.ab";
                IsToolsMain.LogInfo(path);
                AssetBundle.LoadFromFile(path);
            }
            LoadFuBen.loadfuben("S74200", index);
            AllMapBase.RefreshMarksFromStaticData();
        }
        #endregion
    }

}
