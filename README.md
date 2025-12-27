# DXF ファイル ジェネレーター

.NET 10を使用したJavaScriptベースのDXFファイル作成ツール

## 概要

**DXF ファイル ジェネレーター**は、JavaScriptを使用してAutoCAD DXFファイルを作成できるコマンドラインツールです。.NET 10で構築され、Jint JavaScriptエンジンを搭載しており、複雑な幾何学図面をプログラム的に生成するためのシンプルで強力なスクリプト環境を提供します。

### 主な機能

-   **JavaScriptベースのスクリプト** - 使い慣れたJavaScript構文でDXF生成スクリプトを記述
-   **豊富なジオメトリAPI** - 点、線、ポリゴン、楕円などを作成
-   **幾何学的操作** - 図形の回転、拡大縮小、ミラーリング、変換
- ?? **ポリゴンクリッピング** - ポリゴンの高度なブール演算
-   **ファイルシステムアクセス** - エンコーディング対応のファイル読み書き
-  ? **ダイアログサポート** - インタラクティブなファイル選択とユーザー入力
-   **数式評価** - 複雑な数式の評価

## プロジェクト

このリポジトリには2つの主要プロジェクトが含まれています：

- **dxf** - コアライブラリとコマンドラインツール（コンソールアプリケーション）
- **dxfUI** - インタラクティブなJavaScript実行用Windows Formsアプリケーション

## アーキテクチャ

```
dxf/
├── dxf/                   # コアライブラリとCLIツール
│   ├── DXF.cs             # DXFファイル生成
│   ├── PointD.cs          # 演算子オーバーロード対応の倍精度2D点
│   ├── Script.cs          # JavaScript実行エンジン
│   ├── JSFileItem.cs      # ファイルシステム操作
│   ├── JSFileDialog.cs    # ファイル/フォルダ選択ダイアログ
│   ├── JSApp.cs           # アプリケーション情報
│   ├── InputDialog.cs     # ユーザー入力ダイアログ
│   └── Program.cs         # CLIエントリーポイント
└── dxfUI/                 # GUIアプリケーション
    └── (Windows Forms UI)
```

## 動作環境

- **.NET 10** 以降
- **Visual Studio 2026** 以降（開発用）
- **Windows 10/11**（Windows Forms UI用）

## Library Dependencies

