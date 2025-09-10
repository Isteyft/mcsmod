using SkySwordKill.Next.DialogSystem;
using System;
using Fungus;
using UnityEngine;
using SkySwordKill.NextMoreCommand.Utils;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Mod
{

    [DialogEnvQuery("IS_GetBindNpcId")]
    public class IS_GetBindNpcId : IDialogEnvQuery
    {

        [SerializeField]
        [VariableProperty(new Type[] { typeof(IntegerVariable) })]
        protected IntegerVariable VarNPCID;

        public object Execute(DialogEnvQueryContext context)
        {
            Flowchart flowchart = context.Env.flowchart;
            int value = VarNPCID.Value;
            return value;
        }
    }
}