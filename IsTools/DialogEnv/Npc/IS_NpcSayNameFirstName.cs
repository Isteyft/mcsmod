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
    [DialogEnvQuery("IS_NpcSayNameFirstName")]
    public class IS_NpcSayNameFirstName : IDialogEnvQuery
    {

        public object Execute(DialogEnvQueryContext context)
        {
            int npcId = NPCEx.NPCIDToNew(context.GetMyArgs(0, 0));

            Avatar player = PlayerEx.Player;
            int playerLv = player.level;
            int playerSex = player.Sex;
            int playerAge = (int)player.age;
            int playerMenPai = player.menPai;
            string playerFirstName = player.firstName;
            int npcLv = jsonData.instance.AvatarJsonData[npcId.ToString()]["Level"].I;
            int npcSex = jsonData.instance.AvatarJsonData[npcId.ToString()]["SexType"].I;
            int npcAge = jsonData.instance.AvatarJsonData[npcId.ToString()]["age"].I;
            int npcMenPai = jsonData.instance.AvatarJsonData[npcId.ToString()]["MenPai"].I;

            int npcFav = NPCEx.GetFavor(npcId);
            // 是否为道侣
            if (player.DaoLvId.HasItem(npcId))
            {
                return PlayerEx.GetDaoLvNickName(npcId);
            }
            else if (player.Brother.HasItem(npcId)) //是否为结义
            {
                if (playerSex == 1)
                {
                    return playerFirstName+"兄弟";
                }
                else if(playerSex == 2)
                {
                    return playerFirstName+"姑娘";
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
                            return playerFirstName+"兄弟";
                        }
                        else if (npcSex == 2)
                        {
                            return playerFirstName + "哥哥";
                        }
                    }
                    else if (playerSex == 2)
                    {
                        if (npcSex == 1)
                        {
                            return playerFirstName + "姑娘";
                        }
                        else if (npcSex == 2)
                        {
                            return playerFirstName + "姐姐";
                        }
                    }
                }
                else
                {
                    if (playerSex == 1)
                    {
                        if (npcSex == 1)
                        {
                            return playerFirstName + "兄弟";
                        }
                        else if (npcSex == 2)
                        {
                            return playerFirstName + "弟弟";
                        }
                    }
                    else if (playerSex == 2)
                    {
                        if (npcSex == 1)
                        {
                            return playerFirstName + "妹子";
                        }
                        else if (npcSex == 2)
                        {
                            return playerFirstName + "妹妹";
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
                        return playerFirstName + "师祖";
                    }
                    // 玩家是长老，npc为金丹以下
                    if (playerLv > 6 && npcLv < 7)
                    {
                        return playerFirstName + "师叔";
                    }

                    if (playerSex == 1)
                    {
                        return playerFirstName + "师兄";
                    }
                    else if (playerSex == 2)
                    {
                        return playerFirstName + "师姐";
                    }
                }
                else if (playerLv > npcLv)
                {
                    if (npcLv > 12 && playerLv < 13)
                    {
                        return playerFirstName + "师侄";
                    }
                    if (npcLv > 6 && playerLv < 7)
                    {
                        return playerFirstName + "师侄";
                    }
                    if (playerSex == 1)
                    {
                        return playerFirstName + "师弟";
                    }
                    else if (playerSex == 2)
                    {
                        return playerFirstName + "师妹";
                    }
                }
                else
                {
                    if (playerAge >= npcAge)
                    {
                        if (playerSex == 1)
                        {
                            return playerFirstName + "师兄";
                        }
                        else if (playerSex == 2)
                        {
                            return playerFirstName + "师姐";
                        }
                    } 
                    else
                    {
                        if (playerSex == 1)
                        {
                            return playerFirstName + "师弟";
                        }
                        else if (playerSex == 2)
                        {
                            return playerFirstName + "师妹";
                        }
                    }
                }
                
            }

            // 熟悉，直接称呼名字
            if (npcFav >= 40)
            {
                return player.firstName + "" + player.lastName;
            }


            if (playerLv / 3 > npcLv /3)
            {
                return playerFirstName + "前辈";
            } 
            else if (playerLv / 3 < npcLv / 3) 
            {
                return playerFirstName + "小友";
            }
            return playerFirstName + "道友";

        }
    }
}
