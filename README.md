# 类 I Wanna 2D 横板跑酷游戏

<p align="center">
  <img alt="Unity" src="https://img.shields.io/badge/Unity-2022-000000?style=flat-square&logo=unity&logoColor=white">
  <img alt="Windows" src="https://img.shields.io/badge/Platform-Windows-0078D6?style=flat-square&logo=windows&logoColor=white">
  <img alt="C#" src="https://img.shields.io/badge/Language-C%23-239120?style=flat-square&logo=csharp&logoColor=white">
  <img alt="Version" src="https://img.shields.io/badge/Version-v1.0-2088FF?style=flat-square">
</p>

## 项目简介

一款类 I Wanna（I Wanna Be The Guy）风格的 2D 横板跑酷游戏。使用 Unity 2022 开发，运行于 Windows 平台。

## 游戏功能

### 主菜单

- 主菜单可跳转至：选择关卡、团队介绍、设置、退出游戏。
- 选择关卡：可进入已解锁的任意关卡，闯关进度从配置文件（`config.json`）读取。
- 设置：可调节音乐/音效音量，并可更换背景音乐。
- 团队介绍：展示开发团队姓名。
- 所有页面左上角均提供返回主菜单按钮，或按 `Esc` 返回。

### 关卡玩法

- 玩法与 I Wanna 相同：2D 横板跳跃，支持二段跳。
- 暂停：关卡内左上角（或按 `Esc`）打开暂停菜单，冻结游戏时间，可重新挑战 / 继续 / 返回主菜单。
- 挑战信息：界面右上角显示本关挑战时长与死亡次数。
- 子弹：按 `J` 键向人物当前朝向发射一颗子弹，子弹可命中按钮并触发机关。
- 通关：到达终点后显示通关提示并进入下一关；最后一关通关后显示通关界面并返回主菜单。
- 死亡：死亡后播放死亡动画，并回到出生点或最近一次存档点。

## 技术概要

- 引擎：Unity 2022（2D，Windows）
- 架构：主菜单单场景 + 每关一场景；组件只通知、GameManager 决策；Manager 常驻（GameManager / EventCenter / AudioManager / ProgressManager / UIManager）
- 存档：关卡进度与设置统一存储于 `config.json`，由 ProgressManager 管理

## 目录结构

- `UnityProject/`：Unity 工程
- `arch/`：架构与开发规范文档（Obsidian 索引 + 知识图谱）
- `docs/`：其他文档
- `.gitignore` / `.gitattributes`：Git 配置

## 开发团队

A–G 共 7 人，具体分工见 `arch/arch-main/开发分工.md`。
