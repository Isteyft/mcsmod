# 天赋系统Patch介绍

## 功能概述

天赋系统Patch是针对游戏中天赋选择界面的扩展和修复补丁，主要实现了出生地图选择功能以及修复了天赋选择过程中的多个关键问题，提升了游戏体验和稳定性。

## 主要特性

### 1. 出生地图选择功能
- **新增第9页天赋界面**：添加了专门用于选择出生地图的页面（BIRTH_MAP_PAGE = 9）
- **出生地图影响**：不同的出生地图会影响初始资源与遭遇
- **页面切换逻辑**：在第8页天赋选择完成后自动跳转至出生地图选择页面
- **选择验证**：确保玩家必须选择一个出生地图才能继续
- **动态描述**：显示所选出生地图的详细信息和影响

### 2. 问题修复
- **重复键错误修复**：解决了天赋取消再次点击时出现的ArgumentException错误
- **末尾页面文本修复**：恢复了末尾页面介绍文本的显示
- **页面返回残留问题修复**：解决了从第10页返回第9页时的文字残留和按钮状态错误
- **第9页文字更新修复**：确保在第9页（出生地图页面）更改天赋选择时，描述文本能够实时更新

## 技术实现

### 文件结构
```
IsYouZhou/Patch/TianfuPatch.cs
```

### 核心组件

#### 1. 字段访问器
- `BIRTH_MAP_PAGE`：定义出生地图页面常量（值为9）
- `BirthMapCells`：存储出生地图天赋单元格的列表
- `finallyPageRef`：访问最终页面游戏对象
- `nextBtnRef`：访问下一步按钮
- `titleRef`：访问标题文本
- `descRef`：访问描述文本
- `shenYuNumRef`：访问剩余点数显示
- `setLingGenRef`：访问灵根设置组件
- `finallyDescRef`：访问最终描述文本

#### 2. 主要补丁方法

##### Init补丁 (Postfix_Init)
- 初始化出生地图页面
- 将出生地图天赋单元格添加到页面列表中
- 设置单元格的页面属性和初始状态

##### NextPage补丁 (Prefix_NextPage)
- 处理页面切换逻辑
- 验证当前页面是否有天赋被选择
- 实现从第8页到出生地图页面的跳转
- 实现从出生地图页面到最终页面的跳转
- 调用自定义的ShowFinallyPage方法显示最终页面内容

##### LastPage补丁 (Prefix_LastPage)
- 处理页面返回逻辑
- 当从最终页面返回时，重置所有相关UI元素
- 清除最终页面的文本残留
- 恢复第9页的标题和描述文本
- 修复按钮状态错误问题

##### UpdateDesc补丁 (Prefix_UpdateDesc)
- 自定义出生地图页面的描述文本
- 显示已选出生地图的详细信息
- 在第9页时完全接管描述文本更新逻辑

##### 重复键错误修复补丁 (Postfix_Init_FixDuplicateKey)
- 修复天赋取消再次点击时的重复键错误
- 为出生地图天赋的toggle组件重新绑定修复后的监听器
- 添加天赋选择和取消时的描述文本更新调用

##### ShowFinallyPage方法
- 实现最终页面的显示逻辑
- 显示已选天赋的详细描述
- 显示最终的故事文本
- 设置页面元素的可见性和状态

##### RegisterBirthMapCell方法
- 提供外部注册出生地图天赋单元格的接口
- 用于将出生地图单元格添加到系统中

## 使用说明

### 天赋选择流程
1. **正常选择前8页的天赋**：按照原有的天赋选择流程进行
2. **出生地图选择**：
   - 完成第8页选择后，系统会自动跳转至出生地图选择页面（第9页）
   - 选择一个出生地图，页面会实时显示所选地图的详细信息
   - 可以随时更改选择，页面描述会实时更新
3. **最终确认**：
   - 选择出生地图后，点击下一步进入最终页面（第10页）
   - 最终页面显示所有已选天赋的详细描述和故事文本
4. **页面返回**：
   - 在最终页面点击上一步，可以返回出生地图选择页面
   - 返回时会正确重置所有UI元素，不会有文本残留

