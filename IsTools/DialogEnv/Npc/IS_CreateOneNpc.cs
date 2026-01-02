using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Npc
{
    [DialogEnvQuery("IS_CreateOneNpc")]
    public class IS_CreateOneNpc : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            var type = context.GetMyArgs(0, 0);
            var liuPai = context.GetMyArgs(1, 0);
            var level = context.GetMyArgs(2, 0);
            var sex = context.GetMyArgs(3, 0);
            var zhengXie = context.GetMyArgs(4, 0);

            return NpcUtil.CreateNpc(type, liuPai, level, sex, zhengXie);
        }
    }
}
