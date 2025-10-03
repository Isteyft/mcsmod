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
    /// <summary>
    /// NPC信息类，负责存储和管理NPC的各项属性数据
    /// </summary>
    public class NpcInfo
    {
        // 标记NPC是否初始化成功
        public bool Init = false;
        // NPC所属门派ID
        public int MenPai;
        // NPC类型
        public int Type;
        // NPC性别类型
        public int SexType;
        // NPC等级
        public int Level;
        // NPC遁速（移动速度）
        public int DunSu;
        // NPC名称
        public string Name;
        // NPC使用的遁术（移动方式）
        public string DunShu;

        /// <summary>
        /// 构造函数，根据NPC ID初始化NPC数据
        /// </summary>
        /// <param name="npcID">NPC的唯一ID</param>
        public NpcInfo(int npcID)
        {
            JSONObject jsonobject;
            // 尝试从AvatarJsonData中获取NPC数据
            bool flag = jsonData.instance.AvatarJsonData.TryGetValue(npcID.ToString(), out jsonobject);

            if (flag)
            {
                // 从JSON数据中解析各项属性
                this.MenPai = jsonobject["MenPai"].I;      // 门派
                this.Type = jsonobject["Type"].I;          // 类型
                this.SexType = jsonobject["SexType"].I;    // 性别
                this.Level = jsonobject["Level"].I;        // 等级
                this.DunSu = jsonobject["dunSu"].I;        // 遁速

                // 从随机数据中获取NPC名称
                this.Name = jsonData.instance.AvatarRandomJsonData[npcID.ToString()]["Name"].Str;

                // 设置NPC的遁术类型
                this.SetDunShu();

                // 标记初始化成功
                this.Init = true;
            }
        }

        /// <summary>
        /// 设置NPC的遁术类型（移动方式）
        /// </summary>
        public void SetDunShu()
        {
            // 判断NPC等级是否高于11级
            bool flag = this.Level > 11;
            if (flag)
            {
                // 高级NPC随机使用葫芦或风火轮
                this.DunShu = (MyTools.GetRandomBool(50) ? "MapPlayerHuLu" : "MapPlayerFengHuoLun");
            }
            else
            {
                // 根据门派设置不同的遁术
                int menPai = this.MenPai;
                int num = menPai;

                // 门派判断分支
                if (num <= 390)
                {
                    if (num == 380)  // 380门派使用莲花
                    {
                        this.DunShu = "MapPlayerLianHua";
                        return;
                    }
                    if (num == 390)  // 390门派使用金雕
                    {
                        this.DunShu = "MapPlayerJinDiao";
                        return;
                    }
                }
                else
                {
                    if (num == 451)  // 451门派使用腾云
                    {
                        this.DunShu = "MapPlayerTengYun";
                        return;
                    }
                    if (num == 836)  // 836门派随机使用蓝剑或火箭
                    {
                        this.DunShu = (MyTools.GetRandomBool(50) ? "MapPlayerLanJian" : "MapPlayerHuoJian");
                        return;
                    }
                }

                // 默认使用御剑
                this.DunShu = "MapPlayerYuJian";
            }
        }
    }
}