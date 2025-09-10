using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetVariable")]
    public class IS_GetVariable : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            DialogEnvironment env = context.Env;
            var id = env.flowchart.GetVariable(context.GetMyArgs(0, "MonsterID"));
            return id;
        }
    }
}