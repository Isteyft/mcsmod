using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;

namespace top.Isteyft.MCS.TianNan.Scene
{
    public class BtnClick : MonoBehaviour
    {
        // 点击后的事件
        public UnityEvent mouseUpEvent = new UnityEvent();

        /**
         * 鼠标点击时候进行缩小
         */
        protected virtual void OnMouseDown()
        {
            base.transform.localScale = new Vector3(0.84f, 0.84f, 0.84f);
        }
        /**
         * 鼠标点击时候进行进行移动
         */
        protected virtual void OnMouseEnter()
        {
            base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 0.05f, base.transform.localPosition.z);
        }

        /**
         * 鼠标回弹时候恢复原样
         */
        protected virtual void OnMouseUp()
        {
            base.transform.localScale = new Vector3(1f, 1f, 1f);
            if (!DialogAnalysis.IsRunningEvent)
            {
                UnityEvent unityEvent = this.mouseUpEvent;
                if (unityEvent != null)
                {
                    unityEvent.Invoke();
                }
            }
        }

        /**
         * 鼠标滑出时候恢复原样
         */
        protected virtual void OnMouseExit()
        {
            base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y - 0.05f, base.transform.localPosition.z);
        }
    }
}
