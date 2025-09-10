using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Map
{
    [DialogEnvQuery("IS_GetRandomItemType")]
    public class IS_GetRandomItemType : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            int type = context.GetMyArgs(0, 0);
            int level = context.GetMyArgs(0, 0);
            return ItemUtil.GetRandomItemType(type, level);
        }
    }
}
