using BepInEx;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using YSGame;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    public class AllMapComponent : MapComponent
    {
        private float MoveBaseSpeedMin = 1.5f;
        private float MoveBaseSpeed = 4f;
        public UnityEngine.GameObject enter;
        public bool NoEnter = false;
        public LudianJson Data;
        public override int getAvatarNowMapIndex()
        {
            return PlayerEx.Player.fubenContorl[Tools.getScreenName()].NowIndex;
        }
        public override void CloseLuDian()
        {
            UnityEngine.GameObject gameObject = this.enter;
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
        public override void showLuDian()
        {
            bool flag = this.enter != null;
            if (flag)
            {
                this.enter.SetActive(true);
            }
            else
            {
                bool flag2 = this.Data != null && this.NoEnter && !DialogAnalysis.IsRunningEvent;
                if (flag2)
                {
                    bool flag3 = !this.Data.Event.IsNullOrWhiteSpace();
                    if (flag3)
                    {
                        DialogAnalysis.StartDialogEvent(this.Data.Event, null);
                    }
                    else
                    {
                        bool flag4 = !this.Data.Lua.IsNullOrWhiteSpace();
                        if (flag4)
                        {
                            DialogAnalysis.StartTestDialogEvent(this.Data.Lua, null);
                        }
                    }
                }
            }
        }
        public override void setAvatarNowMapIndex()
        {
            Tools.instance.fubenLastIndex = this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex;
            this.ComAvatar.fubenContorl[Tools.getScreenName()].NowIndex = this.NodeIndex;
        }
        public new bool CanClick()
        {
            return AllMapManage.instance.isPlayMove || this.getAvatarNowMapIndex() == this.NodeIndex || DialogAnalysis.IsRunningEvent;
        }
        public override void AvatarMoveToThis()
        {
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            bool flag = playerController.ShowType != MapPlayerShowType.遁术;
            if (flag)
            {
                UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
            }
            else
            {
                KBEngine.Avatar player = PlayerEx.Player;
                BaseMapCompont baseMapCompont = AllMapManage.instance.mapIndex[this.NodeIndex];
                int avatarNowMapIndex = this.getAvatarNowMapIndex();
                BaseMapCompont Nowcomp = AllMapManage.instance.mapIndex[avatarNowMapIndex];
                AllMapManage.instance.isPlayMove = true;
                float num = Vector2.Distance(Nowcomp.transform.position, baseMapCompont.transform.position);
                float num2 = (player.dunSu > 200) ? ((this.MoveBaseSpeedMin + this.MoveBaseSpeed) * 2f) : (this.MoveBaseSpeedMin + this.MoveBaseSpeed * ((float)player.dunSu / 100f));
                num2 *= 2f;
                float num3 = num / num2;
                Queue<UnityAction> queue = new Queue<UnityAction>();
                UnityAction item = delegate ()
                {
                    Nowcomp.CloseLuDian();
                    playerController.SetSpeed(1);
                    iTween.MoveTo(playerController.gameObject, iTween.Hash(new object[]
                    {
                        "x",
                        this.transform.position.x,
                        "y",
                        this.transform.position.y - 0.2f,
                        "z",
                        playerController.transform.position.z,
                        "time",
                        num3,
                        "islocal",
                        false,
                        "EaseType",
                        "linear"
                    }));
                    WASDMove.waitTime = num3;
                    WASDMove.needWait = true;
                    this.Invoke("callContinue", num3);
                };
                queue.Enqueue(item);
                YSFuncList.Ints.AddFunc(queue);
                Queue<UnityAction> queue2 = new Queue<UnityAction>();
                UnityAction item2 = delegate ()
                {
                    this.setAvatarNowMapIndex();
                    playerController.SetSpeed(0);
                    AllMapManage.instance.isPlayMove = false;
                    this.showLuDian();
                    YSFuncList.Ints.Continue();
                };
                queue2.Enqueue(item2);
                YSFuncList.Ints.AddFunc(queue2);
            }
        }
        public override void EventRandom()
        {
            this.AvatarMoveToThis();
            this.BaseAddTime();
        }
    }
}
