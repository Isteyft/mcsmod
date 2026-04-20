using KBEngine;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System.Collections.Generic;
using System.Linq;

namespace top.Isteyft.MCS.JiuZhou.DialogEnv
{
    [DialogEnvQuery("YZ_GetNingZhouLiuPai")]
    public class YZ_GetNingZhouLiuPai : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            int[] a = {
                1, 2, 3, 4, 11, 12, 13, 14, 15, 21, 22, 23, 31, 32, 33, 34, 35,
                41, 42, 43, 44, 61, 62, 71, 72, 81, 82, 91, 92,
                101, 102, 103, 104, 105, 106, 107, 108,
                111, 112,
                121, 122, 123
            };

            return a[new System.Random().Next(0, a.Length)];
        }
    }
}