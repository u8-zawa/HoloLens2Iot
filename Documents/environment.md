# 環境構築

## 必要なもの

- Visual Studio 2019
- Unity 2020.3.36f1

## 手順

### Visual Studio 2019 をインストール

まず、以下のリンクにアクセスして Visual Studio 2019 をダウンロードします。

> インストーラー バージョン 3.4.2244.14676\
> Visual Studio Community 2019 16.11.21

<https://learn.microsoft.com/ja-jp/visualstudio/releases/2019/release-notes>

![Visual Studio 2019 のダウンロード](https://user-images.githubusercontent.com/78470202/201889808-91d0b9e1-e185-413a-918d-117028854359.png)

Visual Studio Installer が開いたら、

![Visual Studio Installer (1)](https://user-images.githubusercontent.com/78470202/201891040-c619e3fc-5b43-4386-8f8f-0a9037f689cb.png)

### インストールするコンポーネントを選ぶ

インストールするコンポーネントを選びます。

- [x] **C++によるデスクトップ開発**
  - [x] MSVC v142 -VS 2019 C++ x64/x86 ビルドツール (最新)
  - [x] Windows 10 SDK (10.0.1904.0)
  - [x] Just-In-Time デバッカー
  - [x] C++ のプロファイル ツール
  - [x] Windows 用 C++ CMake ツール
  - [x] 最新の v142 ビルド ツール用 C++ ATL (x86 および x64)
  - [x] Boost.Test のテスト アダプター
  - [x] Test Adapter for Google Test
  - [x] Live Share
  - [x] C++ AddressSanitizer
  - [x] MSVC v142 - VS 2019 C++ ARM64 ビルド ツール (最新)
  - [x] Windows 10 SDK (10.0.16299.0)
- [x] **ユニバーサルWindowsプラットフォーム開発**
  - [x] IntelliCode
  - [x] USB デバイスの接続
  - [x] C++ (v142) ユニバーサル Windows プラットフォーム ツール
  - [x] DirectX 用グラフィックス デバッカーおよび GPU プロファイラー
  - [x] Windows 10 SDK (10.0.16299.0)
-[x] **個別のコンポーネント**
  - [x] Windows 11 SDK (10.0.22000.0)  **(← OSがWindows11の場合必要になるかも)**

![Visual Studio Installer (2)](https://user-images.githubusercontent.com/78470202/201918817-561d5f94-60c4-4c3b-bdcb-225f61fe4f53.png)

![Visual Studio Installer (3)](https://user-images.githubusercontent.com/78470202/201918812-001f2c9d-e272-431b-8404-c061d597ff65.png)

![Visual Studio Installer (4)](https://user-images.githubusercontent.com/78470202/201918814-447beba7-47aa-4656-a1fe-4d7101f443be.png)

### Unity をインストール

まず、以下のリンクにアクセスして Unity Hub をダウンロード・インストールします。

> Unity Hub 3.3.0

<https://unity3d.com/jp/get-unity/download>

![Unity (1)](https://user-images.githubusercontent.com/78470202/201917281-e592db56-9b7a-4001-9dac-e8a7e83fd6c2.png)

インストールできたら、Unity Hub を開いて Editor をインストールします。

> Unity 2020.3.36f1

![Unity (2)](https://user-images.githubusercontent.com/78470202/201920427-c8be5ef2-d9c2-4a50-a140-da52c63b0960.png)

![Unity (3)](https://user-images.githubusercontent.com/78470202/201921780-24103482-9865-4ff8-95b5-8aced71ed117.png)

![Unity (4)](https://user-images.githubusercontent.com/78470202/201921812-67211281-8ee7-4b3d-9fe7-6ef76ba13eb2.png)

![Unity (5)](https://user-images.githubusercontent.com/78470202/201922558-af0d644f-8f06-403c-8eb8-5837b73dca37.png)

![Unity (6)](https://user-images.githubusercontent.com/78470202/201922904-73a6cd67-bfd3-479c-a44a-dbf75186109c.png)

必要なモジュールを選択する。

- Platforms
  - [x] Universal Windows Platform Build Support
  - [x] Windows Build Support (IL2CPP)
- Language Packs (Preview)
  - [x] 日本語 (← 任意)
- Documentation
  - [x] Documentation

![Unity (7)](https://user-images.githubusercontent.com/78470202/201923141-df87b023-2144-4c3e-83aa-eff4fbd9e3ad.png)

![Unity (8)](https://user-images.githubusercontent.com/78470202/201924340-a874f5f7-0955-4c14-bdea-c75a31f0e6ea.png)

![Unity (9)](https://user-images.githubusercontent.com/78470202/201924346-d12154cd-25e1-4cba-810a-0d021c34b61a.png)

### 追記

Visual Studio 2019 はアプリをビルドするときに必要。
Visual Studio 2022 と混在しているとビルド時に失敗するおそれあり。

<https://github.com/microsoft/MixedRealityToolkit-Unity/issues/10656>
