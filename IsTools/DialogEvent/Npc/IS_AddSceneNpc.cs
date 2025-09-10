using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using System;
using UnityEngine.SceneManagement;

namespace top.Isteyft.MCS.IsTools.DialogEvent.Npc
{
    [DialogEvent("IS_AddSceneNpc")]
    public class IS_AddSceneNpc : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            int id = command.GetInt(0);
            NPCEx.WarpToScene(id, SceneManager.GetActiveScene().name);
            callback?.Invoke();
        }
    }
}