### 操作注意事项
- 每一页都必须至少选择一个天赋才能继续
- 出生地图页面是新增的强制选择页面
- 天赋点不能为负数
- 在出生地图页面更改选择时，描述文本会实时更新

## 修复的问题详情

### 1. 重复键错误
**问题现象**：
```
ArgumentException: An item with the same key has already been added. Key: 10006
```
**原因**：当玩家取消天赋选择后再次点击时，代码尝试将相同的键再次添加到`hasSelectList`字典中
**修复方法**：
- 在添加天赋到`hasSelectList`之前检查键是否已存在
- 重新绑定了出生地图天赋的toggle监听器，确保正确处理选择和取消操作

### 2. 末尾页面文本丢失
**问题现象**：最终页面只显示标题，没有显示已选天赋的详细描述和故事文本
**原因**：原始代码中的ShowFinallyPage方法可能没有被正确调用或实现
**修复方法**：
- 实现了完整的`ShowFinallyPage`方法逻辑
- 添加了`finallyDescRef`字段访问器
- 复制了原始游戏的排序和显示逻辑

### 3. 页面返回残留问题
**问题现象**：从第10页返回第9页时，仍会显示最终页面的文字，按钮状态也不正确
**原因**：原始的LastPage方法在处理页面返回时没有正确重置UI元素
**修复方法**：
- 将LastPage补丁从Postfix改为Prefix，确保在原始方法执行前处理返回逻辑
- 隐藏最终页面并重置所有相关UI元素
- 清除最终页面的文本残留
- 恢复第9页的标题和描述文本

### 4. 第9页文字更新问题
**问题现象**：在第9页（出生地图页面）更改天赋选择时，描述文本不会更新
**原因**：天赋选择事件没有触发描述文本的更新
**修复方法**：
- 在Postfix_Init_FixDuplicateKey方法的toggle事件监听器中添加了`__instance.UpdateDesc()`调用
- 确保在天赋选择或取消选择时立即更新描述文本

## 技术细节

### 关键代码片段

#### 1. 出生地图页面描述更新
```csharp
[HarmonyPrefix]
[HarmonyPatch("UpdateDesc")]
public static bool Prefix_UpdateDesc(MainUISelectTianFu __instance)
{
    var title = titleRef(__instance);
    var desc = descRef(__instance);
    if (__instance.curPage == BIRTH_MAP_PAGE)
    {
        title.text = "出生之地";
        desc.text = "选择你的出生地图，这将影响初始资源与遭遇。";
        
        // 追加已选天赋的描述
        foreach (var key in __instance.hasSelectList.Keys)
        {
            if (__instance.hasSelectList[key].page == BIRTH_MAP_PAGE)
            {
                desc.text += "\n" + jsonData.instance.CreateAvatarJsonData[__instance.hasSelectList[key].id.ToString()]["Info"].Str;
            }
        }
        return false; // 阻止原始 UpdateDesc 执行
    }
    return true;
}
```

#### 2. 修复重复键错误和文字更新
```csharp
[HarmonyPostfix]
public static void Postfix_Init_FixDuplicateKey(MainUISelectTianFu __instance)
{
    // 为所有出生地图天赋的toggle添加键存在检查
    if (__instance.tianFuPageList.ContainsKey(BIRTH_MAP_PAGE))
    {
        foreach (var cell in __instance.tianFuPageList[BIRTH_MAP_PAGE])
        {
            // 移除现有的监听器
            cell.toggle.valueChange.RemoveAllListeners();
            // 添加修复后的监听器
            cell.toggle.valueChange.AddListener(delegate()
            {
                if (cell.toggle.isOn)
                {
                    // 检查是否已存在相同的键
                    if (!__instance.hasSelectList.ContainsKey(cell.id))
                    {
                        __instance.hasSelectList[cell.id] = cell;
                        __instance.AddTianFuDian(-cell.costNum);
                    }
                }
                else
                {
                    // 移除已选择的天赋
                    if (__instance.hasSelectList.ContainsKey(cell.id))
                    {
                        __instance.AddTianFuDian(cell.costNum);
                        __instance.hasSelectList.Remove(cell.id);
                    }
                }
                // 更新描述文本，确保第9页的文字在天赋更改时更新
                __instance.UpdateDesc();
            });
        }
    }
}
```

