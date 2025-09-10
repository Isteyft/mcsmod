using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Map
{
    [DialogEnvQuery("IS_GetRandomItemTypeID")]
    public class IS_GetRandomItemTypeID : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            int type = context.GetMyArgs(0, 0);
            int level = context.GetMyArgs(0, 0);
            return ItemUtil.GetRandomItemTypeID(type, level);
        }
    }
}
