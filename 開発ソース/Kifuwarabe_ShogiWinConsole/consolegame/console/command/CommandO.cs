#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.ky.bb;
using kifuwarabe_shogiwin.listen;
using System.Text;
#else
using kifuwarabe_shogithink.pure.speak.ky.bb;
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.ky.tobikiki;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogiwin.listen;
using kifuwarabe_shogiwin.speak.ban;
using System.Text;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandO
    {
        /// <summary>
        /// 升から「お邪魔駒配置ハッシュキー」盤面表示
        /// </summary>
        /// <param name="header"></param>
        /// <param name="kikiDir"></param>
        /// <param name="ms"></param>
        /// <param name="ky2"></param>
        /// <param name="hyoji"></param>
        /// <param name="dbg_hisigata"></param>
        static void HyojiOjamaHsh(
            string header,
            TobikikiDirection kikiDir,
            Masu ms,
            IHyojiMojiretu hyoji
#if DEBUG
            , bool dbg_hisigata
#endif
            )
        {
            hyoji.Append(string.Format("({0}{1}) ", header, (int)ms));
            Bitboard bb = Util_Tobikiki.GetOjhsh(kikiDir,ms);

            {
#if DEBUG
                // 指定「利きの方向」
                hyoji.Append(string.Format("kikiDir={0} ",
                    kikiDir
                    ));
                hyoji.Append(string.Format("msSpan={0} sakasa_forZHs={1} ",
                    PureMemory.ssssDbg_masuSpan, PureMemory.ssssDbg_sakasa_forZHs
                    ));
                hyoji.Append(string.Format("d={0} atama(dst={1} ms={2}) siriMs={3} ",
                    PureMemory.ssssDbg_nanamedanD,
                    PureMemory.ssssDbg_atama_reverseRotateChikanhyo,
                    PureMemory.ssssDbg_atama_noRotateMotohyo,
                    PureMemory.ssssDbg_siri_noRotateMotohyo
                    ));

#if DEBUG
                if (dbg_hisigata)
                {
                    hyoji.AppendLine("ojama ");
                    SpkBan_Hisigata.ScanAndHyojiHisigata((Masu ms2) =>
                    {
                        return PureMemory.ssssDbg_bb_ojamaTai.IsOn(ms2) ? "〇" : "・";
                    },
                    kikiDir, hyoji);
                }
                else
                {
#endif
                    // 元の盤
                    hyoji.Append(string.Format("ojama(Big={0}, Small={1}) ",
                        LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_ojamaTai.value64127),
                        LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_ojamaTai.value063)
                        ));
#if DEBUG
                }
#endif

                // 右シフト
                hyoji.Append(string.Format("rsft(val={0} Big={1}, Small={2}) ",
                    PureMemory.ssssDbg_rightShift,
                    LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_rightShifted.value64127),
                    LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_rightShifted.value063)
                    ));
                // マスク
                hyoji.Append(string.Format("haba={0} msk(Big={1} Small={2}) ",
                    PureMemory.ssssDbg_haba,
                    LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_mask.value64127),
                    LisHyoji.ToNisinsu(PureMemory.ssssDbg_bb_mask.value063)
                    ));
