#if DEBUG
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;//局面データ
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;//局面データ
using kifuwarabe_shogithink.pure.move;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
#endif

namespace kifuwarabe_shogiapi
{
    /// <summary>
    /// ３４将棋（さんよんしょうぎ）
    /// 
    /// 
    /// 内容物
    /// 
    /// 
    /// 「Kifuwarabe_ShogiAPI」C#プロジェクト
    /// ─────────────
    /// 
    /// Ｕｎｉｔｙ／スマホゲーム用に設計されたＡＰＩ
    /// Shogi34クラスのメソッドを用いて「どうぶつしょうぎ」の局面を与えると、
    /// 「きふわらべ」が一手を返してくる
    /// 
    /// Ｃ＃６．０（Ｕｎｉｔｙに揃えたもの）
    /// 
    /// 
    /// 
    /// 「Kifuwarabe_ShogiThinkPureCsharp」C#プロジェクト
    /// ─────────────────────
    /// 
    /// （３４将棋でも必要）
    /// 
    /// Ｃ＃ピュアなＤＬＬ
    /// コンピュータ「どうぶつしょうぎ」エンジンの思考部
    /// 不完全ながら本将棋にも一部拡張できる
    /// 
    /// ＷＣＳＣ２７出場きふわらべ　からWindowsコンソールを
    /// 分離したもの
    /// 
    /// Ｃ＃６．０（「３４将棋」に揃えたもの）
    /// 
    /// 
    /// 
    /// 「Kifuwarabe_ShogiWinConsole」C#プロジェクト
    /// ─────────────────────
    /// 
    /// （３４将棋を作る場合には、これは使わないので捨てていい）
    /// 
    /// Windows コンソールを介して
    /// きふわらべ将棋アガルタ　ライブラリーの機能をすべて使用可能
    /// 
    /// また、将棋所にエンジン登録することで対局が可能
    /// 
    /// Ｃ＃７．０（Visual Studio 2017のデフォルト）
    /// 
    /// 
    /// 
    /// Unity での必要な設定
    /// ─────────────────────
    /// 
    /// 添付画像「Unity設定.png」参照
    /// 
    /// （１）Edit -> Project Settings -> Player -> Other Settings -> Optimization -> Api Compatibility Level => .NET 2.0
    /// 「.NET 2.0 subset」を「.NET 2.0」に変更すること
    /// 
    /// （２）すぐ下の Scripting Define Symbols* に「UNITY」と追加（既にキーワードが入っている場合は半角スペース区切り）
    /// 
    /// 参考：「unity > error CS0117: `System.Text.RegularExpressions.RegexOptions' does not contain a definition for `Compiled'」https://qiita.com/7of9/items/3334286bc0f0cb5cae13
    /// 
    /// </summary>
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //────────────────────────────────────────
            // （１）まずは初期化だぜ☆（＾～＾）
            //────────────────────────────────────────
            ShogiApi.Init();

            //────────────────────────────────────────
            // （２）設定を変えるなら、Init のあと、 SetRule のまえがそのタイミングだぜ☆（＾～＾）
            //────────────────────────────────────────

            // コンピューターの強さは、思考時間を減らすか伸ばすかで変えろだぜ☆（＾～＾）
            ShogiApi.SetOption_SikoJikan(100);//最低限使える時間（単位：ミリ秒）
            ShogiApi.SetOption_SikoJikanRandom(900);//追加でランダムに使える時間（単位：ミリ秒）

            //────────────────────────────────────────
            // （３）新規ゲーム開始時に設定しろだぜ☆（＾～＾）
            //────────────────────────────────────────
            // 基本、本将棋の設定がＭＡＸだぜ☆（＾～＾）
            ShogiApi.SetRule(
                GameRule.DobutuShogi,// とりあえず、これを設定しておく
                3, // 盤ヨコ幅（３～９）
                4, // 盤タテ幅（３～９）
                   // 駒は　先手が　「ら」「き」「ぞ」「い」「ね」「う」「し」「ひ」、成り駒は「PK」「PZ」「PN」「PU」「PS」「PH」
                   // 駒は　後手が　「ラ」「キ」「ゾ」「イ」「ネ」「ウ」「シ」「ヒ」、成り駒は「pk」「pz」「pn」「pu」「ps」「ph」
                   // 意味はそれぞれ「玉」「飛」「角」「金」「銀」「桂」「香」「歩」、成り駒は「龍」「馬」「全」「圭」「杏」「と」
                "キラゾ" +
                "　ヒ　" +
                "　ひ　" +
                "ぞらき",
                // 持ち駒は全部ゼロで開始☆（＾～＾）
                new Dictionary<Motigoma, int>()
                {
                        { Motigoma.K,0 },
                        { Motigoma.Z,0 },
                        { Motigoma.H,0 },
                        { Motigoma.k,0 },
                        { Motigoma.z,0 },
                        { Motigoma.h,0 },
                }
            );

