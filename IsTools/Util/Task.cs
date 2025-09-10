using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class Task : MonoBehaviour
    {
        public Action action;

        public SkeletonAnimation skeletonAnimation;
        public void SetSpine(SkeletonAnimation skeletonAnimation, float time)
        {
            this.skeletonAnimation = skeletonAnimation;
            base.Invoke("SetSpine", time);
        }
        private void SetSpine()
        {
            this.skeletonAnimation.Initialize(true, false);
            this.skeletonAnimation.gameObject.SetActive(true);
        }
        private void StartInvoke()
        {
            Action action = this.action;
            if (action != null)
            {
                action();
            }
            UnityEngine.Object.Destroy(this);
        }
        public void TaskInvoke(Action action, float time)
        {
            this.action = action;
            base.Invoke("StartInvoke", time);
        }
    }
}
