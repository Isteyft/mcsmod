using Fungus;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.NextCommandExtension.Fungus;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetNpcTag")]
    public class IS_GetNpcTag : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            int npcid =  NPCEx.NPCIDToNew(context.GetMyArgs(0, 0));
            return jsonData.instance.AvatarJsonData[npcid.ToString()]["NPCTag"].I;
        }
    }
}