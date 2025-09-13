using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    public class AllMapNpcController : MonoBehaviour
    {
        public NpcInfo npcdata;
        public int npcID;
        public SkeletonAnimation skeletonAnimation;
        public GameObject spine;
        public const float movespeed = 5.5f;
        public float waittime;
        private void Start()
        {
            this.npcID = int.Parse(base.name);
            this.npcdata = new NpcInfo(this.npcID);
        }
        public void SetNpcMove(int index)
        {
            BaseMapCompont baseMapCompont;
            bool flag = !AllMapManage.instance.mapIndex.TryGetValue(index, out baseMapCompont);
            if (flag)
            {
                UnityEngine.Object.Destroy(base.gameObject);
            }
            else
            {
                float num = Vector2.Distance(base.gameObject.transform.position, baseMapCompont.transform.position);
                float num2 = (this.npcdata.DunSu > 100) ? 5.5f : (5.5f * ((float)this.npcdata.DunSu / 100f));
                num2 *= 4f;
                this.waittime = num / num2;
                bool flag2 = this.waittime > 5f;
                if (flag2)
                {
                    this.waittime = 5f;
                }
                bool flag3 = baseMapCompont.transform.position.x < base.transform.position.x;
                if (flag3)
                {
                    this.spine.transform.localScale = new Vector3(-Mathf.Abs(this.spine.transform.localScale.x), this.spine.transform.localScale.y, this.spine.transform.localScale.z);
                }
                else
                {
                    this.spine.transform.localScale = new Vector3(Mathf.Abs(this.spine.transform.localScale.x), this.spine.transform.localScale.y, this.spine.transform.localScale.z);
                }
                this.skeletonAnimation.AnimationName = "Move";
                this.skeletonAnimation.Initialize(true, false);
                iTween.MoveTo(base.gameObject, iTween.Hash(new object[]
                {
                    "x",
                    baseMapCompont.transform.position.x,
                    "y",
                    baseMapCompont.transform.position.y - 0.2f,
                    "z",
                    base.transform.position.z,
                    "time",
                    this.waittime,
                    "islocal",
                    false,
                    "EaseType",
                    "linear"
                }));
                base.Invoke("SetIdle", this.waittime);
            }
        }
        public void SetIdle()
        {
            this.skeletonAnimation.AnimationName = "Idle";
            this.skeletonAnimation.Initialize(true, false);
        }
        public bool CheckDestroy()
        {
            if (!AllMapBase.NOWMAPNPCLIST.Contains(this.npcID))
            {
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
