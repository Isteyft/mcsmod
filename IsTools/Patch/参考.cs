// 使用Harmony对名为"onAttachRealizeSeid109"的方法进行前缀补丁
// 这个方法通常在某个Buff效果被触发时调用，用于处理技能相关的逻辑
[HarmonyPatch("onAttachRealizeSeid109")]
[HarmonyPrefix]
public static bool Prefix(Buff __instance, int seid, KBEngine.Avatar avatar)
{
    // 检查金丹管理器是否存在，且金丹数据列表不为空
    // 这表明只有在存在金丹系统数据时才执行特殊逻辑
    if (JieDanManager.instence != null && JieDanData.data.Count > 0)
    {
        // 从Buff实例中获取seid配置的value1字段，并转换为技能ID列表
        // 这个value1可能包含要添加的技能ID数组
        List<int> list = __instance.getSeidJson(seid)["value1"].ToList();
        
        // 遍历金丹数据列表，检查是否包含金丹技能
        using (List<JieDanData>.Enumerator enumerator = JieDanData.data.GetEnumerator())
        {
            // 移动到第一个元素
            if (enumerator.MoveNext())
            {
                JieDanData current = enumerator.Current;
                // 如果当前技能列表不包含金丹技能，则跳过自定义逻辑
                // 返回true表示继续执行原方法
                if (!list.Contains(current.JieDanSkill))
                {
                    return true;
                }
            }
        }

        // 禁用技能栏的网格布局组件
        // 这样可以手动控制技能按钮的位置，避免自动布局干扰
        UIFightPanel.Inst.FightSkills[0].transform.parent.GetComponent<GridLayoutGroup>().enabled = false;
        
        // 开始动态扩展技能栏UI
        int num = 1;
        // 循环直到技能栏UI数量满足需求
        // 需要的数量 = 12（基础技能槽）+ 金丹数据数量
        while (UIFightPanel.Inst.FightSkills.Count < JieDanData.data.Count + 12)
        {
            // 获取当前技能按钮作为模板
            UnityEngine.GameObject gameObject = UIFightPanel.Inst.FightSkills[num - 1].gameObject;
            Vector3 localPosition = gameObject.transform.localPosition;
            
            // 克隆一个新技能按钮
            UnityEngine.GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
            
            // 设置新按钮的父对象
            gameObject2.transform.SetParent(gameObject.transform.parent, worldPositionStays: false);
            
            // 设置新按钮的位置（Y轴偏移115像素）
            gameObject2.transform.localPosition = new Vector3(localPosition.x, localPosition.y + 115f, localPosition.z);
            
            // 获取新按钮的UIFightSkillItem组件
            UIFightSkillItem component = gameObject2.GetComponent<UIFightSkillItem>();
            
            // 设置快捷键（数字转字母，如1->Q, 2->W等）
            component.HotKey = NumberToHomeRowLetter(num);
            
            // 更新按钮上的快捷键文本显示
            gameObject2.transform.Find("Slot/LeftUpMask/HotKey").GetComponent<Text>().text = component.HotKey.ToString();
            
            num++;
            
            // 清空新按钮的状态
            component.Clear();
            
            // 将新按钮添加到技能栏列表中
            UIFightPanel.Inst.FightSkills.Add(component);
        }

        // 遍历技能ID列表，将技能添加到角色和UI中
        for (int i = 0; i < list.Count; i++)
        {
            // 前6个技能使用特殊方法添加（可能是在基础技能槽中）
            if (i < 6)
            {
                // 将技能添加到角色技能列表，并在UI的前6个槽位中寻找空位
                avatar.FightAddSkill(list[i], 0, 6);
                continue;
            }

            // 创建新的技能对象
            GUIPackage.Skill skill = new GUIPackage.Skill(list[i], 0, 10);
            
            // 将技能添加到角色的技能列表中
            avatar.skill.Add(skill);
            
            // 检查当前角色是否为玩家
            // 如果不是玩家，则阻止原方法执行
            if (!avatar.isPlayer())
            {
                return false;
            }

            // 将技能设置到对应的UI技能槽中
            // 从第7个槽位开始（i+6），对应扩展的金丹技能槽
            UIFightPanel.Inst.FightSkills[i + 6].SetSkill(skill);
        }

        // 返回false表示阻止原方法执行
        // 这意味着自定义逻辑完全替代了原方法
        return false;
    }

    // 如果金丹系统未激活，则继续执行原方法
    return true;
}



