using SkySwordKill.NextMoreCommand.Patchs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spine;

namespace top.Isteyft.MCS.IsMoXinXiangJuan.Patch
{
    public class NMCPatch
    {
        public static void SpineSpecialSet(SpineAvatarInfo spineAvatar)
        {
            //spineAvatar.Refresh();
            //LineXueMCSPrugin.LogFromFile("spine加载：{spineAvatar.FaceSpine}");
            if (spineAvatar.FaceSpine == "10305")
            {
                string skinname = spineAvatar.Skin;
                if (spineAvatar.IsFightScene)
                {
                    spineAvatar.SetSkin("default", "default");
                    spineAvatar.OnStartAnimator = new Action<TrackEntry, SpineAvatarInfo>(
                        (entry, info) =>
                        {
                            string name = entry.Animation.Name;
                            info.skeletonAnimation.AnimationState.ClearTracks();
                            info.skeletonAnimation.skeleton.SetToSetupPose();
                            info.skeletonAnimation.AnimationState.SetAnimation(0, name, name == "Idle_0");
                        });
                }
                else
                {
                    spineAvatar.SetSkin(skinname.Replace("战斗", ""), "default");
                }
                //spineAvatar.Refresh();
            }
        }
    }
}
