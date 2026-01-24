using KBEngine;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.StaticFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Player
{
    [DialogEvent("IS_SavePlayerNameFace")]
    public class IS_SavePlayerNameFace : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            string name = command.GetStr(0); 
            CustomStaticFaceInfo faceObj = DialogAnalysis.GetPlayerFaceData();
            string faceJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(faceObj);
            var player = env.player;
            ModConfigUtils.SetConfigProperty($"{name}Face", faceJsonString);
            ModConfigUtils.SetConfigProperty($"{name}Name", player.firstName.Replace(" ", "") + player.lastName.Replace(" ", ""));
            ModConfigUtils.SetConfigProperty($"{name}Sex", player.Sex);
            callback?.Invoke();
        }
    }
}