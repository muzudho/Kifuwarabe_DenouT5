#if DEBUG

using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.protocol;
using System.Text.RegularExpressions;
using kifuwarabe_shogithink.pure.control;
using System;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.listen;
#else
using System;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.protocol;
using System.Text.RegularExpressions;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.listen;
#endif

namespace kifuwarabe_shogithink.fen
{
    /// <summary>
    /// FEN のパーサー一覧。
    /// </summary>
    public abstract class Itiran_FenParser
    {
        static Regex intPattern;

        #region 数字
        public static Regex GetIntPattern()
        {
            if (null == intPattern)
            {
                intPattern = new Regex(
                    @"(-?\d+)" // マイナスと数字の間には空白を入れない想定
                    , RegexOptions.Compiled
                );
            }
            return intPattern;
        }
        #endregion

        /// <summary>
        /// 9x9への拡張も考慮
        /// </summary>
        class Dfen3x4Protocol : IFenProtocol
        {
            public string fen { get { return "fen"; } }
            public string startpos { get { return "krz/1h1/1H1/ZRK"; } }
            public string komasyuruiT1 { get { return "HZKSUNIR"; } }
            public string komasyuruiT2 { get { return "hzksunir"; } }
            public string motigomaT1 { get { return "ZKHINUS"; } }
            public string motigomaT2 { get { return "zkhinus"; } }
            public string banjoT1 { get { return "R" + motigomaT1; } }
            public string banjoT2 { get { return "r" + motigomaT2; } }
            public string suji { get { return "ABCDEFGHIabcdefghi"; } }
            public string dan { get { return "123456789"; } }
            public string position { get { return "(?:(" + STARTPOS_LABEL + ")|(?:"+fen+" ([" + SPACE + banjoT1 + banjoT2 + "+/]+) " + motigomaPos + " " + tebanPos + "))"; } }
            public string motigomaPos { get { return "(["+ motigomaNasi+@"\d" + motigomaT1 + motigomaT2 + "]+)"; } }
            public string motigomaNasi { get { return "-"; } }
            public string tebanPos { get { return "(1|2)"; } }
            public string toryo { get { return "toryo"; } }
        }
        static Dfen3x4Protocol Dfen { get; set; }

        class Sfen9x9Protocol : IFenProtocol
        {
            public string fen { get { return "sfen"; } }
            public string startpos { get { return "lnsgkgsnl/1r5b1/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL"; } }
            public string komasyuruiT1 { get { return "PBRLNSGK"; } }
            public string komasyuruiT2 { get { return "pbrlnsgk"; } }
            public string motigomaT1 { get { return "BRPGSNL"; } }
            public string motigomaT2 { get { return "brpgsnl"; } }
            public string banjoT1 { get { return "K" + motigomaT1; } }
            public string banjoT2 { get { return "k" + motigomaT2; } }
            public string suji { get { return "123456789"; } }
            public string dan { get { return "ABCDEFGHIabcdefghi"; } }
            public string position { get { return "(?:(" + STARTPOS_LABEL + ")|(?:" + fen + " ([" + SPACE + banjoT1 + banjoT2 + "+/]+) " + motigomaPos + " " + tebanPos + "))"; } }
            public string motigomaPos { get { return "([" + motigomaNasi + @"\d" + motigomaT1 + motigomaT2 + "]+)"; } }
            public string motigomaNasi { get { return "-"; } }
            public string tebanPos { get { return "(b|w)"; } }
            public string toryo { get { return "resign"; } }
        }
        static Sfen9x9Protocol Sfen { get; set; }

        public static string GetStartpos(FenSyurui f) { return f == FenSyurui.sfe_n ? Sfen.startpos : Dfen.startpos; }
        public static string GetPosition(FenSyurui f) { return f == FenSyurui.sfe_n ? Sfen.position : Dfen.position; }
        public static string GetToryo(FenSyurui f) { return f==FenSyurui.sfe_n ? Sfen.toryo : Dfen.toryo; }

        public const string STARTPOS_LABEL = @"startpos";

        public const string MOTIGOMA_NASI = @"-";
        public const string TAIKYOKUSYA1 = @"b";
        /// <summary>
        /// 盤上の駒。
        /// </summary>
        const string SPACE = "123456789";

