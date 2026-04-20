using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.TianNan.Scene
{
    public class AllMapClick: MonoBehaviour
    {
        // 路点对象
        public AllMapComponent comp;
        // 图标图片对象
        public Transform image;

        /**
         * 开始时，对于路点对象和图标图片进行获取
         */
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
        /**
         * 鼠标点击时候进行缩小
         */
        protected virtual void OnMouseDown()
        {
            base.transform.localScale = new Vector3(0.92f, 0.92f, 0.92f);
        }
        /**
         * 鼠标点击时候进行进行移动
         */
        protected virtual void OnMouseEnter()
        {
            if (this.image != null)
            {
                this.image.localPosition = new Vector3(this.image.localPosition.x, this.image.localPosition.y + 0.1f, this.image.localPosition.z);
            }
            else
            {
                base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 0.1f, base.transform.localPosition.z);
            }
        }
        /**
         * 鼠标回弹时候恢复原样
         */
        protected virtual void OnMouseUp()
        {
            base.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (!this.comp.CanClick() && Tools.instance.canClick(false, true))
            {
                // 如果不可点击时，触发comp.EventRandom()
                this.comp.EventRandom();
            }
        }
        /**
         * 鼠标滑出时候恢复原样
         */
        protected virtual void OnMouseExit()
        {
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
