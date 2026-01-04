# YouZhou项目Scene模块说明文档

## 模块概述

Scene模块是YouZhou模组的核心组成部分，负责实现大地图系统的功能，包括地图展示、NPC控制、玩家移动、交互事件等。该模块通过多个组件协同工作，构建了一个完整的可视化地图交互系统。

## 核心组件说明

### 1. 地图基础管理 (AllMapBase.cs)
这是整个地图系统的核心管理类，负责：
- 初始化地图数据和组件
- 加载场景JSON数据并反序列化为可操作的对象
- 管理地图上的所有路点组件
- 设置地图背景音乐
- 控制玩家初始位置

### 2. 地图组件 (AllMapComponent.cs)
每个地图路点都包含此组件，主要功能包括：
- 管理路点的基本属性和状态
- 处理玩家移动到该路点的逻辑
- 支持普通移动和遁术移动两种方式
- 触发路点相关的事件和Lua脚本

### 3. 地图数据结构
#### AllMapJson.cs
定义整个地图的数据结构：
- Name: 地图名称
- Music: 背景音乐
- LuDian: 包含所有路点数据的字典

#### LudianJson.cs
定义单个路点的数据结构：
- Name: 路点名称
- Event: 关联的事件ID
- Lua: 关联的Lua脚本
- IsFloat: 是否启用浮动效果
- Speed: 浮动速度

#### MapMoveData.cs
定义地图移动相关的数据结构：
- canmove: 是否可以移动到该路点
- canmoveIndex: 可移动到的路点索引列表

### 4. 交互控制组件

#### AllMapClick.cs
处理地图路点的鼠标交互：
- 鼠标悬停效果（上浮）
- 鼠标点击效果（缩放）
- 触发路点事件

#### BtnClick.cs
处理按钮点击事件：
- 鼠标交互视觉反馈
- 执行绑定的事件回调

#### CameraController.cs
控制地图摄像机的行为：
- 鼠标滚轮缩放
- 鼠标拖拽移动
- 边界限制

### 5. 视觉效果组件

#### GravityAnim.cs
实现浮动动画效果：
- 正弦波运动模拟漂浮
- 可调节的浮动速度和幅度

#### AllMapLineShow.cs
显示地图路点间的连接线：
- 自动绘制所有路点之间的连接关系
- 用于调试和可视化路径

### 6. NPC管理系统

#### AllMapNpcController.cs
控制地图上的NPC行为：
- NPC移动到指定路点
- 动画状态管理（移动/待机）
- 方向朝向控制
- 生命周期管理

#### NpcInfo.cs
存储NPC详细信息：
- 基本属性（门派、等级、遁速等）
- 名称和外观数据
- 遁术类型设置

### 7. 场景管理

#### YZSceneManeger.cs
负责场景加载管理：
- 监听场景加载事件
- 在特定场景中初始化地图系统

#### LoadingScreenPatch.cs
补丁类，用于在加载场景时预加载资源：
- 加载地图所需的AssetBundle资源

## 工作流程

1. **场景初始化**：当进入"幽州"场景时，YZSceneManeger会创建AllMapBase实例
2. **数据加载**：AllMapBase加载JSON数据并初始化地图组件
3. **组件注册**：为每个路点添加AllMapComponent和交互组件
4. **NPC生成**：根据数据在地图上生成NPC并控制其行为
5. **用户交互**：用户可以通过鼠标与地图路点交互，触发事件或移动
6. **玩家移动**：支持普通移动和遁术移动两种方式到达目标路点

## 特色功能

1. **多种移动方式**：支持普通寻路移动和遁术瞬移
2. **动态路径规划**：自动计算最优路径
3. **丰富的交互效果**：鼠标悬停、点击反馈、浮动动画等
4. **灵活的事件系统**：支持事件和Lua脚本绑定
5. **NPC智能行为**：NPC可自主移动并在地图上显示
6. **摄像机控制**：支持缩放和平移的大地图浏览体验

## 如何创建第二个与幽州一样的地图

要创建第二个与幽州相同机制的地图，需要以下步骤：

### 1. 创建新的场景文件
- 在Unity中创建一个新的场景，命名为如"F新地图名"
- 按照幽州地图的结构布置路点（使用相同的预制件结构）

### 2. 创建对应的数据文件
- 创建新的JSON数据文件描述新地图的路点信息
- 确保数据格式与幽州地图一致（参考现有JSON结构）

