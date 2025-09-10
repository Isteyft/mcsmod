using SkySwordKill.Next.DialogSystem;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Map
{
    [DialogEnvQuery("IS_GetCurAllMapIndex")]
    public class IS_GetCurAllMapIndex : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {

            return PlayerEx.Player.NowMapIndex;
        }
    }
}
