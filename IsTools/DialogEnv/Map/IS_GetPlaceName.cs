using SkySwordKill.Next.DialogSystem;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Map
{
    [DialogEnvQuery("IS_GetPlaceName")]
    public class IS_GetPlaceName : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            return Util.WarpUtils.GetPlaceName();
        }
    }
}
