using System;
using System.Collections.Generic;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    [System.Serializable]
    public class MapMoveData
    {
        public bool canmove;
        public List<int> canmoveIndex;

        public MapMoveData()
        {
            canmoveIndex = new List<int>();
        }
    }

    [System.Serializable]
    public class MapMoveConfig
    {
        public Dictionary<string, MapMoveData> mapMoveDatas;
    }
}