#endif
                hyoji.Append(string.Format("bb(Big={0} Small={1}) ",
                    LisHyoji.ToNisinsu(bb.value64127),
                    LisHyoji.ToNisinsu(bb.value063)
                    ));
            }


            StringBuilder sb2 = new StringBuilder();
            for (int iMs2 = 0; iMs2 < PureSettei.banHeimen; iMs2++)
            {
                sb2.Append(bb.IsOn((Masu)iMs2) ? "〇" : " ");
            }
            hyoji.AppendLine(sb2.ToString().TrimEnd().Replace(" ", "・"));
        }
        /// <summary>
        /// 升から「お邪魔駒配置ハッシュキー」盤面表示
        /// 全マス
        /// </summary>
        /// <param name="header"></param>
        /// <param name="kikiDir"></param>
        /// <param name="ky2"></param>
        /// <param name="hyoji"></param>
        /// <param name="dbg_hisigata"></param>
        static void HyojiOjamaHsh(
            string header,
            TobikikiDirection kikiDir,
            IHyojiMojiretu hyoji
#if DEBUG
            , bool dbg_hisigata
#endif
            )
        {
            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                HyojiOjamaHsh(
                    header,
                    kikiDir,
                    (Masu)iMs,
                    hyoji
#if DEBUG
                        , dbg_hisigata
#endif
                        );
            }
        }

        /// <summary>
        /// お邪魔駒の配置をビットボードで表示するもの
        /// </summary>
        /// <param name="line"></param>
        /// <param name="hyoji"></param>
        public static bool TryFail_Ojama(string line, IHyojiMojiretu hyoji)
        {
            // うしろに続く文字は☆（＾▽＾）
            int caret = 0;
            Util_String.TobasuTangoToMatubiKuhaku(line, ref caret, "ojama ");

            // 左肩上がり    の置換表
            if (Util_String.MatchAndNext("ha45", line, ref caret))
            {
                // hidarikata agari 45 の略
#if DEBUG
                SpkBan_Hisigata.Setumei( TobikikiDirection.ZHa, hyoji);
#else
                SpkBan_1Column.Setumei_Kyokumen( PureMemory.kifu_endTeme, OjamaBanSyurui.Ha45, hyoji);
#endif
                return Pure.SUCCESSFUL_FALSE;
            }
            // お邪魔ハッシュキーの表示
            else if (Util_String.MatchAndNext("ojhsh", line, ref caret))
            {
                if (Util_String.MatchAndNext("KT", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("きりん縦", TobikikiDirection.KT, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("きりん縦", TobikikiDirection.KT, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }
                else if (Util_String.MatchAndNext("KY", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("きりん横", TobikikiDirection.KY, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("きりん横", TobikikiDirection.KY, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }
                else if (Util_String.MatchAndNext("S", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("いのしし縦", TobikikiDirection.S, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("いのしし縦", TobikikiDirection.S, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }
                else if (Util_String.MatchAndNext("ZHa", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("ぞう左上がり", TobikikiDirection.ZHa, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("ぞう左上がり", TobikikiDirection.ZHa, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }
                else if (Util_String.MatchAndNext("ZHs", line, ref caret))
                {
                    Masu ms;
                    if (Itiran_FenParser.MatchSrcMs(line, ref caret, out ms
#if DEBUG
                    , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // 升で絞り込み
                        HyojiOjamaHsh("ぞう左下がり", TobikikiDirection.ZHs, ms, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                    else
                    {
                        // 全マス
                        HyojiOjamaHsh("ぞう左下がり", TobikikiDirection.ZHs, hyoji
#if DEBUG
                            , false
#endif
                        );
                    }
                }

                return Pure.SUCCESSFUL_FALSE;
            }
            // 左肩下がり    の置換表
            else if (Util_String.MatchAndNext("hs45", line, ref caret))
            {
                // hidarikata sagari 45 の略
#if DEBUG
                SpkBan_Hisigata.Setumei( TobikikiDirection.ZHs, hyoji);
#else
                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, OjamaBanSyurui.Hs45, hyoji);
#endif
                return Pure.SUCCESSFUL_FALSE;
            }
            // ９０°回転    の置換表
            else if (Util_String.MatchAndNext("ht90", line, ref caret))
            {
                // han tokei mawari 90 の略
#if DEBUG
                SpkBan_Hisigata.Setumei( TobikikiDirection.KT, hyoji);
#else
                SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, OjamaBanSyurui.Ht90, hyoji);
#endif
                return Pure.SUCCESSFUL_FALSE;
            }
            // 横型
            else if (Util_String.MatchAndNext("yk00", line, ref caret))
            {
                // yoko 90 の略
#if DEBUG
                SpkBan_Hisigata.Setumei( TobikikiDirection.KY, hyoji);
#else
                //SpkBan_1Column.Setumei_Kyokumen(gky.yomiGky, OjamaBanSyurui.None, hyoji);
#endif
                return Pure.SUCCESSFUL_FALSE;
            }

            return Pure.SUCCESSFUL_FALSE;
        }
    }
}
