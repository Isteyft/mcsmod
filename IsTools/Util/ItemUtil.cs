using JSONClass;
using KBEngine;
using System;
using System.Collections.Generic;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class ItemUtil
    {
        public static int GetRandomItemTypeID(int type, int level)
        {
            List<_ItemJsonData> list = _ItemJsonData.DataList.FindAll((_ItemJsonData s) => s.type == type && s.quality == level);
            Random random = new Random();
            int index = random.Next(list.Count);
            return list[index].id;
        }
        public static _ItemJsonData GetRandomItemType(int type, int level)
        {
            List<_ItemJsonData> list = _ItemJsonData.DataList.FindAll((_ItemJsonData s) => s.type == type && s.quality == level);
            Random random = new Random();
            int index = random.Next(list.Count);
            return list[index];
        }
        public static void AddAllItem(int type, int level)
        {
            List<_ItemJsonData> list = _ItemJsonData.DataList.FindAll((_ItemJsonData s) => s.type == type && s.quality == level);
            Avatar player = Tools.instance.getPlayer();
            for (int i = 0; i < list.Count; i++)
            {
                player.addItem(list[i].id, 1, null, false);
            }
        }
        public static void AddItemList(int Min, int Max)
        {
            Avatar player = Tools.instance.getPlayer();
            for (int i = Min; i < Max + 1; i++)
            {
                player.addItem(i, 1, null, false);
            }
        }
        public static string GetItemName(int itemid)
        {
            _ItemJsonData itemJsonData = _ItemJsonData.DataDict[itemid];
            return itemJsonData.name;
        }

        public static int GetItemType(int itemid)
        {
            _ItemJsonData itemJsonData = _ItemJsonData.DataDict[itemid];
            return itemJsonData.type;
        }
    }
}
