using Fungus;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.NextCommandExtension.Fungus;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetNpcXingGe")]
    public class IS_GetNpcXingGe : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            int npcid =  NPCEx.NPCIDToNew(context.GetMyArgs(0, 0));
            return jsonData.instance.AvatarJsonData[npcid.ToString()]["XingGe"].I;
        }
    }
}