            //────────────────────────────────────────
            // （４）あとはゲームをしろだぜ☆（＾～＾）
            //────────────────────────────────────────

            // １手を作成して……☆（＾～＾）
            // 例えば、
            // 盤の列は左から ABCDEFGHI、行は上から 123456789。
            // B3の駒をB2に移動させるなら 「do B3B2」、
            // その手を待ったするなら 「undo B3B2」を指定する。
            // 駒は
            // 「玉」「飛」「角」「金」「銀」「桂」「香」「歩」、成り駒は「龍」「馬」「全」「圭」「杏」「と」
            // に対応するのはそれぞれ、
            // 「R」 「K」 「Z」 「I」 「N」 「U」 「S」 「H」 、成り駒は「+K」「+Z」「+N」「+P」「+S」「+H」
            // 「r」 「k」 「z」 「i」 「n」 「u」 「s」 「h」 、成り駒は「+k」「+z」「+n」「+p」「+s」「+h」
            // が先手、後手に対応。（この表記は　ＷＣＳＣ２７版きふわらべの「どーぶつＦＥＮ」「dfen」のもの）
            // 先手がきりんを A2 に打つ場合は「do K*A2」
            // 先手が　ひよこ　を B1 で成る場合は末尾にプラスを付けて「do B2B1+」
            // 投了は 「toryo」
            Move move;
            ShogiApi.CreateMove("do b3a4", out move);

            // 文字列を、move データに一度変換する☆（＾～＾）

            // その手を指せるかどうかは、 CanDoMove メソッドでチェックしろだぜ☆（＾～＾）
            MoveMatigaiRiyu riyu;
            string riyuSetumei;
            if (!ShogiApi.CanDoMove(move, out riyu, out riyuSetumei))
            {
                // 指せない手だった場合は、riyu に理由が入っているし、setumei には文章で説明が入っているぜ☆（＾～＾）

                // Visual Studio 2017 の出力ビューへ出力
                Trace.WriteLine(riyuSetumei);
            }

            // 入れ直し☆（＾～＾）
            ShogiApi.CreateMove("do b3b2", out move);

            if (!ShogiApi.CanDoMove(move, out riyu, out riyuSetumei))
            {
                // Visual Studio 2017 の出力ビューへ出力
                Trace.WriteLine(riyuSetumei);
            }

            // １手を指すぜ☆（＾▽＾）
            ShogiApi.DoMove(move);

            //{
            //    IViewMojiretu sindan1 = new StringBuilder();
            //    SpkShogiban.AppendKomanoIbashoTo(PureAppli.genkyokumen.ky.shogiban.ibashoBan.sindanIB, sindan1);// 駒の居場所☆
            //    textBox1.Text = sindan1.ToContents();
            //}
            //return;

            // コンピューターに１手指させるぜ☆（＾～＾）
            string com_moveFen;
            ShogiApi.Go(out com_moveFen);

            // 指し手は PureMemory.tnsk_kohoMove に入っているし、 com_moveFen に文字列データとしても入っているぜ☆（＾～＾）

            // テスト出力
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("（１）");
                sb.AppendLine(com_moveFen);
                sb.AppendLine("（２）");
                sb.AppendLine(ShogiApi.GetKyokumen_TusinYo());
                sb.AppendLine("（３）");
                sb.AppendLine(ShogiApi.GetKyokumen_Zen1());
                sb.AppendLine("（４）先手のひよこの持ち駒の枚数");
                sb.AppendLine(ShogiApi.CountMotigoma(Motigoma.H).ToString());
                sb.AppendLine("（５）対局結果");
                sb.AppendLine(ShogiApi.TaikyokuKekka.ToString());
                sb.AppendLine("（７）盤面の駒データを配列で");
                foreach (Koma km_var in ShogiApi.GetKyokumen_Hairetu())
                {
                    sb.AppendLine(km_var.ToString());
                }
                sb.AppendLine();

                // Visual Studio 2017 の出力ビューへ出力
                Trace.WriteLine(sb.ToString());
            }

            //────────────────────────────────────────
            // （５）自分の手番でも、コンピューターに指させることができるぜ☆（＾～＾）
            //────────────────────────────────────────
            ShogiApi.Go(out com_moveFen);

            // テスト出力
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ShogiApi.GetKyokumen_TusinYo());

                // Visual Studio 2017 の出力ビューへ出力
                Trace.WriteLine(sb.ToString());
            }
        }
    }
}
