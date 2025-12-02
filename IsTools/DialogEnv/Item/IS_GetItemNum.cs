using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Map
{
    [DialogEnvQuery("IS_HasItem")]
    public class IS_GetItemNum : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            int itemID = context.GetMyArgs(0, 0);
            int isPlayer = context.GetMyArgs(1, 0);
            int npcID = context.GetMyArgs(2, 0);
            //Avatar player = PlayerEx.Player;
            Avatar player;
            if (isPlayer == 0) {
                player = Tools.instance.getPlayer();
                return player.hasItem(itemID);
            } else
            {
                int num = NPCEx.NPCIDToNew(npcID);
                JSONObject npcStatus = jsonData.instance.AvatarBackpackJsonData[num.ToString()];
                //IsToolsMain.Log(npcStatus.ToString());
                if (npcStatus != null && npcStatus.HasField("Backpack"))
                {
                    JSONObject backpack = npcStatus["Backpack"];
                    return backpack.list.Find((JSONObject item) => (int)item["ItemID"].n == itemID) != null;
                }
                return false;
            }
        }
    }
}