- [netDxf](https://github.com/haplokuon/netDxf) - DXF
- [Clipper2](https://www.angusj.com/clipper2/Docs/Overview.htm) - Clipper2
- [Jint](https://github.com/sebastienros/jint) - JavaScript interpreter for .NET
- [NCalc](https://ncalc.github.io/ncalc/articles/index.html) - Mathematical expression evaluator
- [WindowsAPICodePack-Shell](https://github.com/aybe/Windows-API-Code-Pack-1.1) - Modern Windows dialogs


## 使い方

### コマンドライン（dxf.exe）

```
dxf <スクリプトファイル.js>
```

**例：**
```
dxf examples/rectangle.js
dxf myscript
```

拡張子が指定されていない場合、自動的に `.js` が追加されます。

### スクリプトの既定フォルダ

スクリプトは以下の場所から自動的に検索されます：
1. `%AppData%\dxf\scripts\`
2. `<実行ファイルディレクトリ>\scripts\`

## JavaScript API リファレンス

### グローバル関数

#### 出力関数
```javascript
write(text)           // コンソールにテキストを出力（改行なし）
writeln(text)         // コンソールにテキストを出力（改行あり）
cls()                 // コンソール画面をクリア
```

#### ダイアログ関数
```javascript
alert(message, caption)                  // アラートダイアログを表示
answerDialog(message, caption)           // Yes/Noダイアログを表示、boolを返す
inputBox(message, caption2, caption)     // 入力ダイアログを表示、文字列またはnullを返す
```

#### クリップボード関数
```javascript
copyText(text)        // クリップボードにテキストをコピー
pasteText()           // クリップボードからテキストを取得、文字列を返す
```

#### ユーティリティ関数
```javascript
calc(expression)      // 数式を評価、数値を返す
                      // サポート: sin, cos, tan, sqrt, abs, asin, acos, atan,
                      //          log, log10, exp, floor, ceil, round, min, max, pow
ls(path)              // ディレクトリ内のファイルリストを返す

// 数値変換関数
numToHex(num, keta)   // 数値を16進数文字列に変換（keta: 桁数、既定値8）
numToBin(num, keta)   // 数値を2進数文字列に変換（keta: 桁数、既定値16）
```

**使用例:**
```javascript
// クリップボード操作
copyText("Hello, World!");
var text = pasteText();
writeln("クリップボードの内容: " + text);

// 数値変換
var hex = numToHex(255);        // "000000FF"
var hex2 = numToHex(255, 4);    // "00FF"
var bin = numToBin(15);         // "0000000000001111"
var bin2 = numToBin(15, 8);     // "00001111"
```

### PointD クラス

演算子オーバーロード対応の倍精度2D点。

```javascript
var p1 = new PointD(10, 20);
var p2 = new PointD(5, 15);

// 算術演算
var p3 = p1 + p2;           // 加算
var p4 = p1 - p2;           // 減算
var p5 = p1 * 2;            // スカラー乗算
var p6 = p1 / 2;            // スカラー除算

// ベクトル演算
var dist = p1.distanceTo(p2);          // 他の点までの距離

// 比較
var isEqual = p1.equals(p2);           // 許容誤差を持った等価比較
var isEqual2 = p1.equals(p2, 0.001);   // カスタム許容誤差で比較

// 変換
var vec2 = p1.toVector2();             // DXF Vector2に変換（Y軸反転）
var pointF = p1.toPointF();            // PointFに変換
var str = p1.toString();               // 文字列表現を取得 "(x, y)"
```

### Dxf クラス

DXFファイルの作成と操作。

```javascript
var dxf = new Dxf();

// 描画メソッド
dxf.drawLine(x0, y0, x1, y1);          // 2点間の線を描画
dxf.drawLine(pointArray);              // ポリラインを描画
dxf.drawPolygon(pointArray);           // 閉じたポリゴンを描画
dxf.drawPolygon(polygonList);          // 複数のポリゴンを描画
dxf.drawEllipse(centerPoint);          // 円/楕円を描画（centerPoint.Rが半径）
dxf.drawEllipse(centerPointArray);     // 複数の円を描画
dxf.drawSemiCircle(centerPoint, startAngle, endAngle); // 円弧を描画（centerPoint.Rが半径）

// DXFファイルを保存
dxf.save("output.dxf");

// 静的ジオメトリユーティリティ
var points = Dxf.createRect(x, y, width, height);         // 矩形を作成
var points = Dxf.createRect(center, width, height);       // 中心点から矩形を作成
var points = Dxf.createTriangle(center, sides, radius);   // 正多角形を作成（三角形以上）

// 幾何学的変換
var rotated = Dxf.rotAry(points, center, angleDegrees);   // 回転
var scaled = Dxf.scaleAry(points, center, scaleX, scaleY); // 拡大縮小（%指定）
var moved = Dxf.moveAry(points, dx, dy);                  // 移動
var mirrored = Dxf.mirrorAry(points, lineStart, lineEnd); // ミラーリング
var mirroredPoint = Dxf.mirrorPoint(point, lineStart, lineEnd); // 単一点のミラーリング

// ポリゴンクリッピング
var result = Dxf.clipping(subjectPolygons, clipPolygons, operation);
// operation: ClipOperation.Union, Intersection, Difference, Xor

// 計算
var angle = Dxf.getAngleAtVertex(point0, point1, point2); // 3点の角度を計算
var center = Dxf.aryCenter(points);                        // 点配列の重心を計算
```

### App オブジェクト

アプリケーション情報とシステム操作。

#### プロパティ
```javascript
App.exeFilePath                    // 実行ファイルのフルパス
App.exeFileName                    // 実行ファイル名
App.exeFileDir                     // 実行ファイルディレクトリ
App.exeFileNameWithoutExtension    // 拡張子なしの実行ファイル名
App.prefFilePath                   // ユーザー設定ディレクトリパス
App.prefFileName                   // 設定ファイルパス
```

#### 静的メソッド
```javascript
// コマンド実行
App.callCmdGetStd(command)         // cmd.exeコマンドを実行、標準出力を返す
App.openProcess(exePath, args)     // プロセスを起動、boolを返す
App.callProcess(exePath, args)     // プロセスを実行して待機、終了コードを返す
App.callProcessGetStd(exePath, args) // プロセスを実行、標準出力を返す

// 情報取得
App.commandLine()                  // コマンドライン引数を配列で取得
App.commandLineJson()              // コマンドライン引数をJSON文字列で取得
App.getEnv(variableName)           // 環境変数を取得
```

**使用例:**
```javascript
// コマンド実行
var output = App.callCmdGetStd("dir");
writeln(output);

// プロセス起動
if (App.openProcess("notepad.exe", "test.txt")) {
    writeln("メモ帳を起動しました");
}

// 環境変数取得
var username = App.getEnv("USERNAME");
writeln("ユーザー名: " + username);

// コマンドライン引数
var args = App.commandLine();
for (var i = 0; i < args.length; i++) {
    writeln("引数 " + i + ": " + args[i]);
}
```

### FileItem オブジェクト

ファイルとディレクトリの操作。

#### コンストラクタ
```javascript
var file = new FileItem(path);
```

#### インスタンスプロパティ
```javascript
file.fullName          // フルパス（読み書き可能）
file.name              // ファイル名
file.ext               // 拡張子
file.directory         // ディレクトリパス
file.parent            // 親ディレクトリ（FileItem）
file.length            // ファイルサイズ（バイト）
file.created           // 作成日時
file.modified          // 更新日時
file.exists            // 存在確認
file.isDirectory       // ディレクトリか確認
file.hidden            // 隠しファイル属性（読み書き可能）
```

#### インスタンスメソッド
```javascript
file.setFullName(path)             // ファイルパスを設定、boolを返す
file.remove()                      // ファイル/フォルダを削除、boolを返す
file.rename(newName)               // ファイル/フォルダ名を変更、boolを返す
file.copy(destPath)                // ファイルをコピー、boolを返す
file.move(destPath)                // ファイル/フォルダを移動、boolを返す
file.getFiles()                    // ディレクトリ内のファイル配列を取得
file.getDirectories()              // サブディレクトリ配列を取得
file.create()                      // ディレクトリを作成
file.execute()                     // ファイルを実行またはフォルダを開く
file.resolve()                     // ショートカットを解決、FileItemを返す
file.resolvePath()                 // ショートカットのターゲットパスを取得
```

#### 静的メソッド
```javascript
FileItem.encode(string)                        // URL文字列をエンコード
FileItem.decode(string)                        // URL文字列をデコード
FileItem.readAllText(path, encoding)           // テキストファイルを読み込み
FileItem.writeAllText(path, content, encoding) // テキストファイルに書き込み
FileItem.getFilesFromDir(path)                 // ディレクトリからファイル配列を取得
FileItem.getDirectoriesFromDir(path)           // ディレクトリ配列を取得
FileItem.createFolder(path)                    // フォルダを作成
FileItem.ls(path)                              // ディレクトリ内のファイルをリスト
```

**エンコーディングサポート:** `"utf-8"`, `"utf-16"`, `"shift_jis"`

#### 静的プロパティ（特殊フォルダ）
```javascript
FileItem.currentPath               // カレントディレクトリ
FileItem.appDataPath               // %AppData%
FileItem.localAppDataPath          // %LocalAppData%
FileItem.myDocumentsPath           // マイドキュメント
FileItem.desktopPath               // デスクトップ
FileItem.tempPath                  // 一時フォルダ
FileItem.userProfilePath           // ユーザープロファイル
FileItem.programFilesPath          // Program Files
FileItem.systemPath                // システムフォルダ
FileItem.commonAppDataPath         // 共通AppData
FileItem.startupPath               // スタートアップフォルダ
```

### FileDialog オブジェクト

モダンなファイル/フォルダ選択ダイアログ。

#### コンストラクタ
```javascript
var dlg = new FileDialog();
```

#### プロパティ
```javascript
dlg.fileName           // 選択されたファイル名（読み書き可能）
dlg.initialDirectory   // 初期ディレクトリ
dlg.title              // ダイアログタイトル
dlg.filter             // ファイルフィルタ（例: "Text(*.txt)|*.txt|All(*.*)|*.*"）
dlg.filterIndex        // フィルタインデックス（1から始まる）
dlg.defaultExt         // 既定の拡張子
dlg.multiSelect        // 複数選択を有効化
```

#### メソッド
```javascript
dlg.openDialog()                   // ファイルを開くダイアログを表示
dlg.openDialog(filename)
dlg.openDialog(filename, title)

dlg.saveDialog()                   // ファイル保存ダイアログを表示
dlg.saveDialog(filename)
dlg.saveDialog(filename, title)

dlg.folderDialog()                 // フォルダ選択ダイアログを表示（モダンUI）
dlg.folderDialog(title)
dlg.folderDialog(initialDir, title)
```

## サンプルスクリプト

### 基本的な矩形

```javascript
var dxf = new Dxf();

// 矩形を作成
var rect = Dxf.createRect(0, 0, 100, 50);
dxf.drawPolygon(rect);

// DXFファイルを保存
dxf.save("rectangle.dxf");
writeln("DXFファイルを作成しました: rectangle.dxf");
```

### 回転したポリゴン

```javascript
var dxf = new Dxf();

// 六角形を作成
var center = new PointD(0, 0);
var hex = Dxf.createTriangle(center, 6, 50);

// オリジナルと回転バージョンを描画
for (var i = 0; i < 12; i++) {
    var angle = i * 30;
    var rotated = Dxf.rotAry(hex, center, angle);
    dxf.drawPolygon(rotated);
}

dxf.save("rotated_hexagons.dxf");
writeln("12個の回転した六角形を作成しました");
```

### インタラクティブなファイル選択

```javascript
var dlg = new FileDialog();
dlg.filter = "DXFファイル(*.dxf)|*.dxf|すべてのファイル(*.*)|*.*";
dlg.title = "DXFファイルを保存";

var filename = dlg.saveDialog("output.dxf");
if (filename) {
    var dxf = new Dxf();

    // ジオメトリを作成（正32角形で円を近似）
    var circle = Dxf.createTriangle(new PointD(0, 0), 32, 100);
    dxf.drawPolygon(circle);
    
    // または、円を直接描画する場合：
    // var centerPoint = new PointD(0, 0);
    // centerPoint.R = 100; // 半径を設定
    // dxf.drawEllipse(centerPoint);

    // 保存
    if (dxf.save(filename)) {
        writeln("ファイルを保存しました: " + filename);
    } else {
        writeln("ファイルの保存に失敗しました");
    }
}
```

### 数学的パターン

```javascript
var dxf = new Dxf();

// 螺旋パターンを作成
var center = new PointD(0, 0);
var steps = 100;
var maxRadius = 200;
var prevPoint;

for (var i = 0; i < steps; i++) {
    var angle = i * calc("360 / " + steps);
    var radius = (i / steps) * maxRadius;

    var point = new PointD(
        calc("cos(" + angle + " * 3.14159 / 180) * " + radius),
        calc("sin(" + angle + " * 3.14159 / 180) * " + radius)
    );

    if (i > 0) {
        dxf.drawLine([prevPoint, point]);
    }
    prevPoint = point;
}

dxf.save("spiral.dxf");
writeln("螺旋パターンを作成しました");
```

### ファイル処理

```javascript
var dlg = new FileDialog();
dlg.filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
var filename = dlg.openDialog();

if (filename) {
    var content = FileItem.readAllText(filename, "utf-8");
    var lines = content.split("\n");

    writeln("ファイル: " + filename);
    writeln("行数: " + lines.length);
    writeln("─".repeat(50));

    for (var i = 0; i < Math.min(10, lines.length); i++) {
        writeln((i + 1) + ": " + lines[i]);
    }
}
```

### ポリゴンクリッピング

```javascript
var dxf = new Dxf();

// 2つの矩形を作成
var rect1 = Dxf.createRect(0, 0, 100, 100);
var rect2 = Dxf.createRect(50, 50, 100, 100);

// 配列に変換
var subjects = [rect1];
var clips = [rect2];

// クリッピング操作
var union = Dxf.clipping(subjects, clips, ClipOperation.Union);
var intersection = Dxf.clipping(subjects, clips, ClipOperation.Intersection);
var difference = Dxf.clipping(subjects, clips, ClipOperation.Difference);

// 結果を描画
dxf.drawPolygon(union);
dxf.save("clipping_result.dxf");
writeln("クリッピング結果を作成しました");
```

### クリップボード操作と数値変換

```javascript
// クリップボードからテキストを取得
var text = pasteText();
writeln("クリップボードの内容: " + text);

// 数値を様々な形式に変換
var num = 255;
var hex = numToHex(num, 4);     // "00FF"
var bin = numToBin(num, 8);     // "11111111"

writeln("10進数: " + num);
writeln("16進数: " + hex);
writeln("2進数: " + bin);

// 結果をクリップボードにコピー
var result = "10進数: " + num + "\n16進数: " + hex + "\n2進数: " + bin;
copyText(result);
writeln("結果をクリップボードにコピーしました");

```

### 円と円弧の描画

```javascript
var dxf = new Dxf();

// 円を描画（drawEllipseを使用）
var center1 = new PointD(0, 0);
center1.R = 50; // 半径を設定
dxf.drawEllipse(center1);

// 複数の円を一度に描画
var circles = [];
for (var i = 0; i < 5; i++) {
    var cp = new PointD(i * 60, 0);
    cp.R = 20 + (i * 5);
    circles.push(cp);
}
dxf.drawEllipse(circles);

// 円弧を描画（drawSemiCircleを使用）
var arcCenter = new PointD(0, 100);
arcCenter.R = 40; // 半径を設定
dxf.drawSemiCircle(arcCenter, 0, 90);    // 0度から90度の円弧
dxf.drawSemiCircle(arcCenter, 90, 180);  // 90度から180度の円弧

dxf.save("circles_and_arcs.dxf");
writeln("円と円弧を作成しました");
```

## ライセンス

このプロジェクトはMITライセンスの下でライセンスされています。詳細については、`LICENSE`ファイルを参照してください。

## Authors

bry-ful(Hiroshi Furuhashi)<br>
twitter:[bryful](https://twitter.com/bryful)<br>