### 3. 修改场景管理代码
在`YZSceneManeger.cs`中添加新的场景监听：
```csharp
public void SceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
{
    string name = scene.name;
    if (name == "F幽州")
    {
        AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
        allMapBase.gameObject.AddComponent<SceneBase>();
        SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
        AllMapBase.RefreshMarksFromStaticData();
    }
    else if (name == "F新地图名")  // 添加新的地图处理逻辑
    {
        AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
        allMapBase.gameObject.AddComponent<SceneBase>();
        SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
        AllMapBase.RefreshMarksFromStaticData();
    }
}
```

### 4. 添加资源加载逻辑
在`LoadingScreenPatch.cs`中添加新地图资源的预加载：
```csharp
[HarmonyPatch("LoadScene")]
public static void Prefix()
{
    if (!MyUtil.init)
    {
        MyUtil.init = true;
        string path = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/幽州.ab";
        AssetBundle.LoadFromFile(path);
        
        // 添加新地图资源加载
        string newPath = IsToolsMain.modPath + "/BaizeAssets/AssetBundle/Scene/新地图.ab";
        AssetBundle.LoadFromFile(newPath);
    }
}
```

### 5. 配置地图数据访问
确保`Jsondata.SceneJsonData`中包含新地图的键值对，以便`AllMapBase`能够正确加载新地图数据。

### 6. 调整路径规划（如需要）
如果新地图的路点连接方式与幽州不同，可能需要调整`YZMove.cs`中的路径规划逻辑。

### 7. 为新地图设置独立的移动速度
为了使新地图拥有独立于幽州地图的移动速度，可以修改`AllMapComponent.cs`中的移动速度逻辑：

1. 在`AllMapComponent.cs`中添加地图名称判断逻辑：
```csharp
// 在AvatarMoveToThis方法中修改速度计算部分
public override void AvatarMoveToThis()
{
    MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
    bool hasDunShu = playerController.ShowType != MapPlayerShowType.遁术;

    // 获取当前地图名称
    string currentMapName = Tools.getScreenName();
    
    // 根据不同地图设置不同的基础移动速度
    float mapMoveBaseSpeedMin = 1.5f;
    float mapMoveBaseSpeed = 4f;
    
    if (currentMapName == "新地图名称")
    {
        // 为新地图设置不同的移动速度
        mapMoveBaseSpeedMin = 2.0f;  // 新地图最小基础移动速度
        mapMoveBaseSpeed = 5.0f;     // 新地图基础移动速度
    }
    // 幽州地图或其他地图使用默认速度

    // 获取当前地图数据
    MapMoveData currentMapData = null;
    IsToolsMain.MapMoveDatas.TryGetValue(this.NodeIndex, out currentMapData);

    if (hasDunShu)
    {
        if (currentMapData != null && currentMapData.canmove)
        {
            // 可以普通移动，计算路径
            MoveToNode(this.NodeIndex, false, mapMoveBaseSpeedMin, mapMoveBaseSpeed);
        }
        else
        {
            // 没有飞行遁术且不允许普通移动
            UIPopTip.Inst.Pop("没有飞行遁术，无法抵达目标地点。", PopTipIconType.叹号);
        }
    }
    else
    {
        // 使用遁术移动
        MoveToNode(this.NodeIndex, true, mapMoveBaseSpeedMin, mapMoveBaseSpeed);
    }
}
```

2. 修改`MoveToNode`方法以接受速度参数：
```csharp
public void MoveToNode(int targetNodeIndex, bool useDunShu, float moveBaseSpeedMin, float moveBaseSpeed)
{
    // ... 原有代码 ...

    if (!useDunShu)
    {
        // 普通移动模式 - 使用路径查找
        List<int> path = FindPath(avatarNowMapIndex, targetNodeIndex);
        if (path == null || path.Count == 0)
        {
            UIPopTip.Inst.Pop("无法找到前往目标地点的路径。", PopTipIconType.叹号);
            return;
        }

        StartCoroutine(MoveAlongPath(path, playerController, nowComp, moveBaseSpeedMin, moveBaseSpeed));
        AllMapBase.RefreshMarksFromStaticData();
    }
    else
    {
        // 遁术移动模式
        float distance = Vector2.Distance(nowComp.transform.position, targetMapCompont.transform.position);
        float speed = (player.dunSu > 200) ?
            ((moveBaseSpeedMin + moveBaseSpeed) * 2f) :
            (moveBaseSpeedMin + moveBaseSpeed * ((float)player.dunSu / 100f));
        speed *= 2f;
        float duration = distance / speed;

        // ... 后续代码保持不变 ...
    }
}
```

### 注意事项
- 确保新地图的所有路点命名规范（使用数字索引）
- 保持与幽州地图相同的组件结构和层级关系
- 测试新地图的所有功能，包括移动、事件触发、NPC行为等
- 确保不同地图间的移动速度调整不会相互影响