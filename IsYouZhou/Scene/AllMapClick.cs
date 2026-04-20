using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.JiuZhou.Scene
{
    public class AllMapClick : MonoBehaviour
    {
        public AllMapComponent comp;
        public Transform image;
        private void Start()
        {
            if (this.comp == null)
            {
                //如果comp为空，从当前游戏对象获取AllMapComponent
                this.comp = base.gameObject.GetComponent<AllMapComponent>();
            }
            if (this.image == null)
            {
                //如果image为空，查找名为"Level1Move"的子对象
                this.image = base.transform.Find("Level1Move");
            }
        }
        protected virtual void OnMouseDown()
        {
            //鼠标按下时的回调,将当前对象的缩放设置为0.42（缩小效果）
            base.transform.localScale = new Vector3(0.92f, 0.92f, 0.92f);
        }
        protected virtual void OnMouseEnter()
        {
            //鼠标进入时的回调,如果有image对象，将其y位置增加0.2（上浮效果）
            if (this.image != null)
            {
                this.image.localPosition = new Vector3(this.image.localPosition.x, this.image.localPosition.y + 0.1f, this.image.localPosition.z);
            }
            else
            {
                //如果没有image对象，移动当前对象本身
                base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 0.1f, base.transform.localPosition.z);
            }
        }
        protected virtual void OnMouseUp()
        {
            //鼠标释放时的回调,将缩放恢复为0.5（原始大小）
            base.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (!this.comp.CanClick() && Tools.instance.canClick(false, true))
            {
                // 如果不允许点击但满足Tools的条件，触发comp.EventRandom()
                this.comp.EventRandom();
            }
        }
        protected virtual void OnMouseExit()
        {
            //鼠标离开时的回调,与OnMouseEnter()相反，将y位置减少0.2（恢复原位置）
            if (this.image != null)
            {
                this.image.localPosition = new Vector3(this.image.localPosition.x, this.image.localPosition.y - 0.1f, this.image.localPosition.z);
            }
            else
            {
                base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y - 0.1f, base.transform.localPosition.z);
            }
        }
    }
}
