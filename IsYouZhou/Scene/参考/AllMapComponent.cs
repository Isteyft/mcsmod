using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using GetWay;
using MaiJiu.MCS.HH.Data;
using MaiJiu.MCS.HH.Tool;
using SkySwordKill.Next.DialogSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MaiJiu.MCS.HH.Scene
{
    // AllMapComponent 继承自 MapComponent，代表地图上的一个节点（如场景、传送点等）
    public class AllMapComponent : MapComponent
    {
        // 初始化方法
        protected override void Start()
        {
            base.Start();
            JSONObject jsonObject;
            // 尝试从全局场景配置中获取当前节点（NodeIndex）的配置数据
            bool flag = AllMapBase.inst.sceneJson != null && AllMapBase.inst.sceneJson.TryGetValue(this.NodeIndex.ToString(), out jsonObject);
            if (flag)
            {
                JSONObject jsonobject;
                // 解析“抵达触发”配置，即到达该节点时触发的指令
                bool flag2 = jsonObject.TryGetValue("抵达触发", out jsonobject);
                if (flag2)
                {
                    this.reachEvent = new MCommand(jsonobject.Str);
                }
                JSONObject jsonobject2;
                // 解析“进入按钮触发”配置，即点击按钮进入该节点时触发的指令
                bool flag3 = jsonObject.TryGetValue("进入按钮触发", out jsonobject2);
                if (flag3)
                {
                    this.enterEvent = new MCommand(jsonobject2.Str);
                }
            }
        }

        // 角色移动到该节点（重写基类方法）
        public override void AvatarMoveToThis()
        {
            base.AvatarMoveToThis();
        }

        // 获取角色当前所在的地图索引
        public override int getAvatarNowMapIndex()
        {
            return this.ComAvatar.NowMapIndex;
        }

        // 设置角色当前所在的地图索引，并更新存档数据
        public override void setAvatarNowMapIndex()
        {
            // 保存上一个地图索引到临时变量
            Tools.instance.fubenLastIndex = this.ComAvatar.NowMapIndex;
            // 更新角色当前地图索引为当前节点索引
            this.ComAvatar.NowMapIndex = this.NodeIndex;
            // 更新存档中的地图索引记录
            MaiSaveData.Inst.allMapIndex[SceneManager.GetActiveScene().name] = this.NodeIndex;
        }

        // 移动角色逻辑
        public override void movaAvatar()
        {
            // 如果角色当前不在该节点
            bool flag = this.getAvatarNowMapIndex() != this.NodeIndex;
            if (flag)
            {
                // 检查角色是否处于“遁术”状态（可能是快速移动或穿墙模式）
                bool flag2 = AllMapManage.instance.MapPlayerController.ShowType == MapPlayerShowType.遁术;
                if (flag2)
                {
                    // 如果是遁术，直接移动，不播放走路动画
                    base.NewMovaAvatar();
                }
                else
                {
                    // 否则，协程播放移动过程
                    base.StartCoroutine(this.Move());
                }
            }
        }

        // 随机事件逻辑（看起来和移动逻辑类似，可能是点击触发随机遭遇）
        public override void EventRandom()
        {
            bool flag = this.getAvatarNowMapIndex() != this.NodeIndex;
            if (flag)
            {
                bool flag2 = AllMapManage.instance.MapPlayerController.ShowType == MapPlayerShowType.遁术;
                if (flag2)
                {
                    this.NewEventRandom();
                }
                else
                {
                    base.StartCoroutine(this.Move());
                }
            }
        }

        // 显示路点/触发抵达事件
        public override void showLuDian()
        {
            DialogEnvironment env = new DialogEnvironment();
            // 检查是否有抵达事件配置，且条件满足
            bool flag = this.reachEvent != null && !this.reachEvent.param.IsNullOrWhiteSpace() && DialogAnalysis.CheckCondition(this.reachEvent.condition, env);
            if (flag)
            {
                // 触发对话或事件
                DialogAnalysis.StartTestDialogEvent(this.reachEvent.param, null);
            }
        }

        // 点击“进入”按钮触发的事件
        public virtual void enterBtn()
        {
            DialogEnvironment env = new DialogEnvironment();
            // 检查是否有进入按钮事件配置，且条件满足
            bool flag = this.enterEvent != null && !this.enterEvent.param.IsNullOrWhiteSpace() && DialogAnalysis.CheckCondition(this.enterEvent.condition, env);
            if (flag)
            {
                DialogAnalysis.StartTestDialogEvent(this.enterEvent.param, null);
            }
        }

        // 新的随机事件处理
        public new virtual void NewEventRandom()
        {
            // 检查是否可以点击
            bool flag = !base.CanClick();
            if (!flag)
            {
                this.fuBenSetClick(); // 设置副本点击状态
                base.NewMovaAvatar(); // 执行移动
            }
        }

        // 自动寻路协程
        public virtual IEnumerator Move()
        {
            List<int> list = new List<int>();
            int NowMapIndex = this.getAvatarNowMapIndex();
            
            // 计算路径：如果当前节点和目标节点相邻，直接添加；否则计算最佳路径
            bool flag = MapGetWay.Inst.IsNearly(NowMapIndex, this.NodeIndex);
            if (flag)
            {
                list.Add(this.NodeIndex);
            }
            else
            {
                list = MapGetWay.Inst.GetBestList(NowMapIndex, this.NodeIndex);
            }
            
            bool flag2 = list != null;
            if (flag2)
            {
                int index = 0;
                MapGetWay.Inst.IsStop = false;
                MapGetWay.Inst.CurTalk = 0;
                
                // 如果路径长度大于1，显示移动提示
                bool flag3 = list.Count > 1;
                if (flag3)
                {
                    MapMoveTips.Show();
                }
                else
                {
                    // 如果路径为空，提示无法前往
                    bool flag4 = list.Count == 0;
                    if (flag4)
                    {
                        UIPopTip.Inst.Pop("无法前往！", PopTipIconType.叹号);
                    }
                }
                
                // 遍历路径节点进行移动
                while (index < list.Count)
                {
                    int num = list[index];
                    bool flag5 = AllMapManage.instance != null && AllMapManage.instance.mapIndex.ContainsKey(num);
                    if (flag5)
                    {
                        // 根据节点类型（静态或动态）执行不同的移动逻辑
                        bool isStatic = this.IsStatic;
                        if (isStatic)
                        {
                            (AllMapManage.instance.mapIndex[num] as AllMapComponent).NewMovaAvatar();
                        }
                        else
                        {
                            (AllMapManage.instance.mapIndex[num] as AllMapComponent).NewEventRandom();
                        }
                    }
                    
                    // 等待移动动画播放完毕
                    while (AllMapManage.instance.isPlayMove)
                    {
                        yield return new WaitForSeconds(0.2f);
                    }
                    yield return new WaitForSeconds(0.3f);
                    
                    // 检查是否被中断（如玩家点击停止或不可点击）
                    bool flag6 = !Tools.instance.canClick(false, true) || MapGetWay.Inst.IsStop;
                    if (flag6)
                    {
                        break;
                    }
                    int num2 = index;
                    index = num2 + 1;
                }
            }
            // 停止自动移动
            MapGetWay.Inst.StopAuToMove();
            yield break;
        }

        // 字段定义
        private MCommand reachEvent; // 抵达时触发的指令
        private MCommand enterEvent; // 进入按钮触发的指令
    }
}