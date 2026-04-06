# WalkMap 模式改动说明

## 目标
- 在现有大地图系统上新增第二种地图模式。
- 该模式下玩家通过 `W/A/S/D` 自由移动。
- 原本“当前激活节点直接显示入口点”的表现，改为“玩家靠近节点时才显示入口点”。
- 不改 `AllMapBase` 和原地图资源逻辑，只在特殊判断文件决定哪些场景启用新模式。

## 实际改动

### 1. 新增模式控制文件
- 文件：`Scene/walkMap/WalkMapModeConfig.cs`
- 作用：作为唯一的模式开关文件。
- 行为：通过 `EnabledSceneNames` 集合决定哪些场景进入 WalkMap 模式。

示例：

```csharp
private static readonly HashSet<string> EnabledSceneNames = new HashSet<string>
{
    "F雪剑域"
};
```

只要把场景名加入这个集合，该场景就会启用第二种地图模式。

### 2. 新增 WalkMap 运行时控制器
- 文件：`Scene/walkMap/WalkMapController.cs`
- 负责内容：
  - 读取 `W/A/S/D` 输入并移动地图玩家。
  - 玩家移动范围根据 `/AllMap/MapLand` 的边界限制，并额外收缩一圈内边距。
  - 主相机可见范围和正交尺寸也基于 `/AllMap/MapLand` 计算，并额外减去可见边距，避免初始视野过大导致页面抖动。
  - 关闭原有路点点击移动：禁用 `AllMapClick`，避免和新模式冲突。
  - 入口点仍然沿用原逻辑获取方式：`transform.Find("enter")`。
  - 根据玩家与节点距离控制 `enter` 显示/隐藏。
  - 玩家靠近某节点时，同步该节点为当前 `NowIndex`，保证入口交互和原有节点语义一致。

### 3. 仅在场景接入点做模式判断
- 文件：`Scene/YZSceneManeger.cs`
- 改动方式：保留原有场景加载流程，只抽出一个公共创建方法。
- 接入逻辑：创建 `AllMapBase` 后，如果 `WalkMapModeConfig.IsWalkMapScene(scene.name)` 返回 `true`，就额外挂载 `WalkMapController`。
- 这就是新模式的唯一接入点，不需要修改 `AllMapBase.cs`。

### 4. 更新工程编译项
- 文件：`YouZhou.csproj`
- 新增编译文件：
  - `Scene/walkMap/WalkMapController.cs`
  - `Scene/walkMap/WalkMapModeConfig.cs`

## 未修改内容
- `Scene/AllMapBase.cs` 未改。
- 原有地图节点资源、`enter` 子物体结构未改。
- 原地图模式逻辑仍然完整保留。

## 使用方式
1. 打开 `Scene/walkMap/WalkMapModeConfig.cs`。
2. 把目标场景名加入 `EnabledSceneNames`。
3. 进入该场景后，就会自动切换到 WalkMap 模式。

## 兼容性说明
- 未加入 `EnabledSceneNames` 的场景，继续使用原来的点击移动地图模式。
- 新模式只影响命中的场景，不会改写其他地图行为。
