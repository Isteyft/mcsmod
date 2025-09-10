using HarmonyLib;
using SkySwordKill.Next;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsMoDaoKuoZhanMain.Util;
using UnityEngine.Events;
using UnityEngine;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.ModPatch.LunDaoPatch
{
    public class SelectLunTiPatch
    {
        [HarmonyPatch(typeof(SelectLunTi), "Init")]
        [HarmonyPostfix]
        public static void Init_Postfix(SelectLunTi __instance)
        {
            Transform transform = __instance.transform.Find("LunTiList");
            if (transform != null)
            {
                string name = transform.gameObject.name;
                List<Sprite> value = Traverse.Create(__instance).Field("selectSprites").GetValue<List<Sprite>>();
                foreach (JSONObject jsonobject in jsonData.instance.WuDaoAllTypeJson.list)
                {
                    int i = jsonobject["id"].I;
                    if (i == 11)
                    {
                        string lunDaoName = JsonUtil.GetLunDaoName(i);
                        IsMoDaoKuoZhanMain.LogInfo("新增魔道悟道");
                        GameObject value2 = Traverse.Create(__instance).Field("lunTiCell").GetValue<GameObject>();
                        LunTiCell component = UnityEngine.Object.Instantiate<GameObject>(value2, transform).gameObject.GetComponent<LunTiCell>();
                        Sprite sprite = null;
                        try
                        {
                            sprite = SpriteUtil.GetLunDaoSprite(1, i);
                        }
                        catch
                        {
                            IsMoDaoKuoZhanMain.LogInfo("魔道并未提供相应论道支持");
                            continue;
                        }
                        if (sprite != null)
                        {
                            component.InitLunTiCell(sprite, value[0], i, lunDaoName, new UnityAction<int>(__instance.AddLunTiToList), new UnityAction<int>(__instance.RemoveLunTiByList));
                            IsMoDaoKuoZhanMain.LogInfo("已为魔道添加新的论题，祝您论道愉快！");
                            component.gameObject.SetActive(true);
                        }
                        else
                        {
                            IsMoDaoKuoZhanMain.LogInfo("魔道并未提供相应论道支持，进行跳过");
                        }
                    }
                }
            }
            else
            {
                IsMoDaoKuoZhanMain.LogInfo("无法获取相关父对象");
            }
        }
    }
}