        /// <summary>
        /// 参照 : 「グループ化のみ行う括弧(?:..)」 https://www.javadrive.jp/regex/ref/index3.html
        /// 参照 : 「正規表現 最長一致と最短一致 ?について」https://ameblo.jp/blueskyame/entry-10326268249.html
        /// </summary>
        static Itiran_FenParser()
        {
            Dfen = new Dfen3x4Protocol();
            Sfen = new Sfen9x9Protocol();

            HyokatiPattern = new Regex(
                @"(-?\s*\d+)"
                , RegexOptions.Compiled
            );
        }

        public static Regex GetKyokumenPattern(FenSyurui f) {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        if (null == kyokumenPattern_sfen_)
                        {
                            kyokumenPattern_sfen_ = new Regex(
                                Sfen.position +// とりあえず　ごっそりマッチ。123～はスペース数。+は成りゴマ。
                                "(?: (moves.*))?"//棋譜
                                , RegexOptions.Compiled
                            );
                        }
                        return kyokumenPattern_sfen_;
                    }
                case FenSyurui.dfe_n:
                    {
                        if (null == kyokumenPattern_dfen_)
                        {
                            kyokumenPattern_dfen_ = new Regex(
                                Dfen.position +// とりあえず　ごっそりマッチ。123～はスペース数。+は成りゴマ。
                                "(?: (moves.*))?"//棋譜
                                , RegexOptions.Compiled
                            );
                        }
                        return kyokumenPattern_dfen_;
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
        static Regex kyokumenPattern_sfen_;
        static Regex kyokumenPattern_dfen_;

        public static Regex GetMovePattern(FenSyurui f)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        if (null == movePattern_sfen_)
                        {
                            // 3×4 を最低限実装。
                            // 9×9 へも拡張。
                            // 1文字目の ZKH は打てる持ち駒だが、ひよこのHが、筋番号のHと区別できない
                            movePattern_sfen_ = new Regex(
                                @"([" + Sfen.suji + Sfen.motigomaT1 + @"])([" + Sfen.dan + @"\*])([" + Sfen.suji + @"])([" + Sfen.dan + @"])(\+)?"
                                , RegexOptions.Compiled
                            );
                        }
                        return movePattern_sfen_;
                    }
                case FenSyurui.dfe_n:
                    {
                        if (null == movePattern_dfen_)
                        {
                            // 3×4 を最低限実装。
                            // 9×9 へも拡張。
                            // 1文字目の ZKH は打てる持ち駒だが、ひよこのHが、筋番号のHと区別できない
                            movePattern_dfen_ = new Regex(
                                @"([" + Dfen.suji + Dfen.motigomaT1 + @"])([" + Dfen.dan + @"\*])([" + Dfen.suji + @"])([" + Dfen.dan + @"])(\+)?"
                                , RegexOptions.Compiled
                            );
                        }
                        return movePattern_dfen_;
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
        static Regex movePattern_sfen_;
        static Regex movePattern_dfen_;

        /// <summary>
        /// 指し手パターン
        /// </summary>
        /// <param name="isSfen"></param>
        /// <returns></returns>
        public static Regex GetMasuMovePattern(FenSyurui f)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        if (null == masMovePattern_sfen_)
                        {
                            masMovePattern_sfen_ = new Regex(
                            "([" + Sfen.suji + Sfen.motigomaT1 + "])([" + Sfen.dan + @"\*])"
                                , RegexOptions.Compiled
                            );
                        }
                        return masMovePattern_sfen_;
                    }
                case FenSyurui.dfe_n:
                    {
                        if (null == masMovePattern_dfen_)
                        {
                            masMovePattern_dfen_ = new Regex(
                            "([" + Dfen.suji + Dfen.motigomaT1 + "])([" + Dfen.dan + @"\*])"
                                , RegexOptions.Compiled
                            );
                        }
                        return masMovePattern_dfen_;
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
        static Regex masMovePattern_sfen_;
        static Regex masMovePattern_dfen_;

        #region 升
        /// <summary>
        /// 升パターン
        /// </summary>
        /// <param name="isSfen"></param>
        /// <returns></returns>
        public static Regex GetMasuPattern(FenSyurui f)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        if (null == masPattern_sfen_)
                        {
                            masPattern_sfen_ = new Regex(
                                @"([" + Sfen.suji + @"])([" + Sfen.dan + @"])"
                                , RegexOptions.Compiled
                            );
                        }
                        return masPattern_sfen_;
                    }
                case FenSyurui.dfe_n:
                    {
                        if (null == masPattern_dfen_)
                        {
                            masPattern_dfen_ = new Regex(
                                @"([" + Dfen.suji + @"])([" + Dfen.dan + @"])"
                                , RegexOptions.Compiled
                            );
                        }
                        return masPattern_dfen_;
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
        static Regex masPattern_sfen_;
        static Regex masPattern_dfen_;
        #endregion

        ///// <summary>
        ///// FIXME:分解したい
        ///// </summary>
        ///// <param name="isSfen"></param>
        ///// <returns></returns>
        //public static Regex GetKikiCommandPattern(bool isSfen)
        //{
        //    if (isSfen)
        //    {
        //        if (null == kikiCommandPattern_sfen_)
        //        {
        //            // kiki B3 R 1  : 升と、駒の種類と、手番を指定すると、利きを表示するぜ☆（＾▽＾）
        //            kikiCommandPattern_sfen_ = new Regex(
        //                @"([" + Sfen.suji + @"])([" + Sfen.dan + @"\*])\s+([" + Sfen.motigomaT1 + Sfen.motigomaT2 + @"])\s+"+Sfen.tebanPos
        //                , RegexOptions.Compiled
        //            );
        //        }
        //        return kikiCommandPattern_sfen_;
        //    }
        //    else
        //    {
        //        if (null == kikiCommandPattern_dfen_)
        //        {
        //            // kiki B3 R 1  : 升と、駒の種類と、手番を指定すると、利きを表示するぜ☆（＾▽＾）
        //            kikiCommandPattern_dfen_ = new Regex(
        //                @"([" + Sfen.suji + @"])([" + Sfen.dan + @"\*])\s+([" + Sfen.motigomaT1 + Sfen.motigomaT2 + @"])\s+" + Sfen.tebanPos
        //                , RegexOptions.Compiled
        //            );
        //        }
        //        return kikiCommandPattern_dfen_;
        //    }
        //}
        //static Regex kikiCommandPattern_sfen_;
        //static Regex kikiCommandPattern_dfen_;

        #region 移動元升
        /// <summary>
        /// 「打ち」に対応するために、「K*」等も読み込めるようにしてあるぜ☆（＾～＾）
        /// </summary>
        /// <param name="isSfen"></param>
        /// <returns></returns>
        static Regex GetSrcMsPattern(FenSyurui f)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    if (null == srcMs_sfen_)
                    {
                        // 例「K」「K*」
                        srcMs_sfen_ = new Regex(
                            @"([" + Sfen.komasyuruiT1 + Sfen.suji + @"][" + Sfen.dan + @"\*])"//\s*
                            , RegexOptions.Compiled
                        );
                    }
                    return srcMs_sfen_;
                case FenSyurui.dfe_n:
                    {
                        if (null == srcMs_dfen_)
                        {
                            // 例「R」「R*」
                            srcMs_dfen_ = new Regex(
                                @"([" + Dfen.komasyuruiT1 + Dfen.suji + @"][" + Dfen.dan + @"\*])"//\s*
                                , RegexOptions.Compiled
                            );
                        }
                        return srcMs_dfen_;
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
        static Regex srcMs_sfen_;
        static Regex srcMs_dfen_;
        public static bool MatchSrcMs(string line, ref int caret, out Masu out_ms
#if DEBUG
            , IDebugMojiretu hyoji
#endif
            )
        {
            Match m = GetSrcMsPattern(PureSettei.fenSyurui).Match(line, caret);
            if (m.Success)
            {
                // キャレットを進めます
                Util_String.SkipMatch(line, ref caret, m);
                string ms_moji = m.Groups[1].Value;

                int caret2 = 0;
                return LisMasu.MatchMasu(ms_moji, ref caret2, out out_ms
#if DEBUG
                    , hyoji
#endif
                );
            }
            else
            {
                out_ms = Conv_Masu.masu_error;
                return false;
            }
        }
        #endregion

        #region 駒、駒種類
        static Regex GetKomaPattern(FenSyurui f)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        if (null == komasyurui_sfen_)
                        {
                            // 例「K」
                            komasyurui_sfen_ = new Regex(
                                // 「+」は成り駒
                                @"(\+?[" + Sfen.komasyuruiT1 + Sfen.komasyuruiT2 + @"])"
                                , RegexOptions.Compiled
                            );
                        }
                        return komasyurui_sfen_;
                    }
                case FenSyurui.dfe_n:
                    {
                        if (null == komasyurui_dfen_)
                        {
                            // 例「R」
                            komasyurui_dfen_ = new Regex(
                                // 「+」は成り駒
                                @"(\+?[" + Dfen.komasyuruiT1 + Dfen.komasyuruiT2 + @"])"
                                , RegexOptions.Compiled
                            );
                        }
                        return komasyurui_dfen_;
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
        static Regex komasyurui_sfen_;
        static Regex komasyurui_dfen_;
        public static bool MatchKomasyurui(string line, ref int caret, out Komasyurui out_ks)
        {
            Match m = GetKomaPattern(PureSettei.fenSyurui).Match(line, caret);
            if (m.Success)
            {
                // キャレットを進めます
                Util_String.SkipMatch(line, ref caret, m);

                // 駒種類（FIXME: 後手の表記だと誤動作になるか？）
                string ks_moji = m.Groups[1].Value;
                out_ks = Med_Parser.MojiToKomasyurui(PureSettei.fenSyurui, ks_moji);
                return true;
            }
            else
            {
                out_ks = Komasyurui.Yososu;
                return false;
            }
        }
        public static bool MatchKoma(string line, ref int caret, out Koma out_km)
        {
            Match m = GetKomaPattern(PureSettei.fenSyurui).Match(line, caret);
            if (m.Success)
            {
                // キャレットを進めます
                Util_String.SkipMatch(line, ref caret, m);

                // 駒種類（FIXME: 後手の表記だと誤動作になるか？）
                string ks_moji = m.Groups[1].Value;
                return LisKoma.Try_ParseFen(PureSettei.fenSyurui, ks_moji, out out_km);
            }
            else
            {
                out_km = Koma.Yososu;
                return false;
            }
        }
        #endregion

        #region 対局者
        static Regex GetTaikyokusyaPattern(FenSyurui f)
        {
            switch (f)
            {
                case FenSyurui.sfe_n:
                    {
                        if (null == taikyokusya_sfen_)
                        {
                            // 例「K」
                            taikyokusya_sfen_ = new Regex(
                                @"([" + Sfen.tebanPos + @"])"
                                , RegexOptions.Compiled
                            );
                        }
                        return taikyokusya_sfen_;
                    }
                case FenSyurui.dfe_n:
                    {
                        if (null == taikyokusya_dfen_)
                        {
                            // 例「R」
                            taikyokusya_dfen_ = new Regex(
                                @"([" + Dfen.tebanPos + @"])"
                                , RegexOptions.Compiled
                            );
                        }
                        return taikyokusya_dfen_;
                    }
                default:
                    throw new Exception(string.Format("未定義 {0}", f));
            }
        }
        static Regex taikyokusya_sfen_;
        static Regex taikyokusya_dfen_;
        public static bool MatchTaikyokusya(string line, ref int caret, out Taikyokusya out_tai
#if DEBUG
            , IDebugMojiretu hyoji
#endif
            )
        {
            Match m = GetTaikyokusyaPattern(PureSettei.fenSyurui).Match(line, caret);
            if (m.Success)
            {
                string tai_moji = m.Groups[1].Value;

                if (!Med_Parser.Try_MojiToTaikyokusya(PureSettei.fenSyurui, tai_moji, out out_tai))
                {
                    // パースエラーの場合（エラーにはしない）
                    return false;
                }

                // キャレットを進めます
                Util_String.SkipMatch(line, ref caret, m);
                return true;
            }
            else
            {
                out_tai = Taikyokusya.Yososu;
                return false;
            }
        }
        #endregion


        public static Regex HyokatiPattern { get; set; }
    }
}
