using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.JiuZhou.Scene
{
    /// <summary>
    /// NPC地图控制器，负责管理大地图上NPC的显示、移动和行为
    /// </summary>
    public class AllMapNpcController : MonoBehaviour
    {
        // NPC数据信息
        public NpcInfo npcdata;
        // NPC的唯一ID
        public int npcID;
        // Spine骨骼动画组件
        public SkeletonAnimation skeletonAnimation;
        // Spine动画对象
        public GameObject spine;
        // NPC基础移动速度
        public const float movespeed = 5.5f;
        // NPC移动等待时间
        public float waittime;

        /// <summary>
        /// Unity Start生命周期方法，初始化NPC
        /// </summary>
        private void Start()
        {
            // 从游戏对象名称解析NPC ID
            this.npcID = int.Parse(base.name);
            // 根据ID初始化NPC数据
            this.npcdata = new NpcInfo(this.npcID);
        }
        /// <summary>
        /// 设置NPC移动到指定路点
        /// </summary>
        /// <param name="index">目标路点索引</param>
        public void SetNpcMove(int index)
        {
            BaseMapCompont baseMapCompont;
            // 尝试获取目标路点组件
            bool flag = !AllMapManage.instance.mapIndex.TryGetValue(index, out baseMapCompont);

            // 如果路点不存在，销毁NPC对象
            if (flag)
            {
                UnityEngine.Object.Destroy(base.gameObject);
            }
            else
            {
                // 计算当前位置到目标位置的距离
                float num = Vector2.Distance(base.gameObject.transform.position, baseMapCompont.transform.position);

                // 根据NPC的遁速属性计算实际移动速度
                float num2 = (this.npcdata.DunSu > 100) ? 5.5f : (5.5f * ((float)this.npcdata.DunSu / 100f));
                num2 *= 4f;

                // 计算移动所需时间
                this.waittime = num / num2;

                // 限制最大移动时间为5秒
                bool flag2 = this.waittime > 5f;
                if (flag2)
                {
                    this.waittime = 5f;
                }
                // 根据移动方向调整NPC朝向
                bool flag3 = baseMapCompont.transform.position.x < base.transform.position.x;
                if (flag3)
                {
                    // 向左移动时翻转精灵
                    this.spine.transform.localScale = new Vector3(-Mathf.Abs(this.spine.transform.localScale.x), this.spine.transform.localScale.y, this.spine.transform.localScale.z);
                }
                else
                {
                    // 向右移动时保持原朝向
                    this.spine.transform.localScale = new Vector3(Mathf.Abs(this.spine.transform.localScale.x), this.spine.transform.localScale.y, this.spine.transform.localScale.z);
                }
                // 设置移动动画
                this.skeletonAnimation.AnimationName = "Move";
                this.skeletonAnimation.Initialize(true, false);

                // 使用iTween实现平滑移动
                iTween.MoveTo(base.gameObject, iTween.Hash(new object[]
                {
                    "x",
                    baseMapCompont.transform.position.x,
                    "y",
                    baseMapCompont.transform.position.y - 0.2f, // 稍微向下偏移
                    "z",
                    base.transform.position.z,
                    "time",
                    this.waittime,
                    "islocal",
                    false,
                    "EaseType",
                    "linear"  // 线性移动
                }));
                // 移动完成后切换到待机状态
                base.Invoke("SetIdle", this.waittime);
            }
        }
        /// <summary>
        /// 设置NPC为待机状态
        /// </summary>
        public void SetIdle()
        {
            this.skeletonAnimation.AnimationName = "Idle";
            this.skeletonAnimation.Initialize(true, false);
        }
        /// <summary>
        /// 检查NPC是否需要销毁
        /// </summary>
        /// <returns>如果NPC已被销毁返回true，否则返回false</returns>
        public bool CheckDestroy()
        {
            // 检查当前NPC是否还在当前地图的NPC列表中
            if (!AllMapBase.NOWMAPNPCLIST.Contains(this.npcID))
            {
                // 从怪物列表中移除并销毁对象
                AllMapBase.Inst.monsterlist.Remove(this.npcID);
                UnityEngine.Object.Destroy(base.gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
