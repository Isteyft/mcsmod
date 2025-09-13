using MaiJiu.MCS.HH.Tool;
using MaiJiu.MCS.HH.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.Utils;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    public class NpcInfo
    {
        public bool Init = false;
        public int MenPai;
        public int Type;
        public int SexType;
        public int Level;
        public int DunSu;
        public string Name;
        public string DunShu;
        public NpcInfo(int npcID)
        {
            JSONObject jsonobject;
            bool flag = jsonData.instance.AvatarJsonData.TryGetValue(npcID.ToString(), out jsonobject);
            if (flag)
            {
                this.MenPai = jsonobject["MenPai"].I;
                this.Type = jsonobject["Type"].I;
                this.SexType = jsonobject["SexType"].I;
                this.Level = jsonobject["Level"].I;
                this.DunSu = jsonobject["dunSu"].I;
                this.Name = jsonData.instance.AvatarRandomJsonData[npcID.ToString()]["Name"].Str;
                this.SetDunShu();
                this.Init = true;
            }
        }

        // Token: 0x06000065 RID: 101 RVA: 0x00004E90 File Offset: 0x00003090
        public void SetDunShu()
        {
            bool flag = this.Level > 11;
            if (flag)
            {
                this.DunShu = (MyTools.GetRandomBool(50) ? "MapPlayerHuLu" : "MapPlayerFengHuoLun");
            }
            else
            {
                int menPai = this.MenPai;
                int num = menPai;
                if (num <= 390)
                {
                    if (num == 380)
                    {
                        this.DunShu = "MapPlayerLianHua";
                        return;
                    }
                    if (num == 390)
                    {
                        this.DunShu = "MapPlayerJinDiao";
                        return;
                    }
                }
                else
                {
                    if (num == 451)
                    {
                        this.DunShu = "MapPlayerTengYun";
                        return;
                    }
                    if (num == 836)
                    {
                        this.DunShu = (MyTools.GetRandomBool(50) ? "MapPlayerLanJian" : "MapPlayerHuoJian");
                        return;
                    }
                }
                this.DunShu = "MapPlayerYuJian";
            }
        }
    }
}
