using SkySwordKill.Next.DialogSystem;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Map
{
    [DialogEnvQuery("IS_GetCurFubenIndex")]
    public class IS_GetCurFubenIndex : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            if (PlayerEx.Player.NowFuBen == "" && !RandomFuBen.IsInRandomFuBen)
                return 0;
            return PlayerEx.Player.fubenContorl[Tools.getScreenName()].NowIndex;
        }
    }
}
