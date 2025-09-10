using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace top.Isteyft.MCS.IsMoDaoKuoZhanMain.Util
{
    public class SpriteUtil
    {
        public static Sprite GetLunDaoSprite(int type, int Id)
        {
            Sprite result = null;
            string str = Id.ToString();
            switch (type)
            {
                case 1:
                    result = ResManager.inst.LoadSprite("newui/lundao/lunti/mcs_ld_xuanlunti_" + str);
                    break;
                case 2:
                    result = ResManager.inst.LoadSprite("newui/lundao/pai/MCS_LD_pai_" + str);
                    break;
                case 3:
                    result = ResManager.inst.LoadSprite("newui/lundao/lundaoqiu/MCS_LD_qiu_" + str);
                    break;
                case 4:
                    result = ResManager.inst.LoadSprite("newui/lundao/fightlunti/MCS_LD_lunti_" + str);
                    break;
                case 5:
                    result = ResManager.inst.LoadSprite("newui/lundao/donghuapai/MCS_LD_npcpai_" + str);
                    break;
            }
            return result;
        }
    }
}
