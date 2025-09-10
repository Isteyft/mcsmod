using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Npc
{
    [DialogEnvQuery("IS_GetNpcWuDaoLevel")]
    public class IS_GetNpcWuDaoLevel : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            var npcid = context.GetMyArgs(0, 0);
            var wudaoId = context.GetMyArgs(1, 0);

            npcid = NPCEx.NPCIDToNew(npcid);
            if (NPCEx.IsDeath(npcid))
            {
                //IsToolsMain.Warning($"npc“—À¿Õˆ");
                return -1;
            }
            var wuDaoData = jsonData.instance.AvatarJsonData[npcid.ToString()]["wuDaoJson"];
            if (wuDaoData.HasField(wudaoId.ToString()))
            {
                var wudaoInfo = wuDaoData[wudaoId.ToString()];
                return wudaoInfo["level"].I;
            }
            return 0;
        }
    }
}