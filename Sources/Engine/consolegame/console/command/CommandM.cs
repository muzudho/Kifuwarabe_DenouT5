#if DEBUG
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
#else
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using Grayscale.Kifuwarabi.Entities.Logging;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandM
    {
        public static void Man(IHyojiMojiretu hyoji)
        {
            // なるべく、アルファベット順☆（＾▽＾）
            hyoji.AppendLine("(空っぽEnter)   : ゲームモードのフラグを ON にするぜ☆");
            hyoji.AppendLine("@例             : 「例.txt」ファイル読込んでコマンド実行だぜ☆(UTF-8 BOM有り)");
            hyoji.AppendLine("#コメント       : なんにもしないぜ☆");
            hyoji.AppendLine("bitboard        : ビットボードのテスト用だぜ☆");
            hyoji.AppendLine("bitboard kiki   : 駒の動きを表示するぜ☆");
            hyoji.AppendLine("bitboard remake : 駒の動きを作り直すぜ☆");
#if DEBUG
            hyoji.AppendLine("bitboard usagitobi: 桂馬跳びの表示☆");
#endif
            hyoji.AppendLine("cando B4B3      : B4にある駒をB3へ動かせるなら true を返すぜ☆");
            hyoji.AppendLine("                : 動かせないなら「false, 理由」を返すぜ☆");

            hyoji.AppendLine("chikanhyo ha45  : 左肩上がり４５°の盤面表示☆");
            hyoji.AppendLine("chikanhyo hs45  : 左肩下がり４５°の盤面表示☆");
            hyoji.AppendLine("chikanhyo ht90  : 反時計回り９０°の盤面表示☆");

            hyoji.AppendLine("clear           : コンソールをクリアーするぜ☆");
            hyoji.AppendLine("do B4B3         : B4にある駒をB3へ動かしたあと ky するぜ☆");
            hyoji.AppendLine("                : アルファベットは小文字でも構わない☆");
            hyoji.AppendLine("do Z*A2         : 持ち駒の ぞう をA2へ打ったあと ky するぜ☆");
            hyoji.AppendLine("                : Z* ぞう打　K* きりん打　H* ひよこ打☆");
            hyoji.AppendLine("do C2C1+        : C2にある駒をC1へ動かしたあと成って ky するぜ☆");
            hyoji.AppendLine("do toryo        : 投了するぜ☆");
#if DEBUG
            hyoji.AppendLine("dump            : 変数をたくさん出力☆");
            hyoji.AppendLine("dump move     : 指し手生成の変数をたくさん出力☆");
#endif

            hyoji.AppendLine("fugo suji       : 筋符号一覧表☆");

            hyoji.AppendLine("go              : コンピューターが１手指したあと ky するぜ☆");
            hyoji.AppendLine("hirate          : 平手初期局面にしたあと ky するぜ☆");
            hyoji.AppendLine("honyaku fen sfen startpos : 翻訳☆ どうぶつしょうぎfenから");
            hyoji.AppendLine("                          : 本将棋fenへ変換☆ 4単語目以降を☆");
            hyoji.AppendLine("hyoka           : 現局面を（読み無しで）形成判断するぜ☆");
            hyoji.AppendLine("hyoka komawari  : 現局面を　駒割りだけで　形成判断するぜ☆");

            hyoji.AppendLine("jokyo           : 現在の内部の状況を表示☆");
            hyoji.AppendLine("kansosen        : 終わった直後の局面データを復活させるぜ☆（＾～＾）");
            hyoji.AppendLine("kifu            : 終わった直後の局面の棋譜を表示するぜ☆");
            hyoji.AppendLine("kifu 10         : 終わった直後の局面の10手目までの図を表示するぜ☆");
            hyoji.AppendLine("kiki            : 味方の駒の利きを一覧するぜ☆");
            hyoji.AppendLine("kiki atk B3     : 現局面の B3 にあるアタック駒の利きを一覧するぜ☆");
            hyoji.AppendLine("                : 旧「kiki B3」");
            hyoji.AppendLine("kiki B3 R 1     : 升と、駒の種類と、手番を指定すると、利きを表示するぜ☆");
            hyoji.AppendLine("kikisu          : 重ね利きの数を一覧するぜ☆");
            hyoji.AppendLine("koma            : 対局者１、２の駒の場所を表示☆");
            hyoji.AppendLine("koma zenbu      : 対局者１、２の駒全部盤だけを表示☆");
            hyoji.AppendLine("koma R          : 対局者１、２の　らいおん　の場所を表示☆");
            hyoji.AppendLine("koma +z         : 対局者１、２の　パワーゾウ　の場所を表示☆ 他同様☆");
            hyoji.AppendLine("ky              : 将棋盤をグラフィカル表示☆ KYokumen");
            hyoji.AppendLine("ky:             : 将棋盤を１行表示☆");
            hyoji.AppendLine("                : fen krz/1h1/1H1/ZRK - 1 0 1");
            hyoji.AppendLine("                : fen 盤上の駒配置 持ち駒の数 手番の対局者 何手目 同形反復の回数");
            hyoji.AppendLine("ky fen krz/1h1/1H1/ZRK - 1 : fen を打ち込んで局面作成☆");
            hyoji.AppendLine("ky fen          : 現局面の fen を表示☆");
            hyoji.AppendLine("ky hanten       : 将棋盤を１８０度回転☆ 反転☆");
            hyoji.AppendLine("ky kesu C4      : C4升に置いてある駒を消すぜ☆");
            hyoji.AppendLine("ky mazeru       : 将棋盤をごちゃごちゃに混ぜるぜ☆ シャッフル☆");
            hyoji.AppendLine("ky motu I 2     : 持ち駒の▲犬を２にするぜ☆");
            hyoji.AppendLine("ky oku K C3     : きりんをC3升に置くぜ☆ 最後に ky tekiyo 必要☆");
            hyoji.AppendLine("ky tekiyo       : 編集した盤面を使えるようにするぜ☆");
            hyoji.AppendLine("man,manual      : これ");
            hyoji.AppendLine("masu 3          : A1,B1升の位置を表す盤を返すぜ☆");
            hyoji.AppendLine("nanamedan atama : ナナメ段の行頭のマス番号（マス毎）☆");
            hyoji.AppendLine("nanamedan d     : ナナメ段の符号（マス毎）☆");
            hyoji.AppendLine("nanamedan haba  : ナナメ段の幅（マス毎）☆");
            hyoji.AppendLine("nanamedan nukidasi: 全マスのナナメ段のビットボード抜出し☆");
            hyoji.AppendLine("nanamedan nukidasi KT: きりん縦☆");
            hyoji.AppendLine("nanamedan nukidasi KY: きりん横☆");
            hyoji.AppendLine("nanamedan nukidasi S: いのしし☆");
            hyoji.AppendLine("nanamedan nukidasi ZHa: ぞう左上がり☆");
            hyoji.AppendLine("nanamedan nukidasi ZHs: ぞう左下がり☆");
            hyoji.AppendLine("nanamedan masu A1: A1マスに紐づくナナメ段情報表示☆");

            hyoji.AppendLine("ojama ha45         : 左肩上がり４５°の盤面表示☆");
            hyoji.AppendLine("ojama hs45         : 左肩下がり４５°の盤面表示☆");
            hyoji.AppendLine("ojama ht90         : 反時計回り９０°の盤面表示☆");

            hyoji.AppendLine("quit            : アプリケーション終了。保存してないものは保存する☆");
            hyoji.AppendLine("rnd             : ランダムに１手指すぜ☆");
            hyoji.AppendLine("move          : 味方の指し手を一覧するぜ☆");
            hyoji.AppendLine("move 1361     : 指し手コード 1361 を翻訳するぜ☆");
            hyoji.AppendLine("move seisei   : 指し手生成のテストだぜ☆");
            hyoji.AppendLine("move su       : 指し手の件数を出力するぜ☆");
            hyoji.AppendLine("set             : 設定を一覧表示するぜ☆");
            hyoji.AppendLine("set BanTateHaba 9       : 盤の縦幅☆");
            hyoji.AppendLine("set BanYokoHaba 9       : 盤の横幅☆");
            hyoji.AppendLine("set JohoJikan 3000      : 読み筋表示を 3000 ミリ秒間隔で行うぜ☆ 負数で表示なし☆");
            hyoji.AppendLine("set Learn true          : 機械学習を行うぜ☆");
            hyoji.AppendLine("set P1Char HyokatiYusen : 対局者１の指し手設定。評価値優先☆");
            hyoji.AppendLine("set P1Char SinteYusen   : 対局者１の指し手設定。新手優先☆");
            hyoji.AppendLine("set P1Char SinteNomi    : 対局者１の指し手設定。新手最優先☆");
            hyoji.AppendLine("set P1Char SyorituYusen : 対局者１の指し手設定。勝率優先☆");
            hyoji.AppendLine("set P1Char SyorituNomi  : 対局者１の指し手設定。勝率最優先☆");
            hyoji.AppendLine("set P1Char TansakuNomi  : 対局者１の指し手設定。探索のみ☆");
            hyoji.AppendLine("set P1Com true          : 対局者１をコンピューターに指させるぜ☆");
            hyoji.AppendLine("set P2Char 略           : P1Char 参照☆");
            hyoji.AppendLine("set P2Com true          : 対局者２をコンピューターに指させるぜ☆");
            hyoji.AppendLine("set P1Name きふわらべ    : 対局者１の表示名を きふわらべ に変更☆");
            hyoji.AppendLine("set P2Name きふわらべ    : 対局者２の表示名を きふわらべ に変更☆");
            hyoji.AppendLine("set RenzokuRandomRule true : 連続対局をランダムにルール変えてやる☆");
            hyoji.AppendLine("set RenzokuTaikyoku true: 強制終了するまで連続対局だぜ☆");
            hyoji.AppendLine("set SagareruHiyoko true : 下がれるひよこモード☆普通のひよこはいなくなる☆");
            hyoji.AppendLine("set SaidaiFukasa 3      : コンピューターの探索深さの最大を3に設定するぜ☆");
            hyoji.AppendLine("set SikoJikan 4000      : コンピューターが一手に思考する時間を 4秒 に設定するぜ☆");
            hyoji.AppendLine("set SikoJikanRandom 1000: 探索毎に 0～0.999秒 の範囲で思考時間を多めに取るぜ☆");
            hyoji.AppendLine("set TobikikiTukau true  : 飛び利きのＯＮ／ＯＦＦ☆");
            hyoji.AppendLine("set UseTimeOver false   : 思考時間の時間切れ判定を無視するぜ☆");
            hyoji.AppendLine("set USI false           : USI通信モードを途中でやめたくなったとき☆");
            hyoji.AppendLine("tantaitest        : 色んなテストを一気にするぜ☆");
            hyoji.AppendLine("taikyokusya       : 手番を表示するんだぜ☆");
            hyoji.AppendLine("test bit-shift    : ビットシフト の動作テスト☆");
            hyoji.AppendLine("test bit-ntz      : ビット演算 NTZ の動作テスト☆");
            hyoji.AppendLine("test bit-kiki     : ビット演算の利きの動作テスト☆");
            hyoji.AppendLine("test bitboard     : 固定ビットボードの確認☆");
            hyoji.AppendLine("test downSizing   : 定跡ファイルの内容を減らすテストだぜ☆");
            hyoji.AppendLine("test ittedume     : 一手詰めの動作テスト☆");
            hyoji.AppendLine("test jisatusyu B3 : B3升に駒を動かすのは自殺手かテスト☆");
            hyoji.AppendLine("test tryrule      : トライルールの動作テスト☆");
            hyoji.AppendLine("tu                : tumeshogi と同じだぜ☆");
            hyoji.AppendLine("tumeshogi         : 詰将棋が用意されるぜ☆");
            hyoji.AppendLine("ugokikata R 0 0 0 : 動き方。[駒][升][長い利き方向][邪魔ブロック]☆");
            hyoji.AppendLine("undo B4B3         : B3にある駒をB4へ動かしたあと ky するぜ☆");
            Logger.Flush(hyoji);
        }

        public static void Masu_cmd(string line, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "masu ");
            string line2 = line.Substring(caret).Trim();

            // masu 2468 といった数字かどうか☆（＾～＾）
            if (!LisBitboard.TryParse(line2, out Bitboard bitboard))
            {
                hyoji.AppendLine("masuコマンド解析失敗？");
            }
            else
            {
                SpkBan_1Column.Setumei_Bitboard("升の位置", bitboard, hyoji);
                //int masu;
                //while(0!= masuBango && Util_BitEnzan.PopNTZ(ref masuBango, out masu))
                //{

                //}
            }
        }


    }
}
