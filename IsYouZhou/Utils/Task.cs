using System;
using Spine.Unity;
using UnityEngine;

namespace top.Isteyft.MCS.JiuZhou.Utils
{
    // Token: 0x0200000F RID: 15
    public class Task : MonoBehaviour
    {
        // Token: 0x06000042 RID: 66 RVA: 0x00003CE5 File Offset: 0x00001EE5
        public void SetSpine(SkeletonAnimation skeletonAnimation, float time)
        {
            this.skeletonAnimation = skeletonAnimation;
            base.Invoke("SetSpine", time);
        }

        // Token: 0x06000043 RID: 67 RVA: 0x00003CFC File Offset: 0x00001EFC
        private void SetSpine()
        {
            this.skeletonAnimation.Initialize(true, false);
            this.skeletonAnimation.gameObject.SetActive(true);
        }

        // Token: 0x06000044 RID: 68 RVA: 0x00003D1F File Offset: 0x00001F1F
        private void StartInvoke()
        {
            Action action = this.action;
            if (action != null)
            {
                action();
            }
            UnityEngine.Object.Destroy(this);
        }

        // Token: 0x06000045 RID: 69 RVA: 0x00003D3B File Offset: 0x00001F3B
        public void TaskInvoke(Action action, float time)
        {
            this.action = action;
            base.Invoke("StartInvoke", time);
        }

        // Token: 0x04000018 RID: 24
        public Action action;

        // Token: 0x04000019 RID: 25
        public SkeletonAnimation skeletonAnimation;
    }
}
