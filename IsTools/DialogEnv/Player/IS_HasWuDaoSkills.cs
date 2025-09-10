using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Player
{
    [DialogEnvQuery("IS_HasWuDaoSkills")]
    public class IS_HasWuDaoSkills : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            int id = DialogEnvQueryContextExtension.GetMyArgs<int>(context, 0, 0);
            return WudaoUtil.HasWuDaoSkill(id);
        }
    }
}
