using Fungus;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.NextCommandExtension.Fungus;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetNpcNewId")]
    public class IS_GetNpcNewId : IDialogEnvQuery
    {
        public int npcid = 0;
        public object Execute(DialogEnvQueryContext context)
        {
            this.npcid =  NPCEx.NPCIDToNew(context.GetMyArgs(0, 0));
            return this.npcid;
        }
    }
}