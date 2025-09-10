using SkySwordKill.Next.DialogSystem;
using System.Collections.Generic;
using System.IO;

namespace top.Isteyft.MCS.IsTools.DialogEnv.MenPai
{
    [DialogEnvQuery("IS_GetMenPai")]
    public class IS_GetMenPai : IDialogEnvQuery
    {
        private static List<DirectoryInfo> _mods;

        public object Execute(DialogEnvQueryContext context)
        {

            return PlayerEx.Player.menPai;
        }
    }
}