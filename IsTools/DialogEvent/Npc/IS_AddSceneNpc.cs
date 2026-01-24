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
            var scene = SceneManager.GetActiveScene().name;
            if (scene.StartsWith("S")) {
                NPCEx.WarpToScene(id, scene);
            }
            else
            {
                NPCEx.WarpToMap(id);
            }
            callback?.Invoke();
        }
    }
}
