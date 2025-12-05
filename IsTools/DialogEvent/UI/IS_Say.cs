using Fungus;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Patch;
using top.Isteyft.MCS.IsTools.Util;
using UnityEngine;
using Fungus;
using UnityEngine.UI;

namespace top.Isteyft.MCS.IsTools.DialogEvent.UI
{

    [DialogEvent("IS_Say")]
    public class IS_Say : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            string str = command.GetStr(0);
            string str2 = command.GetStr(1);
            string str3 = command.GetStr(2);
            string str4 = command.GetStr(3);
            if (!command.BindEventData.Character.TryGetValue(str, out var value) && !DialogAnalysis.TmpCharacter.TryGetValue(str, out value))
            {
                value = 0;
            }

            SetCharacter(value, str2, str3);
            string text = DialogAnalysis.DealSayText(str4, value);
            SayDialog sayDialog = SayDialog.GetSayDialog();
            if (sayDialog == null)
            {
                Debug.LogError("WSay: 未找到SayDialog");
                callback?.Invoke();
            }
            else
            {
                sayDialog.Say(text, clearPrevious: true, waitForInput: true, fadeWhenDone: true, stopVoiceover: true, waitForVO: false, null, delegate
                {
                    callback?.Invoke();
                });
            }
        }

        public static void SetCharacter(int getNum, string chengHao, string name)
        {
            SayDialog sayDialog = SayDialog.GetSayDialog();
            if (sayDialog == null)
            {
                Debug.LogError("SetCharacter: SayDialog instance not found!");
                return;
            }

            ExtendedSayDialog ExtendedSayDialog = new ExtendedSayDialog(sayDialog);
            int num = getNum;
            if (NpcJieSuanManager.inst.ImportantNpcBangDingDictionary.ContainsKey(num))
            {
                num = NpcJieSuanManager.inst.ImportantNpcBangDingDictionary[num];
            }

            ExtendedSayDialog.SetCharacter(null, num);
            if (ExtendedSayDialog.NameText != null && !string.IsNullOrEmpty(name))
            {
                ExtendedSayDialog.NameText = name;
            }

            if (!string.IsNullOrEmpty(chengHao))
            {
                ExtendedSayDialog.SetChengHao(chengHao);
            }

            ExtendedSayDialog.SetCharacterImage(null, num);
        }
    }
}
