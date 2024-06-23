# bry
 とりあえず試行錯誤用のソリューションです。<br>
アプリとしてはClearScriptを使ったJavascript実行コンソールっぽい何かです<br>
その他のアプリで使うための細かい物をここで作ってます。永遠に改正しない予定です。

## AEdit.cs (WinForm用のAvalonEditorラッパー)
WPF用のAvalonEditorをWinFormで使うためのコントロールです。<br>

## Script.cs (ClearScript V8)
JavascriptV8エンジンを使ったサンプル。<br>
コンソールではなくTextBoxに出力します。<br>
実行時例外を受け取ってそれを表示させます。

## DockPanelSuite
 DockPanelSuiteを使ってドッキングパネルを実装するサンプル。
ググればサンプルがいっぱいあって使い方も簡単。
でも、.netframework専用で.NET6には対応していないので注意。<br>

はまりやすい罠は最近のバージョンだと<b>dockPanel.Theme</b>を設定しておかないと実行時エラーになる。テーマはNuGetで別にインストールしておかないといけない。

```
dockPanel1.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
```

## PrefFile
System.Text.Jsomを使った簡単な設定ファイルクラス。よく使うのでここでバージョン管理。


## Dependency
Visual studio 2022 C#

## License
This software is released under the MIT License, see LICENSE

## Authors

bry-ful(Hiroshi Furuhashi)
twitter:[bryful](https://twitter.com/bryful)
Mail: bryful@gmail.com

