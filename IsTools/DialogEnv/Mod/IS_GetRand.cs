using Fungus;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.NextCommandExtension.Fungus;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{
    [DialogEnvQuery("IS_GetRand")]
    public class IS_GetRand : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            var randMax = context.GetMyArgs(0, 1);
            var randMin = context.GetMyArgs(1, 0);
            return new System.Random().Next(randMin, randMax);
        }
    }
}