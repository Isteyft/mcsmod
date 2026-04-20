using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.JiuZhou.Scene
{
    public class GravityAnim : MonoBehaviour
    {
        public float floatSpeed = 1f;
        public float floatMagnitude = 0.1f;
        private Vector3 startPosition;
        private void Start()
        {
            this.startPosition = base.transform.position;
        }
        private void Update()
        {
            float y = Mathf.Sin(Time.time * this.floatSpeed) * this.floatMagnitude;
            base.transform.position = this.startPosition + new Vector3(0f, y, 0f);
        }
    }
}