#### 3. 页面返回残留问题修复
```csharp
[HarmonyPrefix]
[HarmonyPatch("LastPage")]
public static bool Prefix_LastPage(MainUISelectTianFu __instance)
{
    var finallyPage = finallyPageRef(__instance);
    var nextBtn = nextBtnRef(__instance);
    var shenYuNum = shenYuNumRef(__instance);
    var finallyDesc = finallyDescRef(__instance);
    var title = titleRef(__instance);
    var desc = descRef(__instance);

    // 从最终页返回（当前页面是10）
    if (__instance.curPage == 10)
    {
        // 隐藏最终页面并重置所有UI元素
        finallyPage.SetActive(false);
        nextBtn.SetActive(true);
        shenYuNum.SetActive(true);
        
        // 清除最终页的文本
        if (finallyDesc != null)
        {
            finallyDesc.text = "";
        }
        
        // 重置标题和描述
        title.text = "出生之地";
        desc.text = "选择你的出生地图，这将影响初始资源与遭遇。";
        
        // 追加已选天赋的描述
        foreach (var key in __instance.hasSelectList.Keys)
        {
            if (__instance.hasSelectList[key].page == BIRTH_MAP_PAGE)
            {
                desc.text += "\n" + jsonData.instance.CreateAvatarJsonData[__instance.hasSelectList[key].id.ToString()]["Info"].Str;
            }
        }
        
        // 隐藏当前页面并显示出生地图页面
        __instance.HideCurPage();
        __instance.curPage = BIRTH_MAP_PAGE;
        __instance.ShowCurPageList();
        
        return false; // 阻止原始方法执行
    }
    
    // 如果当前页面是出生地图页面，执行原始逻辑
    if (__instance.curPage == BIRTH_MAP_PAGE)
    {
        return true; // 执行原始方法
    }
    
    // 其他页面执行原始逻辑
    return true;
}
```

#### 4. 最终页面文本修复
```csharp
private static void ShowFinallyPage(MainUISelectTianFu __instance)
{
    var title = titleRef(__instance);
    var desc = descRef(__instance);
    var finallyPage = finallyPageRef(__instance);
    var shenYuNum = shenYuNumRef(__instance);
    var finallyDesc = finallyDescRef(__instance);
    var nextBtn = nextBtnRef(__instance);

    title.text = "经历";
    desc.text = "";
    shenYuNum.SetActive(false);
    nextBtn.SetActive(false);
    finallyDesc.text = "\n";

    // 复制原始逻辑：排序并显示已选天赋描述
    List<int> list = new List<int>();
    foreach (int item in __instance.hasSelectList.Keys)
    {
        list.Add(item);
    }
    list.Sort((int x, int y) => x.CompareTo(y));
    foreach (int key in list)
    {
        if (__instance.hasSelectList[key].page != 1)
        {
            Text text = finallyDesc;
            text.text = text.text + jsonData.instance.CreateAvatarJsonData[__instance.hasSelectList[key].id.ToString()]["Info"].Str + "\n\n";
        }
    }
    
    // 添加最终的故事文本
    Text text2 = finallyDesc;
    text2.text += "十六岁那年，你意外捡到了一把满是锈迹的钝剑，无意间唤醒了其中沉睡的老者灵魂。在老者的指引下，长生之途的大门缓缓为你敞开——\n";

    finallyPage.SetActive(true);
}
```

## 版本信息

- **开发时间**：2026年1月
- **适用游戏版本**：当前游戏版本
- **作者**：系统自动生成

## 后续改进建议

1. **视觉效果增强**：可以考虑为出生地图添加更多的视觉效果和详细描述
2. **预览功能**：可以实现出生地图选择的预览功能，显示选择后的预期效果
3. **更多选择**：可以考虑添加更多的出生地图选项，增加游戏的多样性
4. **配置支持**：可以考虑添加配置文件支持，允许玩家自定义出生地图的影响
5. **兼容性优化**：可以进一步优化补丁的兼容性，确保与其他mod的良好协同

---

以上是天赋系统Patch的详细介绍，该补丁扩展了游戏的天赋选择功能，并修复了多个关键问题，提升了游戏体验和稳定性。