using MaiJiu.MCS.HH.Scene;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.Scene;
using top.Isteyft.MCS.YouZhou.Utils;
using UnityEngine.SceneManagement;

namespace top.Isteyft.MCS.YouZhou.DialogEvent
{
    [DialogEvent("YZ_MoveTo")]
    public class YZ_MoveTo : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int targetNodeIndex = command.GetInt(0);
            AllMapComponent.SetAvatarNowMapIndexStatic(targetNodeIndex);
            callback?.Invoke();
        }
    }
}
