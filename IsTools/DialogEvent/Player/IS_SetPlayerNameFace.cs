using KBEngine;
using Newtonsoft.Json;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.Next.StaticFace;
using SkySwordKill.NextMoreCommand.NextCommandExtension.Npc;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Player
{
    [DialogEvent("IS_SetPlayerNameFace")]
    public class IS_SetPlayerNameFace : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            string name = command.GetStr(0);
            int npcid = command.GetInt(1);
            string face = ModConfigUtils.GetConfigProperty($"{name}Face");
            string names = ModConfigUtils.GetConfigProperty($"{name}Name");
            CustomStaticFaceInfo loadedFaceInfo = JsonConvert.DeserializeObject<CustomStaticFaceInfo>(face);
            DialogAnalysis.SetNpcFaceData(npcid, loadedFaceInfo);
            NpcUtils.SetNpcFullName(npcid, "", names);
            callback?.Invoke();
        }
    }
}