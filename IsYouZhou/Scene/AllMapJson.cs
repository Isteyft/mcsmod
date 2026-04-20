using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.JiuZhou.Scene
{
    public class AllMapJson
    {
        // 大地图数据
        public string Name { get; set; }
        public string Music { get; set; }
        public Dictionary<int, LudianJson> LuDian { get; set; }
    }
}
