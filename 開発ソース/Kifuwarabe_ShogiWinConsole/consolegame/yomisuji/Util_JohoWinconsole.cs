#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.com.jikan;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.com;
using kifuwarabe_shogithink.pure.speak.ky_info;
using kifuwarabe_shogiwin.consolegame.machine;
using kifuwarabe_shogiwin.speak.ban;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.com.jikan;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.com;
using kifuwarabe_shogithink.pure.speak.ky_info;
using kifuwarabe_shogiwin.consolegame.machine;
#endif

namespace kifuwarabe_shogiwin.consolegame.yomisuji
{
    /// <summary>
    /// 読み筋情報表示☆（＾～＾）
    /// </summary>
    public abstract class Util_JohoWinconsole
    {
        #region 読み筋情報表示
        /// <summary>
        /// 読み筋情報
        /// </summary>
        public static Util_Joho.Dlgt_CreateJoho Dlgt_WriteYomisujiJoho = (
            Taikyokusya hyokatiNoTaikyokusya,
            Hyokati hyokaSu,
            int fukasa,
            int nekkoKaranoFukasa,
            IHyojiMojiretu hyoji
#if DEBUG
            ,string hint
#endif
            ) =>
        {
            if (PureMemory.tnsk_kaisiTaikyokusya != hyokatiNoTaikyokusya)// 探索者の反対側の局面評価値の場合☆
            {
                // 評価値の符号を逆転
                hyokaSu = new Hyokati(hyokaSu);
                hyokaSu.ToHanten();
            }

            hyoji.Append(PureSettei.fenSyurui==FenSyurui.sfe_n?"info ":"joho ");
#if DEBUG
            hyoji.Append("Debug["); hyoji.Append(hint); hyoji.Append("] ");
#endif

            //──────────
            // 思考した時間（ミリ秒）
            //──────────
            if (PureSettei.fenSyurui == FenSyurui.sfe_n)
            {
                hyoji.Append("time ");
                hyoji.Append(Face_TimeManager.timeManager.stopwatch_Tansaku.ElapsedMilliseconds.ToString());
            }
            else
            {
                hyoji.Append(string.Format("{0:0.000}byo", (float)Face_TimeManager.timeManager.stopwatch_Tansaku.ElapsedMilliseconds/1000f));
            }

            //──────────
            // 深さ
            //──────────
            hyoji.Append(PureSettei.fenSyurui == FenSyurui.sfe_n ? " depth ":" fukasa ");
            if (fukasa != int.MinValue)// fukasa に int.MinValue を指定していた場合は、「-」表記。
            {
                // 深さは 1 スタート☆
                // 根っこからの深さも 1 からスタート☆
                // 探索は　数字の大きい方から小さい方へ進むぜ☆
                // 根っこの深さ 7 の場合、最初は　深さ7　からスタート☆　深さ 1 が最後だぜ☆
                hyoji.Append((nekkoKaranoFukasa - (fukasa - 1)).ToString());
                hyoji.Append("/");
                hyoji.Append(nekkoKaranoFukasa.ToString());
            }
            else
            {
                hyoji.Append("-/");
                hyoji.Append(nekkoKaranoFukasa.ToString());
            }

            //──────────
            // 探索ノード数
            //──────────
            hyoji.Append(PureSettei.fenSyurui == FenSyurui.sfe_n ? " nodes ":" eda ");
            hyoji.Append(PureMemory.tnsk_tyakusyuEdas.ToString());

            //──────────
            // 評価値
            //──────────

            hyoji.Append(PureSettei.fenSyurui == FenSyurui.sfe_n ? " score ":" hyokati ");
            SpkHyokati.Setumei(hyokaSu, hyoji);

            if (PureSettei.fenSyurui != FenSyurui.sfe_n)
            {
                //──────────
                // 評価値の内訳等
                //──────────

#if DEBUG
                // どちらの評価値か
                hyoji.Append(string.Format(
                    "{0}({1}) ",
                    hyokatiNoTaikyokusya == Taikyokusya.T1 ? "p1" : "p2",
                    PureMemory.tnsk_kaisiTaikyokusya == hyokatiNoTaikyokusya ? "self" : "opponent"
                    ));

                // TODO: ほんとうは評価値の内訳を出したかったんだぜ☆（＾～＾）
                hyoji.Append(string.Format("utiwake<dbg_riyu={0} hosoku={1}>",
                    hyokaSu.dbg_riyu,
                    hyokaSu.dbg_riyuHosoku
                    ));
#endif
            }


            //──────────
            // 読み筋
            //──────────
            // （将棋所では一番最後に出力すること）
            hyoji.Append(PureSettei.fenSyurui == FenSyurui.sfe_n ? " pv ":" yomisuji ");
            SpkYomisuji.Setumei(PureSettei.fenSyurui, hyoji);

            hyoji.AppendLine();

            if (hyoji == PureAppli.syuturyoku1)
            {
#if DEBUG
                // 局面表示
                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji);
#endif
                /*
#if DEBUG
                if (hint!="UpAlpha" && hint!="UpAlphaRnd")
                {
                    //詰将棋のときの強力なデバッグ出力だぜ☆（＾▽＾）ｗｗｗ
                    Face_Commandline.Sasite_cmd("sasite seisei", syuturyoku);
                }
#endif
                // */
                //#if DEBUG
                //                if (hint == "Stalemate")
                //                {
                //                    // 駒包囲テストのときの強力なデバッグ出力だぜ☆（＾▽＾）ｗｗｗ

                //                    if(CommandK.TryFail_Ky("ky", ky2, hyoji))
                //                    {
                //                        Util_Machine.Flush(hyoji);
                //                        throw new Exception(hyoji.ToContents());
                //                    }

                //                    if(CommandS.TryFail_Sasite_cmd( "sasite", ky2, hyoji))
                //                    {
                //                        throw new Exception(hyoji.ToContents());
                //                    }
                //                    if(CommandS.TryFail_Sasite_cmd( "sasite seisei", ky2, hyoji))
                //                    {
                //                        throw new Exception(hyoji.ToContents());
                //                    }
                //                }
                //#endif

                // どちらも強制フラッシュだぜ☆（＾～＾）
                if (PureSettei.usi)
                {
                    Util_Machine.Flush_USI(hyoji);
                }
                else
                {
                    // 出力先がコンソールなら、すぐ表示してしまおうぜ☆（＾▽＾）
                    Util_Machine.Flush(hyoji);
                }
            }
        };
#endregion
    }
}
