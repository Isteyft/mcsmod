using Google.Protobuf.WellKnownTypes;
using MaiJiu.MCS.HH.Tool;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Item
{
    [DialogEvent("IS_GetNpcDrop")]
    [DialogEvent("白泽_获取NPC储物袋")]
    public class IS_GetNpcDrop : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int npcId = command.GetInt(0, 0).ToNpcNewId();
            var player = env.player;
            // 1. 获取该 NPC 的 dropType
            if (!jsonData.instance.AvatarJsonData.TryGetValue(npcId.ToString(), out JSONObject avatarData))
            {
                IsToolsMain.Warning($"未找到 NPC {npcId} 的基础配置");
                return;
            }
            int dropType = (int)avatarData["dropType"].n;

            // 灵石
            if (jsonData.instance.AvatarBackpackJsonData.TryGetValue(npcId.ToString(), out JSONObject moneyData))
            {
                long baseMoney = (long)moneyData["money"].n;
                player.money += (ulong)baseMoney;
                UIPopTip.Inst.Pop($"获得灵石*{baseMoney}", PopTipIconType.包裹);
                IsToolsMain.LogInfo($"发放金钱: {baseMoney}");
            }

            // 强制生成 100% 掉落（武器 + 背包物品）
            var rewards = NpcJieSuanManager.inst.npcFight.npcDrop.dropReward(
                1f, // weapon drop rate → 100%
                1f, // backpack drop rate → 100%
                npcId
            );

            // 遍历所有掉落项，添加给玩家
            foreach (JSONObject itemData in rewards.list)
            {
                int itemId = itemData["ID"].I;
                int num = itemData["Num"].I;
                var seid = itemData["seid"];

                player.addItem(itemId, num, seid, false);
                UIPopTip.Inst.Pop($"获得{ItemUtil.GetItemName(itemId)}*{num}", PopTipIconType.包裹);
                //IsToolsMain.LogInfo($"发放物品: ID={itemId},{ItemUtil.GetItemName(itemId)}, 数量={num}");
            }

            callback?.Invoke();
        }
    }
}
