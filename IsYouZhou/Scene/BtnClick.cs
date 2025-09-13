using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    public class BtnClick : MonoBehaviour
    {
        public UnityEvent mouseUpEvent = new UnityEvent();
        protected virtual void OnMouseDown()
        {
            base.transform.localScale = new Vector3(0.84f, 0.84f, 0.84f);
        }
        protected virtual void OnMouseEnter()
        {
            base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 0.05f, base.transform.localPosition.z);
        }
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
        protected virtual void OnMouseExit()
        {
            base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y - 0.05f, base.transform.localPosition.z);
        }
    }
}
