---
title: A-G 快速上手清单
tags: [分工, 教程, 上手]
created: 2025-08-20
status: 正式版 v1.0
---

# A–G 快速上手清单

> 面向 7 位成员的"先做什么"。分工依据见 [[开发分工]]，各 Manager 接口见 [[功能总览]]。

## 通用第一步（人人必做）

1. 先读 [[架构总览]] 和 [[代码与命名规范]]（目录/命名/事件名/Tag-Layer/键位是硬约定，**第 1 天必须定死**）。
2. 克隆仓库，切到 `dev` 分支（见 [[Git 规范]]）。
3. 把 Unity 工程做两处设置：`Visible Meta Files` + `Force Text`。

---

## A：核心逻辑 + 工程协作

- **负责**：`GameManager`、`EventCenter`、`Singleton<T>` + `Bootstrap`、Git、整合/测试/打包、文档维护。
- **先读**：[[架构总览]]、[[代码与命名规范]]（第 1、5 节）、[[玩法系统规范]]、[[功能总览]]（第 1、2、6 节）、[[Git 规范]]。
- **首先**：
  1. 建仓库 + `dev` 分支；Unity 建工程 `UnityProject/`。
  2. 写 `Singleton<T>` 基类 + `EventCenter` + `GameEvent` 枚举。
  3. 在 `MainMenu` 场景搭 `Bootstrap`，挂各 Manager。
  4. 写 `GameManager` 状态机骨架：`LoadLevel` / `RestartLevel` / `ResumeGame` / `PauseGame` / `BackToMainMenu`。
- **依赖**：调用 `B:ProgressManager`（解锁/校验）、`C:UIManager`（弹 UI）；订阅 `E` 组件的死亡/存档/通关事件。
- **交付**：`GameManager`、`EventCenter`、`Bootstrap`、发版、测试报告。

## B：音频 + 存档

- **负责**：`AudioManager`、`ProgressManager`、`config.json` + `ProgressData`。
- **先读**：[[架构总览]]、[[配置与存档规范]]（全文）、[[功能总览]]（第 3、4 节）、[[代码与命名规范]]。
- **首先**：
  1. 定义 `ProgressData` 类 + `config.json` 结构（`totalLevels=3`、`unlockedLevels=[1]`、音量、`bgmIndex`）。
  2. 写 `ProgressManager`：`Load` / `IsLevelUnlocked` / `UnlockLevel` / `TotalLevels` / `MusicVolume` / `SFXVolume` / `BgmIndex`（`Save` 私有）。
  3. 写 `AudioManager`：`PlayBGM` / `SetMusicVolume` / `SetSFXVolume` / `PlaySFX`。
- **依赖**：`ProgressManager` 被 `A:GameManager`（解锁）和 `C`（设置页）调用。
- **交付**：`AudioManager`、`ProgressManager`、JSON 结构。

## C：UI + 美术（G 协助）

- **负责**：`UIManager`、主菜单场景、HUD、暂停面板、通关提示 + 通关 UI、`Art/Environment`、`Art/Animation`。
- **先读**：[[架构总览]]、[[场景与流程规范]]（场景清单/流程/暂停/切换调用链）、[[功能总览]]（第 5 节）、[[代码与命名规范]]。
- **首先**：
  1. 搭 `MainMenu` 场景（主界面 / 选关 / 设置 / 团队介绍）。
  2. 写 `UIManager`：`ShowHUD` / `ShowPauseMenu` / `HidePauseMenu` / `ShowClearHint` / `ShowCompleteUI`。
  3. 准备占位美术（`Art/Environment`、`Art/Animation`）。
- **依赖**：设置页调 `B:ProgressManager`；选关调 `A:GameManager.LoadLevel`；HUD 订阅 `DeathCountChanged`/`TimerTick`。
- **交付**：`MainMenu` 场景、UI Prefab、`UIManager`、美术资源。

## D：玩家 + 子弹

- **负责**：`PlayerController`、`Bullet`。
- **先读**：[[玩法系统规范]]（第 1、2 节）、[[代码与命名规范]]（键位/Tag-Layer）、[[动画事件回调示例]]。
- **首先**：
  1. 写 `PlayerController`：`GetAxisRaw` 移动、二段跳、朝向、J 键射击、`Die()`/`Respawn()`。
  2. 写 `Bullet`（哑弹）：直线飞行、撞墙销毁、超时销毁。
  3. 做玩家死亡动画 + 最后一帧 Animation Event → `OnDeathAnimationFinished()`。
- **依赖**：`Die`/`Respawn` 被 `A:GameManager` 调用；子弹被 `F` 的按钮检测。
- **交付**：`Player` Prefab、`Bullet`、`PlayerController`。

## E：玩法组件

- **负责**：`Spike`、`SavePoint`、`Goal`。
- **先读**：[[玩法系统规范]]（第 4、5、6 节）、[[代码与命名规范]]（事件中心/Tag-Layer）、[[事件中心与广播订阅示例]]。
- **首先**：
  1. `Spike`：`OnTriggerEnter2D` 检测玩家 → 广播 `PlayerDeath`；订阅 `PlayerRespawned` 复位。
  2. `SavePoint`：广播 `SavePointReached(位置)` + 换贴图 + 只触发一次。
  3. `Goal`：广播 `LevelComplete` + `isTriggered` 防连发。
- **依赖**：只广播事件，由 `A:GameManager` 处理。
- **交付**：`Spike` / `SavePoint` / `Goal` Prefab + 脚本。

## F：关卡

- **负责**：`ButtonTarget`、`Door` + `MovingPlatform`、关卡场景搭建（`Level_01`~`Level_03`）。
- **先读**：[[玩法系统规范]]（第 3 节按钮/机关）、[[代码与命名规范]]（Layer/目录 Level/）、[[碰撞矩阵配置说明]]。
- **首先**：
  1. `ButtonTarget`：`OnTriggerEnter2D` 检测子弹 → 触发机关 + 销毁子弹。
  2. `Door` / `MovingPlatform` 机关脚本。
  3. 搭 `Level_01` 场景：地形、摆放 `E` 的组件、按钮→机关连线。
- **依赖**：使用 `E` 的组件 Prefab；按钮检测 `D` 的子弹；需按 [[碰撞矩阵配置说明]] 配置 Layer。
- **交付**：3 个关卡场景、`ButtonTarget`、`Door` / `MovingPlatform`。

## G：协助 C

- **负责**：与 `C` 共同完成 UI + 美术（4、14–20）。
- **先读**：同 `C`（[[场景与流程规范]]、[[功能总览]] 第 5 节、[[代码与命名规范]]）。
- **首先**：与 `C` 分工——G 可先负责美术资源（`Art/Environment`、`Art/Animation` 占位图）或 HUD / 暂停面板。
- **交付**：协助 `C` 的 UI / 美术产出。

---

## 依赖关系速览

```
A(GameManager/EventCenter) ← 依赖 ← B(ProgressManager) + C(UIManager) + E(组件事件) + D(玩家)
B(ProgressManager/Audio)   ← 被 A、C 调用
C(UI/美术)                 ← 调 A(切场景)、B(设置)；G 协助
D(玩家/子弹)               ← 玩家接口被 A 调用；子弹被 F 检测
E(组件)                    ← 只广播事件给 A
F(关卡)                    ← 用 E 的组件、检测 D 的子弹
```

