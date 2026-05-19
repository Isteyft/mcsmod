using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.JiuZhou.UI;
using top.Isteyft.MCS.JiuZhou.Utils;
using UnityEngine;

namespace top.Isteyft.MCS.JiuZhou.DialogEvent
{
    [DialogEvent("YZ_OpenMap")]
    public class YZ_OpenMap : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            try
            {
                YZMapPanel.Show();
            }
            catch (System.Exception ex)
            {
                IsToolsMain.Warning($"YZ_OpenMap error: {ex.Message}");
            }
            finally
            {
                callback?.Invoke();
            }
        }
    }
}
