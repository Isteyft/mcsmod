using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.JiuZhou.UI
{
    public class YouZhouUIManager : MonoBehaviour
    {
        private void Awake()
        {
            this.prefabBank = new Dictionary<string, GameObject>();
            this.spriteBank = new Dictionary<string, Sprite>();
        }
        private void Start()
        {
        }
        public Dictionary<string, GameObject> prefabBank;

        public Dictionary<string, Sprite> spriteBank;
    }
}
