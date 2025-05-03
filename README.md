# game-archive-manager
using c# and sql to manage archive of games

项目结构
```
.
├── LICENSE
├── README.md
├── game-archive-manager
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── Assets
│   │   ├── LockScreenLogo.scale-200.ico
│   │   ├── Wide310x150Logo.scale-200.png
│   │   └── pic
│   ├── ContentDialogs
│   │   ├── GameConfig.xaml
│   │   ├── GameConfig.xaml.cs
│   │   ├── SignInContentDialog.xaml
│   │   └── SignInContentDialog.xaml.cs
│   ├── Controls
│   │   ├── ContentShow.xaml
│   │   ├── ContentShow.xaml.cs
│   │   ├── GameArchiveShow.xaml
│   │   ├── GameArchiveShow.xaml.cs
│   │   ├── ImageUploader.xaml
│   │   └── ImageUploader.xaml.cs
│   ├── DataItems
│   │   ├── Archive.cs
│   │   ├── ControlInfoDataItem.cs
│   │   ├── DB
│   │   ├── Game.cs
│   │   ├── GameInfo.cs
│   │   ├── GameRule.cs
│   │   ├── ImageData.cs
│   │   ├── MatchRule.cs
│   │   ├── Role.cs
│   │   ├── Rule.cs
│   │   ├── User.cs
│   │   ├── UserGame.cs
│   │   └── UserRule.cs
│   ├── Helper
│   │   ├── DifyClient.cs
│   │   ├── FileHelper.cs
│   │   ├── PasswordHasher.cs
│   │   ├── RuleHelper.cs
│   │   └── WindowHelper.cs
│   ├── LoginWindow.xaml
│   ├── LoginWindow.xaml.cs
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── Package.appxmanifest
│   ├── Pages
│   │   ├── HomePage.xaml
│   │   ├── HomePage.xaml.cs
│   │   ├── LoginPage.xaml
│   │   ├── LoginPage.xaml.cs
│   │   ├── RegisterPage.xaml
│   │   ├── RegisterPage.xaml.cs
│   │   ├── SettingsPage.xaml
│   │   └── SettingsPage.xaml.cs
│   └── game-archive-manager.csproj
├── game-archive-manager.sln
└── git
```

运行项目：
1. 运行`game-archive-manager.sln`
2. 使用debug模式打包文件
3. 运行打包文件

数据库文件和日志会存储在文档目录中，图片信息则会存在AppData中。

---

# 游戏存档加密管理系统

## 项目概述

本项目旨在开发一个高效、稳定且易于维护的游戏存档加密管理系统。该系统通过模块化设计、AES对称加密技术和SQLCipher数据库加密方法，确保了游戏存档的安全存储与管理。同时，利用WinUI框架构建了用户友好的图形界面，提升了用户体验，并集成了AI助手功能以帮助用户更便捷地配置规则。

## 功能特性

### 基本功能
- **展示存档名称**：清晰地显示所有游戏存档的名称。
- **载入存档功能**：允许用户加载已备份的存档文件。
- **数据管理功能**：支持查看、修改和保存存档信息。
- **安全认证功能**：提供用户登录功能，并确保只有授权用户才能访问和修改游戏信息。密码文件经过加密处理以保障安全性。
- **数据查询功能**：根据特定条件（如游戏名称或日期）查询存档。
- **数据库的安全保护**：采用SQLCipher对数据库进行加密，保证数据的安全性。
- **图形化的UI界面设计**：通过WinUI框架构建直观易用的用户界面，提升用户体验。
- **记录用户操作**：系统会记录用户的每一步操作，确保数据可追溯。

### 扩展功能
- **文件加密和解密**：系统支持对游戏存档文件进行AES对称加密和解密，确保数据的安全性。
- **AI助手集成**：在设置界面中集成了AI助手，帮助用户更方便地配置规则。

## 安装指南

### 环境要求
- Windows 10及以上版本
- .NET SDK 6.0或更高版本
- Visual Studio 2022或更高版本（推荐）

### 步骤

1. **克隆仓库**
   ```bash
   git clone https://gitlab.yujieweb.top/starsers/game-archive-manager.git
   cd game-archive-manager
   ```

2. **安装依赖**
   使用NuGet包管理器安装所需的依赖项：
   ```bash
   dotnet restore
   ```

3. **构建项目**
   在命令行中运行以下命令来构建项目：
   ```bash
   dotnet build -p:Platform=x64 -p:RuntimeIdentifier=win-x64
   ```

4. **运行项目**
   构建成功后，可以通过以下命令运行项目：
   ```bash
   dotnet run --project ./game-archive-manager.csproj  -p:Platform=x64 -p:RuntimeIdentifier=win-x64
   ```

5. **配置数据库**
   首次运行时，系统会自动生成并初始化数据库。请确保数据库文件路径正确，并且具有写权限。

## 使用说明

### 用户注册与登录
- 打开应用程序，进入登录界面。
- 如果是新用户，请点击“注册”按钮，填写用户名、密码和邮箱等信息完成注册。
- 已注册用户可以直接输入用户名和密码登录系统。

### 存档管理
- 登录成功后，主界面会展示所有游戏存档。
- 用户可以添加新的存档、载入已有存档、修改存档信息以及删除存档。
- 系统支持根据游戏名称或日期等条件查询存档。

### 数据加密与解密
- 系统自动对存档文件进行AES对称加密和解密，确保数据的安全性。
- 用户无需手动操作加密和解密过程，系统会在后台自动处理。

### AI助手
- 在设置界面中，用户可以找到AI助手功能，帮助更方便地配置规则。

## 贡献指南

我们欢迎任何贡献者为本项目做出贡献。请按照以下步骤提交您的贡献：

1. Fork本仓库到您的GitHub账户。
2. 创建一个新的分支（例如：`feature/new-feature`）。
3. 提交您的更改。
4. 提交Pull Request。

## 技术栈

- **编程语言**：C#
- **框架**：WinUI 3
- **数据库**：SQLite + SQLCipher
- **加密技术**：AES对称加密
- **其他工具**：Visual Studio, Git

## 许可证

本项目采用MIT许可证。详情请参阅[LICENSE](LICENSE)文件。

---

希望这个README文件能够帮助用户更好地理解和使用你的游戏存档加密管理系统。如果有任何进一步的需求或修改，请随时告诉我！