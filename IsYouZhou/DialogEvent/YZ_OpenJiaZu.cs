using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using top.Isteyft.MCS.JiuZhou.UI;

namespace top.Isteyft.MCS.JiuZhou.DialogEvent
{
    [DialogEvent("YZ_OpenJiaZu")]
    public class YZ_OpenJiaZu : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            try
            {
                JiaZuPanel.Show();
            }
            catch (System.Exception ex)
            {
                IsToolsMain.Warning($"YZ_OpenJiaZu error: {ex.Message}");
            }
            finally
            {
                callback?.Invoke();
            }
        }
    }
}
