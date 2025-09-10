using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetStaticValue")]
    public class IS_GetStaticValue : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            IsToolsMain.Log("获取原版全局变量");
            return GlobalValue.Get(context.GetMyArgs(0, 0));
        }
    }
}