using KBEngine;
using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class AvatarUtils
    {
        // 获取当前avatar是否是玩家
        public static bool isPlayer(Avatar avatar)
        {
            if (avatar != Tools.instance.getPlayer())
            {
                return false;
            }
            return true;
        }
        // 获取玩家avatar
        public static Avatar GetAvatar(int isPlayer)
        {
            if (isPlayer == 0)
            {
                return Tools.instance.getPlayer();
            }
            return Tools.instance.getPlayer().OtherAvatar;
        }
        // 获取玩家avatar
        public static Avatar GetAvatar(bool isPlayer)
        {
            if (isPlayer)
            {
                return Tools.instance.getPlayer();
            }
            return Tools.instance.getPlayer().OtherAvatar;
        }
    }
}
