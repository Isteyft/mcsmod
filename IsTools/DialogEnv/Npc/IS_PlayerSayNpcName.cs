using KBEngine;
using SkySwordKill.Next.DialogEvent;
using SkySwordKill.Next.DialogSystem;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.IsTools.Util;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Npc
{
    [DialogEnvQuery("IS_PlayerSayNpcName")]
    public class IS_PlayerSayNpcName : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            int npcId = NPCEx.NPCIDToNew(context.GetMyArgs(0, 0));

            Avatar player = PlayerEx.Player;
            int playerLv = jsonData.instance.AvatarJsonData[npcId.ToString()]["Level"].I;
            int playerSex = jsonData.instance.AvatarJsonData[npcId.ToString()]["SexType"].I;
            int playerAge = jsonData.instance.AvatarJsonData[npcId.ToString()]["age"].I;
            int playerMenPai = jsonData.instance.AvatarJsonData[npcId.ToString()]["MenPai"].I;
            int npcLv = player.level;
            int npcSex = player.Sex;
            int npcAge = (int)player.age;
            int npcMenPai = player.menPai;

            string npcName = jsonData.instance.AvatarJsonData[npcId.ToString()]["Name"].Str;
            int npcFav = NPCEx.GetFavor(npcId);
            // 是否为道侣
            if (player.DaoLvId.HasItem(npcId))
            {
                string resultName = npcName;
                string[] specialSuffixes = { "道人", "上人", "仙子", "真人", "大圣", "剑仙", "老祖", "老魔", "散人", "子", "道长", "尊者", "长老" };

                //string[] doubleSurnames = {
                //    "南宫", "欧阳", "夏候", "诸葛", "东方", "皇甫", "尉迟", "公羊",
                //    "澹台", "公冶", "宗政", "淳于", "单于", "太叔", "申屠", "仲孙",
                //    "轩辕", "令狐", "钟离", "长孙", "慕容", "鲜于", "闾丘", "司徒",
                //    "司空", "端木", "太吾", "昆仑", "太公", "古月", "宰父", "乐正",
                //    "有琴", "闻人", "纳兰", "拓跋", "独孤", "十方", "四海", "武罗",
                //    "两仪", "东皇", "公孙", "百里"
                //};
                bool isSpecialTitle = false;

                // 检查是否包含特殊后缀
                foreach (string suffix in specialSuffixes)
                {
                    if (npcName.EndsWith(suffix))
                    {
                        resultName = npcName.Substring(0, npcName.Length - suffix.Length);
                        isSpecialTitle = true;
                        break;
                    }
                }

                // 如果不是特殊后缀，走“名字+儿”的逻辑
                if (!isSpecialTitle)
                {
                    // 防止名字为空报错
                    if (!string.IsNullOrEmpty(npcName))
                    {
                        string lastChar = npcName.Substring(npcName.Length - 1);
                        resultName = lastChar + "儿";
                    }
                }

                return resultName;
            }
            else if (player.Brother.HasItem(npcId)) //是否为结义
            {
                if (playerSex == 1)
                {
                    return "兄弟";
                }
                else if(playerSex == 2)
                {
                    return "姑娘";
                }
            }
            else if (player.TeatherId.HasItem(npcId)) //npc是否为你师傅
            {
                return "徒儿";
            }
            else if (player.TuDiId.HasItem(npcId)) // npc是否为你徒弟
            {
                return "师傅";
            }

            // 好感过高，类似炎萧哥哥
            if (npcFav >= 140)
            {
                // 玩家年纪大
                if (playerAge > npcAge)
                {
                    if (playerSex == 1)
                    {
                        if (npcSex == 1)
                        {
                            return "兄弟";
                        }
                        else if (npcSex == 2)
                        {
                            return "哥哥";
                        }
                    }
                    else if (playerSex == 2)
                    {
                        if (npcSex == 1)
                        {
                            return "姑娘";
                        }
                        else if (npcSex == 2)
                        {
                            return "姐姐";
                        }
                    }
                }
                else
                {
                    if (playerSex == 1)
                    {
                        if (npcSex == 1)
                        {
                            return "兄弟";
                        }
                        else if (npcSex == 2)
                        {
                            return "弟弟";
                        }
                    }
                    else if (playerSex == 2)
                    {
                        if (npcSex == 1)
                        {
                            return "妹子";
                        }
                        else if (npcSex == 2)
                        {
                            return "妹妹";
                        }
                    }
                }
            } 

            if (playerMenPai == npcMenPai)
            {
                if (playerLv > npcLv)
                {
                    // 玩家化神，npc不化神，叫老祖
                    if (playerLv > 12 && npcLv < 13)
                    {
                        return "师祖";
                    }
                    // 玩家是长老，npc为金丹以下
                    if (playerLv > 6 && npcLv < 7)
                    {
                        return "师叔";
                    }

                    if (playerSex == 1)
                    {
                        return "师兄";
                    }
                    else if (playerSex == 2)
                    {
                        return "师姐";
                    }
                }
                else if (playerLv > npcLv)
                {
                    if (npcLv > 12 && playerLv < 13)
                    {
                        return "师侄";
                    }
                    if (npcLv > 6 && playerLv < 7)
                    {
                        return "师侄";
                    }
                    if (playerSex == 1)
                    {
                        return "师弟";
                    }
                    else if (playerSex == 2)
                    {
                        return "师妹";
                    }
                }
                else
                {
                    if (playerAge >= npcAge)
                    {
                        if (playerSex == 1)
                        {
                            return "师兄";
                        }
                        else if (playerSex == 2)
                        {
                            return "师姐";
                        }
                    } 
                    else
                    {
                        if (playerSex == 1)
                        {
                            return "师弟";
                        }
                        else if (playerSex == 2)
                        {
                            return "师妹";
                        }
                    }
                }
                
            }

            // 熟悉，直接称呼名字
            if (npcFav >= 40)
            {
                return npcName;
            }


            if (playerLv / 3 > npcLv /3)
            {
                return "前辈";
            } 
            else if (playerLv / 3 < npcLv / 3) 
            {
                return "小友";
            }
            return "道友";

        }
    }
}
