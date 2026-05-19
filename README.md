# HW2_Gomoku - 五子棋 Windows Forms 遊戲

## 專案簡介

本專案是「視窗程式設計 (II) 作業二：棋牌類遊戲」的五子棋遊戲。使用 C# Windows Forms 製作，支援黑白棋輪流下棋、勝負判斷、重新開始、棋子圖片顯示與音效播放。

## 功能特色

- 15 x 15 五子棋棋盤
- 黑棋先手，黑白雙方輪流點擊棋盤交叉點下棋
- 橫向、直向、左斜、右斜任一方向連成五顆即獲勝
- 視窗採用寬版配置，左側棋盤與右側文字區分開，避免棋盤擋到說明文字
- 右側狀態欄顯示目前回合或勝利結果
- 可按「重新開始」重置棋局
- 使用 `black.png` 與 `white.png` 顯示棋子圖片
- 使用 `place.wav` 播放下棋音效，使用 `win.wav` 播放勝利音效

## 執行環境

- Visual Studio 2022
- .NET 8 SDK
- Windows Forms / .NET Desktop Development workload

## 執行方式

1. 使用 Visual Studio 開啟 `HW2_Gomoku.sln`。
2. 確認啟動專案為 `HW2_Gomoku`。
3. 按下 `F5` 或點選「開始偵錯」執行。
4. 遊戲開始後，黑棋先手，雙方輪流點擊棋盤交叉點。

## 遊戲畫面截圖

### 遊戲進行中

![main screenshot](docs/screenshot_main.png)

### 黑棋獲勝畫面

![win screenshot](docs/screenshot_win.png)

## 專案結構

```text
HW2_Gomoku_Submission/
├── HW2_Gomoku.sln
├── HW2_Gomoku/
│   ├── HW2_Gomoku.csproj
│   ├── Program.cs
│   ├── Form1.cs
│   └── Resources/
│       ├── black.png
│       ├── white.png
│       ├── place.wav
│       └── win.wav
├── docs/
│   ├── screenshot_main.png
│   └── screenshot_win.png
├── report.docx
├── 學號_姓名.txt
├── README.md
└── .gitignore
```

## GitHub 上傳注意事項

請確認 GitHub 倉庫有包含：

- 原始程式碼
- README.md
- .gitignore
- 截圖

不要上傳以下編譯或暫存資料夾：

- `bin/`
- `obj/`
- `.vs/`
- `.git/` 不需要放進繳交壓縮檔

## 資料來源

- 遊戲玩法：一般五子棋規則
- 棋子圖片與音效：本專案自製簡易素材
