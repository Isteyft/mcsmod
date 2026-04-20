using GetWay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.JiuZhou.Scene
{
    public class YZMove
    {

        public static void Refresh(MapComponent components)
        {
            if (MapGetWay.Inst.Dict.Count == 0) MapGetWay.Inst.IsNearly(1, 2);
            var index = int.Parse(components.gameObject.name);
            MapGetWay.Inst.Dict[index] = new List<int>(components.nextIndex);
            MapGetWay.Inst.NodeDict[index] = new MapNode(index, components.transform.position.x, components.transform.position.y);
        }
    }
}
