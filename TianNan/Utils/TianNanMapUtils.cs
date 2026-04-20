using Fungus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.TianNan.Scene;
using top.Isteyft.MCS.TianNan;
using UnityEngine;

namespace top.Isteyft.MCS.TianNan.Utils
{
    public class TianNanMapUtils
    {
        public static bool init = false;
        public static void LoadTianNan(int index = 1)
        {
            if (!init)
            {
                init = true;
                string path = Main.dll + "/BaizeAssets/Map/天南.ab";
                Main.LogInfo($"加载天南大地图,路径:{path}");
                AssetBundle.LoadFromFile(path);
            }
            if (PlayerEx.Player.FuBen.HasField("F天南"))
            {
                Tools.instance.loadMapScenes("F天南", false);
            }
            else
            {
                LoadFuBen.loadfuben("F天南", index);
            }
        }
    }
}
