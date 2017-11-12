# リードミー #

Unityでライブラリとして使う場合の設定方法の説明など☆

### What is this repository for? ###

* Quick summary
* Version
* [Learn Markdown](https://bitbucket.org/tutorials/markdowndemo)

### 設定方法 ###

* ソースコードを Unity 用に切り替える方法。  
Unityのメニューから [File] - [Build Settings] - [Player Settings] - [Other Settings] - [Scripting Define Symbols] に「UNITY」と入力。  
  
* 定跡ファイル  
空っぽでも可。PC版ではテキストファイルに記述、Unity版ではソースコード内に埋め込み。書式は後述。  
-PC  
--外部ファイル _auto_joseki_Honshogi(P1).txt や _auto_joseki_SagareruHiyoko(P1).txt を読書きする。ファイルは分割されている。  
-Unity  
--[facade/Face_Joseki.cs]ファイル - [GetKumikomiJoseki()]関数 に、外部ファイル の内容を埋め込む。  
  
* 成績ファイル  
空っぽでも可。PC版ではテキストファイルに記述、Unity版ではソースコード内に埋め込み。書式は後述。  
-PC  
--外部ファイル _auto_seiseki_Honshogi(P1).txt や _auto_seiseki_SagareruHiyoko(P1).txt を読書きする。ファイルは分割されている。  
-Unity  
--[facade/Face_Seiseki.cs]ファイル - [GetKumikomiSeiseki()]関数 に、外部ファイル の内容を埋め込む。  
  
* 二駒関係評価値ファイル  
空っぽでも可。PC版ではテキストファイルに記述、Unity版ではソースコード内に埋め込み。書式は後述。  
-PC  
--外部ファイル _auto_nikoma_Honshogi.txt や _auto_nikoma_SagareruHiyoko.txt を読書きする。ファイルは分割されている。  
-Unity  
--[facade/Face_Nikoma.cs]ファイル - [GetKumikomiNikoma()]関数 に、外部ファイル の内容を埋め込む。  
  
### サンプル・プログラム ###
shogi34/Program.cs shogi34.Program.Main コンソール・アプリケーション・プログラム  
大きく分けて７つの手順で回している。  
-（手順１）アプリケーション開始前設定  
-（手順２）ユーザー入力  
-（手順３）人間の手番  
-（手順４）コンピューターの手番  
-（手順５）決着時  
-（手順６）ゲーム用の指し手以外のコマンドライン実行  
-（手順７）保存して終了  
  
**設定  
-設定の仕方  
--アプリケーション開始前  
---Face_Application.Optionlist.項目名 = 値;  
--アプリケーション開始後  
---Face_Application.Set( "set 項目名 値" );  
  
-コンピューターの思考関連  
--set P1Com true                        : プレイヤー１をコンピューターに指させるぜ☆  
--set P2Com true                        : プレイヤー２をコンピューターに指させるぜ☆  
--set SaidaiFukasa 3                    : コンピューターの探索深さの最大を3に設定するぜ☆  
--set SikoJikan 4000                    : コンピューターが一手に思考する時間を 4秒 に設定するぜ☆  
--set SikoJikanRandom 1000              : 探索毎に 0～0.999秒 の範囲で思考時間を多めに取るぜ☆  
--set SennititeKaihi true               : コンピューターが千日手を必ず回避するぜ☆  
-内部的な設定  
--set P1Char HyokatiYusen               : （推奨）プレイヤー１の指し手設定。評価値優先☆  
--set P1Char SinteYusen                 :         プレイヤー１の指し手設定。新手優先☆  
--set P1Char SinteNomi                  :         プレイヤー１の指し手設定。新手最優先☆  
--set P1Char SyorituYusen               :         プレイヤー１の指し手設定。勝率優先☆  
--set P1Char SyorituNomi                :         プレイヤー１の指し手設定。勝率最優先☆  
--set P1Char TansakuNomi                :         プレイヤー１の指し手設定。探索のみ☆  
--set P2Char 略                         : P1Char 参照☆  
--set AspirationFukasa 7                : アスピレーション窓探索を使い始める深さ☆（＾～＾）  
--set AspirationWindow 300              : アスピレーション窓探索で使う数字☆（＾～＾）  
--set BetaCutPer 100                    : 100%の確率でベータ・カットを使うぜ☆ 0～100  
--set HanpukuSinkaTansakuTukau true     : 反復深化探索を使うぜ☆トランスポジション・テーブルを使う必要あり☆  
--set JosekiPer 50                      : 50%の確率で定跡を使うぜ☆ 0～100  
--set JosekiRec true                    : 定跡の登録を行うぜ☆  
--set Learn true                        : 機械学習を行うぜ☆  
--set NikomaHyokaKeisu 0.2              : 二駒関係評価値を 0.2 倍にするぜ☆  
--set NikomaGakusyuKeisu 0.01           : 二駒関係評価値学習の調整量を 0.01 倍☆  
--set RandomNikoma true                 : 指し手にランダム性を付けるぜ☆  
--set TranspositionTableTukau true      : トランスポジション・テーブル使うぜ☆  
--set UseTimeOver false                 : 思考時間の時間切れ判定を無視するぜ☆  
-コンソール用  
--set P1Name きふわらべ                 : プレイヤー１の表示名を きふわらべ に変更☆  
--set P2Name きふわらべ                 : プレイヤー２の表示名を きふわらべ に変更☆  
--set JohoJikan 3000                    : 読み筋表示を 3000 ミリ秒間隔で行うぜ☆ 負数で表示なし☆  
-ゲームルール関連  
--set SagareruHiyoko true               : 下がれるひよこモード☆普通のひよこはいなくなる☆  
-連続対局設定  
--set RandomCharacter true              : 対局終了時に、COMの指し手の性格を変えるぜ☆  
--set RandomStart true                  : 開始局面をランダムにするぜ☆  
--set RandomStartTaikyokusya true       : 開始先後をランダムにするぜ☆  
--set RenzokuRandomRule true            : 連続対局をランダムにルール変えてやる☆  
--set RenzokuTaikyoku true              : 強制終了するまで連続対局だぜ☆  
* 依存関係
* データベース
  
* コマンドライン  
  
-アプリケーションを終了するコマンド  
-quit  
  
-コマンド一覧を見るコマンド  
--man  
  
-現在の設定を確認するコマンド  
--set  
  
-ゲームモード  
--※何もコマンドを打たず[Enter]キーを打鍵。対局終了までゲームモード。  
--　コンピューターが指してくる。  
  
-指し手  
--do B3B2  
--※B3の駒をB2に動かす。  
--do C2C1+  
--※C2の駒をC2に動かして、成る。  
--do H*A2  
--※持駒のひよこをA2に打つ。  
--undo B3B2  
--※B2の駒をB3に戻す。最後の do を戻す用途のみ有効。  
  
-局面  
--hirate  
--※平手初期局面に初期化  
--ky hanten  
--※盤を１８０度回転  
--hyoka  
--※読み無しで、現局面の評価値を返す。  
  
-定跡作成  
--set JosekiPer 40  
--set SikoJikan 4000  
--set SaidaiFukasa 100  
--set P1Com true  
--set RenzokuTaikyoku true  
--※あとは自動で定跡の作成を開始する。  
--※SikoJikan（思考時間） はミリ秒単位。  
  
-COM vs COM 観戦  
--set P1Com true  
--set RenzokuTaikyoku true  
  
-MAN vs MAN 対局  
--set P2Com false  
  
-定跡の自動生成を止めたい場合  
--set JosekiRec false  
--※または joseki.txt ファイルを、フォルダーの中から避けておく。  
  
-定跡の使用確率  
--set JosekiPer 100  
--※これで 100% 使用  
--set JosekiPer 0  
--※これで 不使用  
--※または joseki.txt ファイルを、フォルダーの中から避けておくことで不使用。  
  
-その他、動作確認用のコマンドがいくつかある。  
  
* 配布手順  
  
### 定数 ###
## 対局者 ##
interfaces/Taikyokusya.cs shogi34.interfaces.Taikyokusya 対局者  
T1 対局者１  
T2 対局者２  
  
## 対局結果 ##
interfaces/TaikyokuKekka.cs shogi34.interfaces.TaikyokuKekka 対局結果  
Karappo 未設定  
Taikyokusya1NoKati 対局者１の勝ち  
Taikyokusya2NoKati 対局者２の勝ち  
Hikiwake 引き分け  
Sennitite 引き分け（千日手）  
  
## 駒 ##
interfaces/Koma.cs shogi34.interfaces.Koma 先後付きの盤上の駒  
R 対局者１のらいおん  
Z 対局者１のぞう  
K 対局者１のきりん  
H 対局者１のひよこ  
N 対局者１のにわとり  
r 対局者２のらいおん  
z 対局者２のぞう  
k 対局者２のきりん  
h 対局者２のひよこ  
n 対局者２のにわとり  
Kuhaku 空白升  
  
interfaces/Koma.cs shogi34.interfaces.MotiKoma 持ち駒  
Z 対局者１のぞう  
K 対局者１のきりん  
H 対局者１のひよこ  
z 対局者２のぞう  
k 対局者２のきりん  
h 対局者２のひよこ  
  
## 駒種類 ##
interfaces/Komasyurui.cs shogi34.interfaces.Komasyurui 先後を付けない盤上の駒  
R らいおん  
Z ぞう  
K きりん  
H ひよこ  
N にわとり  
  
interfaces/Komasyurui.cs shogi34.interfaces.MotiKomasyurui 先後を付けない持ち駒
Z ぞう  
K きりん  
H ひよこ  
  
## 升 ##
interfaces/Masu.cs shogi34.interfaces.Masu  
> [A1] [B1] [C1]  
> [A2] [B2] [C2]  
> [A3] [B3] [C3]  
> [A4] [B4] [C4]  
  
## 指し手 ##
interfaces/Sasite.cs shogi34.interfaces.Sasite 指し手  
0 投了  

## 指し手間違い理由 ##
interfaces/Sasite.cs shogi34.interfaces.SasiteMatigaiRiyu 指し手間違い理由  
Karappo  エラーなし  
ParameterSyosikiMatigai  パラメーターの書式が間違っているdoコマンド  
NaiMotiKomaUti  持駒が無いのに駒を打った☆  
BangaiIdo  盤外に移動しようとしたぜ☆（＾～＾）  
TebanKomaNoTokoroheIdo  自分の駒が置いてあるところに、駒を動かしたぜ☆  
KomaGaAruTokoroheUti  駒が置いてあるところに、駒を打ち込んだぜ☆  
KuhakuWoIdo  空き升に駒が置いてあると思って、動かそうとしたぜ☆  
AiteNoKomaIdo  相手の駒を動かそうとするのは、イリーガル・ムーブだぜ☆（＾▽＾）  
NarenaiNari  ひよこ以外が、にわとりになろうとしました☆  
SonoKomasyuruiKarahaArienaiUgoki  その駒の種類からは、ありえない動きをしました。  
  
### データ ###
## 局面 ##
interfaces/Kyokumen.cs shogi34.interfaces.Kyokumen  
Option_Application.Kyokumen 局面  
Option_Application.Kyokumen.Kekka 対局結果  
Option_Application.Kyokumen.Teban 手番
Option_Application.Kyokumen.Teme 何手目か☆
Option_Application.Kyokumen.BanjoKomas 盤上の駒[0]～[11]
> [ 0] [ 1] [ 2]  
> [ 3] [ 4] [ 5]  
> [ 6] [ 7] [ 8]  
> [ 9] [10] [11]  
Option_Application.Kyokumen.MotiKomas 持ち駒の数[R][Z][H][r][z][h]
  
### 操作 ###
## 局面 ##
interfaces/Kyokumen.cs shogi34.interfaces.Kyokumen  


### ファイル・フォーマット ###
## 定跡ファイル・フォーマット ##
*例  
fen krz/1h1/1H1/ZRK - 1  
B4A3 none 19996 9 123  
B4C3 none 970 13 124  
B3B2 none 747 13 123  
C4C3 none 78 13 124  
fen k1z/1hr/RH1/Z1K - 1  
C4C3 none 19997 9 121  
C4B4 none 19997 9 123  
A3B4 none 187 9 121  
  
*構成  
局面文字列  
指し手 none 評価値 深さ バージョン  

## 成績ファイル・フォーマット ##
*例  
fen k1z/1h1/RHr/ZK1 - 1  
B4C4 none 123 0 0 7  
fen k1z/1hr/RH1/Z1K - 1  
C4B4 none 123 0 0 6  
fen krz/1h1/1H1/ZRK - 1  
B4A3 none 123 0 0 6  
B4C3 none 121 7 0 0  
C4C3 none 122 1 0 0  
  
*構成
局面文字列
指し手 none バージョン 勝ち数 引き分け数 負け数

## 二駒関係評価値ファイル・フォーマット ##
*例（一部抜粋）
> xxxxxxxxxxxxx     0.00000000     0.00000000     0.00000000  
> -------------  xxxxxxxxxxxxx     0.00000000     0.00000000  
> -------------  -------------  xxxxxxxxxxxxx     0.00000000  
> -------------  -------------  -------------  xxxxxxxxxxxxx  
> -------------  -------------  -------------  -------------  
  
縦横に132×132のテーブル。（盤上１２０、駒台１２）  
▲ら×１２　▲ぞ×１２　▲き×１２　▲ひ×１２　▲に×１２  
△ら×１２　△ぞ×１２　△き×１２　△ひ×１２　△に×１２  
▲ぞ１　▲ぞ２　▲き１　▲き２　▲ひ１　▲ひ２  
△ぞ１　△ぞ２　△き１　△き２　△ひ１　△ひ２  
