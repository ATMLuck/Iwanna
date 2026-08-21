---
title: Git 规范
tags: [Git, 规范, 架构]
created: 2025-08-20
status: 正式版 v1.0
---

# Git 规范

> 仓库创建、分支划分（main/docx/arch/dev/debug）、提交规范。

## 1. 仓库创建

```bash
# 方式一：本地初始化后推远程
git init
git add .
git commit -m "chore: 初始化项目骨架"
git branch -M main
git remote add origin <远程仓库地址>   # GitHub
git push -u origin main

# 方式二：先在网页建空仓库，再 clone 后提交
git clone <远程仓库地址>
# 放入工程文件后：
git add .
git commit -m "chore: 初始化项目骨架"
git push
```

**提交前先做 Unity 配置（否则会导致多人合并冲突）**：
- `Edit > Project Settings > Editor`：
  - `Asset Serialization` → **Force Text**
- 这个设置保证 `.meta` 文件被纳入版本控制、场景/预制体以文本存储，可正常 diff/merge。

### 1.1 仓库初始结构

```
<仓库根>/
├─ UnityProject/    Unity 2022 工程（Assets、Packages、ProjectSettings 等）
├─ arch/            架构文档（本套 Obsidian 索引 + 知识图谱）
├─ docs/            其他文档（需求、会议记录等，对应 docx 分支）
├─ TempContext.md   AI 上下文存档（可入库，便于共享进度）
└─ .gitignore
```

- 文档与工程同仓库、不同目录，便于按目录/分支管理。

## 2. 分支划分（5 条常驻分支）

| 分支 | 用途 | 谁提交 |
|---|---|---|
| `main` | 稳定可发布版本，只接受 `dev` 的合并 | A:整合/测试(QA) |
| `dev` | 日常开发集成主线，功能分支都合到这里 | 全员 |
| `docx` | 文档（本 `arch/` 之外的说明文档、README 等） | A:文档负责人 |
| `arch` | 架构文档（本 `arch/` 目录） | A:架构负责人 |
| `debug` | 调试修复（修 bug、临时调试代码） | A:整合/测试(QA) |

**临时分支命名**：新功能 `feature/xxx`，修 bug `fix/xxx`；从 `dev` 切出，完成后合回 `dev`，不在 `dev` 上直接开发。

**工作流**：
1. 各功能从 `dev` 切 `feature/xxx` / `fix/xxx`，完成后合回 `dev`。
2. 文档改动提交到 `docx`，架构改动提交到 `arch`，修 bug 用 `debug` 分支（修完合回 `dev`）。
3. `dev` 稳定后通过 **Pull Request** 合并到 `main`（GitHub 给 `main` 设**分支保护**，仅 A 能合并），打 tag 发版。

**合并方式**：
- `dev`：直接 push。
- `main`：GitHub 分支保护 + Pull Request（仅 A 合并，保证发版稳定）。

```bash
# 从 dev 切功能分支
git checkout dev
git pull
git checkout -b feature/player-controller

# 开发完成合并回 dev
git add .
git commit -m "feat: 玩家移动与二段跳"
git checkout dev
git merge feature/player-controller
git push
```

## 3. 提交规范（Conventional Commits 风格）

格式：`<type>: <中文简述>`（**统一中文**），一次提交只做一件事。

| type | 含义 | 示例 |
|---|---|---|
| `feat` | 新功能 | `feat: 子弹命中按钮触发机关` |
| `fix` | 修 bug | `fix: 死亡后计时未清零` |
| `docs` | 文档 | `docs: 补充开发分工说明` |
| `refactor` | 重构（不改行为） | `refactor: 抽取事件中心` |
| `chore` | 杂项/构建 | `chore: 添加 .gitignore` |
| `test` | 测试 | `test: 增加关卡流自测` |

**规则**：
- 提交前 `git status` 检查，不把 `Library/`、`Temp/` 等生成物提交进去（靠 `.gitignore`）。
- 不要一次提交塞进"功能 + 调试 + 文档"混杂物。

## 4. Unity .gitignore 与 .gitattributes

使用 Unity 官方 `.gitignore`（或至少包含以下），放在仓库根目录：

```gitignore
# Unity 生成物（不入库）
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]serSettings/
[Mm]emoryCaptures/
# IDE 生成
*.csproj
*.sln
*.user
.vs/
.idea/
# 系统
.DS_Store
```

- 必须入库：`Assets/`、`Packages/`、`ProjectSettings/`（含 `.meta` 文件）。

### 4.1 .gitattributes

放在仓库根目录，规范换行符、把 Unity 资源标记为文本/二进制，减少合并冲突：

```gitattributes
# Unity 文本资源（按文本 diff/merge）
*.unity text
*.prefab text
*.asset text
*.mat text
*.meta text
*.anim text
*.controller text
*.spriteatlas text
*.mixer text

# 代码统一 LF 换行
*.cs text eol=lf
*.shader text eol=lf

# 二进制资源（不 diff）
*.png binary
*.jpg binary
*.jpeg binary
*.gif binary
*.psd binary
*.tga binary
*.fbx binary
*.wav binary
*.mp3 binary
*.ogg binary
*.mp4 binary
*.dll binary
*.exe binary
```

- 作用：让 Git 知道哪些文件是文本（可正常 diff/merge）、哪些是二进制；统一 `LF` 换行，避免 Windows/Linux 换行符差异导致的冲突。

