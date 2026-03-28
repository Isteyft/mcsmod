using DebuggingEssentials;
using Google.Protobuf.WellKnownTypes;
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
        // 获取能制造的上限
        public static int GetZhiZuoFuLuNum(int fuLuItemID)
        {
            bool flag = true;
            Avatar avatar = Tools.instance.getPlayer();
            var min = int.MaxValue;
            FuLuItemData fuLuItem = FuLuItemData.GetFuLuItemDataByFuLuId(fuLuItemID);
            for (int i = 0; i < fuLuItem.FuLuItem.Length; i++)
            {
                min = Math.Min(avatar.getItemNum(fuLuItem.FuLuItem[i]) / fuLuItem.FuLuItemNum[i], min);
            }
            return min;
        }
        // 消耗制作符箓的材料
        public static void RemoveZhiZuoFuLuNum(FuLuItemData fuLuItem, int num)
        {
            float p = 1f;
            if (WudaoUtil.HasWuDaoSkill(2313)) p -= 0.3f;
            for (int i = 0; i < fuLuItem.FuLuItem.Length; i++)
            {
                IsToolsMain.LogInfo($"消耗{ItemUtil.GetItemName(fuLuItem.FuLuItem[i])}数量:{(int)Math.Ceiling(fuLuItem.FuLuItemNum[i] * num * p)}");
                Tools.instance.getPlayer().removeItem(fuLuItem.FuLuItem[i], (int)Math.Ceiling(fuLuItem.FuLuItemNum[i] * num * p));
            }
        }
        
        // 制作符箓
        public static void ZhiZuoFuLu(int fuLuItemID)
        {
            Avatar avatar = Tools.instance.getPlayer();
            FuLuItemData fuLuItem = FuLuItemData.GetFuLuItemDataByFuLuId(fuLuItemID);
            int count = GetZhiZuoFuLuNum(fuLuItemID);
            if (count <= 0)
            {
                UIPopTip.Inst.Pop($"制作{ItemUtil.GetItemName(fuLuItemID)}的材料数量不足");
                return;
            }
            USelectNum.Show(
                desc: "请选择制作"+  ItemUtil.GetItemName(fuLuItemID) +"数量：x{num}",
                minNum: 1,
                maxNum: count,
                OK: (selectedNumber) =>
                {
                    IsToolsMain.Log($"玩家选区制作{ItemUtil.GetItemName(fuLuItemID)}符箓：{selectedNumber}张");
                    // 拥有省材时制作
                    RemoveZhiZuoFuLuNum(fuLuItem, selectedNumber);
                    // 制作张数
                    int num = 0;
                    int noSuccessNum = 0;
                    int successNum = 0;
                    // 额外产出的概率
                    int p0 = 0;
                    // 凝符增加10%的额外产出1张符箓的概率
                    if (WudaoUtil.HasWuDaoSkill(2321)) p0 += 10;
                    if (WudaoUtil.HasWuDaoSkill(2332)) p0 += 20;
                    if (WudaoUtil.HasWuDaoSkill(2341)) p0 += 30;
                    System.Random random = new System.Random();
                    for (int i = 0; i < selectedNumber; i++)
                    {
                        int nowNum = 1;
                        int randomP0 = random.Next(1, 101);
                        if (p0 > 0 && randomP0 <= p0) nowNum += 1;
                        if(WudaoUtil.HasWuDaoSkill(2341))
                        {
                            nowNum *= 2;
                            successNum += 1;
                            num += nowNum;
                        }
                        else
                        {
                            int randomValue = random.Next(1, 101);
                            if (randomValue < (50 - fuLuItem.FuLuLevel * 15))
                            {
                                successNum += 1;
                                num += nowNum;
                            }
                            else
                            {
                                noSuccessNum += 1;
                            }
                        }
                    }

                    avatar.AddTime(fuLuItem.FuLuTime*selectedNumber, 0, 0);
                    if (noSuccessNum > 0)
                    {
                        UIPopTip.Inst.Pop($"制作失败{ItemUtil.GetItemName(fuLuItemID)}符箓：{noSuccessNum}次。");
                    }
                    if (successNum > 0)
                    {
                        UIPopTip.Inst.Pop($"花费{fuLuItem.FuLuTime * selectedNumber}天，获得{ItemUtil.GetItemName(fuLuItemID)}*{num}", PopTipIconType.包裹);
                        IsToolsMain.Log($"制作成功{ItemUtil.GetItemName(fuLuItemID)}符箓：{num}张");
                        UIPopTip.Inst.Pop($"获得符道经验{fuLuItem.FuLuExp * successNum}点", PopTipIconType.上箭头);
                        avatar.addItem(fuLuItemID, num, null, false);
                        avatar.wuDaoMag.addWuDaoEx(fuLuItem.FuLuExp * successNum, 23);
                    }
                },
                Cancel: () =>
                {
                    // 用户点了取消，什么也不做或处理取消逻辑
                    IsToolsMain.LogInfo("取消制作符箓");
                }
            );
        }

    }
}
