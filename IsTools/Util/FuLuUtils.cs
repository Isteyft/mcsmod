using DebuggingEssentials;
using KBEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Data;
using XLua.Cast;
using static UltimateSurvival.ItemProperty;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class FuLuUtils
    {
        // 判断能否制作一定数量的符箓
        public static bool CanZhiZuoFuLuNum(int fuLuItemID, int num=1)
        {
            bool flag = true;
            Avatar avatar = Tools.instance.getPlayer();
            FuLuItemData fuLuItem = FuLuItemData.GetFuLuItemDataByFuLuId(fuLuItemID);
            for (int i = 0; i < fuLuItem.FuLuItem.Length; i++)
            {
                if (avatar.getItemNum(fuLuItem.FuLuItem[i]) != fuLuItem.FuLuItemNum[i]*num)
                {
                    UIPopTip.Inst.Pop($"{ItemUtil.GetItemName(i)}数量不足");
                    flag = false;
                }
            }
            return flag;
        }
        // 消耗制作符箓的材料
        public static void RemoveZhiZuoFuLuNum(FuLuItemData fuLuItem, int num = 1)
        {
            float p = 1f;
            if (WudaoUtil.HasWuDaoSkill(2313)) p -= 0.3f;
            for (int i = 0; i < fuLuItem.FuLuItem.Length; i++)
            {
                Tools.instance.getPlayer().removeItem(fuLuItem.FuLuItem[i], (int)Math.Ceiling(fuLuItem.FuLuItemNum[i] * p));
            }
        }
        // 制作符箓
        public static void ZhiZuoFuLu(int fuLuItemID, int count=1)
        {
            Avatar avatar = Tools.instance.getPlayer();
            FuLuItemData fuLuItem = FuLuItemData.GetFuLuItemDataByFuLuId(fuLuItemID);
            if (!CanZhiZuoFuLuNum(fuLuItemID, 1)) return;
            // 拥有省材时制作
            RemoveZhiZuoFuLuNum(fuLuItem, 1);
            int num = 1;
            // 额外产出的概率
            int p0 = 0;
            int randomP0 = new System.Random().Next(1, 101);
            // 凝符增加10%的额外产出1张符箓的概率
            if (WudaoUtil.HasWuDaoSkill(2321)) p0 += 10;
            if (WudaoUtil.HasWuDaoSkill(2332)) p0 += 20;
            if (WudaoUtil.HasWuDaoSkill(2341)) p0 += 30;
            if (p0 > 0 && randomP0 <= p0) num += 1;

            avatar.AddTime(fuLuItem.FuLuTime, 0, 0);

            if (WudaoUtil.HasWuDaoSkill(2341))
            {
                num *= 2;
            }
            else
            {
                // 生成 1-100 的随机数
                int randomValue = new System.Random().Next(1, 101);
                if (randomValue > (50 - fuLuItem.FuLuLevel * 15 - 15))
                {
                    UIPopTip.Inst.Pop($"花费{fuLuItem.FuLuTime}天，{ItemUtil.GetItemName(fuLuItemID)}制作失败。");
                    return;
                }
            }
            UIPopTip.Inst.Pop($"花费{fuLuItem.FuLuTime}天，获得{ItemUtil.GetItemName(fuLuItemID)}*{num}", PopTipIconType.包裹);
            avatar.wuDaoMag.addWuDaoEx(fuLuItem.FuLuExp, 23);
        }

    }
}
