#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
using kifuwarabe_shogiwin.project.speak;
using kifuwarabe_shogiwin.speak.ban;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.ky;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
using kifuwarabe_shogiwin.speak.ban;
#endif

namespace kifuwarabe_shogiwin.consolegame.console.command
{
    public static class CommandD
    {
        /// <summary>
        /// 「dosub」Doを分解したサブルーチン
        /// </summary>
        /// <param name="isSfen"></param>
        /// <param name="line"></param>
        /// <param name="ky"></param>
        /// <param name="commandMode"></param>
        /// <param name="hyoji"></param>
        /// <returns></returns>
        public static bool TryFail_DoSub(string line,
            IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("dosub", line, ref caret))
            {
                if (Util_String.MatchAndNext("daiOff", line, ref caret))
                {
                    if (!LisPlay.MatchFenSasite(PureSettei.fenSyurui, line, ref caret, out Sasite ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    SasiteSeiseiAccessor.BunkaiSasite_Dmv(ss);

                    DoSasiteOpe.TryFail_DaiOff(
                        PureMemory.dmv_ms_t0,
                        PureMemory.dmv_km_t0,
                        PureMemory.dmv_mk_t0,
                        PureMemory.dmv_ms_t1
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                // 取った駒を、駒台に置くぜ☆（＾▽＾）
                else if (Util_String.MatchAndNext("daiOn", line, ref caret))
                {
                    if (!LisPlay.MatchFenSasite(PureSettei.fenSyurui, line, ref caret, out Sasite ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    SasiteSeiseiAccessor.BunkaiSasite_Dmv(ss);

                    DoSasiteOpe.TryFail_DaiOn(
                        PureMemory.dmv_km_c,
                        PureMemory.dmv_ks_c,
                        PureMemory.dmv_mk_c
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                // 移動先に駒があれば消すぜ☆（＾～＾）
                else if (Util_String.MatchAndNext("dstOff", line, ref caret))
                {
                    if (!LisPlay.MatchFenSasite(PureSettei.fenSyurui, line, ref caret, out Sasite ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    SasiteSeiseiAccessor.BunkaiSasite_Dmv(ss);

                    DoSasiteOpe.TryFail_DstOff(
                        PureMemory.dmv_ms_t1,
                        PureMemory.dmv_km_c,
                        PureMemory.dmv_ks_c
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                // 移動先に駒を置くぜ☆（＾～＾）
                else if (Util_String.MatchAndNext("dstOn", line, ref caret))
                {
                    // dosub dstOn 移動元升 移動先升 駒

                    // 移動元升
                    Masu ms_t0;
                    if(!Itiran_FenParser.MatchSrcMs(line, ref caret, out ms_t0
#if DEBUG
                            , (IDebugMojiretu)hyoji
#endif
                        ))
                    {
                        // エラーの場合、「打ち」の可能性
                    }
                    // 移動先升
                    Masu ms_t1;
                    Itiran_FenParser.MatchSrcMs(line, ref caret, out ms_t1
#if DEBUG
                            , (IDebugMojiretu)hyoji
#endif
                        );
                    // 駒
                    Koma km_t1;
                    Itiran_FenParser.MatchKoma(line, ref caret, out km_t1);

                    DoSasiteOpe.TryFail_DstOn(
                        ms_t0, // 移動元の升
                        km_t1, // 駒
                        ms_t1 // 打ち先の升
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                else if (Util_String.MatchAndNext("srcOff", line, ref caret))
                {
                    if (!LisPlay.MatchFenSasite(PureSettei.fenSyurui, line, ref caret, out Sasite ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    SasiteSeiseiAccessor.BunkaiSasite_Dmv(ss);

                    DoSasiteOpe.TryFail_SrcOff(
                        ss,
                        PureMemory.dmv_ms_t0,
                        PureMemory.dmv_km_t1,
                        PureMemory.dmv_mk_t0,
                        PureMemory.dmv_ms_t1
#if DEBUG
                        , PureSettei.fenSyurui
                        , (IDebugMojiretu)hyoji
#endif
                        );
                }
                // 指し手の終わりに☆（＾～＾）
                else if (Util_String.MatchAndNext("addSasiteToKifu", line, ref caret))
                {
                    // 「dosub addSasiteToKifu K*E5 -」
                    // 指し手、取った駒種類
                    if (!LisPlay.MatchFenSasite(PureSettei.fenSyurui, line, ref caret, out Sasite ss))
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    Komasyurui ks_c;
                    if (Util_String.MatchAndNext("-", line, ref caret))
                    {
                        ks_c = Komasyurui.Yososu;
                    }
                    else if (LisKomasyurui.MatchKomasyurui(line, ref caret, out ks_c))
                    {
                    }
                    else
                    {
                        return Pure.FailTrue(string.Format("指し手のパースエラー [{0}]", line));
                    }

                    SasiteSeiseiAccessor.AddKifu(ss, SasiteType.N00_Karappo, PureMemory.dmv_ks_c);
//#if DEBUG
//                    Util_Tansaku.Snapshot("DoSub(1)", (IDebugMojiretu)hyoji);
//#endif
                }
                else
                {
                    return Pure.FailTrue(string.Format("コマンドのパースエラー☆（＾～＾） [{0}]", line));
                }
            }

            return Pure.SUCCESSFUL_FALSE;
        }

        public static bool TryFail_Do(FenSyurui f, string line,
            CommandMode commandMode, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("do", line, ref caret))//"do "
            {
                // 一体型☆（＾～＾）
                if (!LisPlay.MatchFenSasite(f, line, ref caret, out Sasite ss))
                {
                    return Pure.FailTrue(string.Format( "指し手のパースエラー [{0}]", line));
                }

                SasiteType ssType = SasiteType.N00_Karappo;
                if (DoSasiteOpe.TryFail_DoSasite_All(
                    ss,
                    ssType
#if DEBUG
                    , f
                    , (IDebugMojiretu)hyoji
                    , false
                    , "CommandD#Do"
#endif
                ))
                {
                    return Pure.FailTrue("TryFail_DoSasite_All");
                }
                // 手番を進めるぜ☆（＾～＾）
                SasiteSeiseiAccessor.AddKifu(ss, ssType, PureMemory.dmv_ks_c);
//#if DEBUG
//                Util_Tansaku.Snapshot("Doコマンド", (IDebugMojiretu)hyoji);
//#endif

                // FIXME: 局面表示
                switch (commandMode)
                {
                    case CommandMode.NigenYoConsoleKaihatu: SpkBan_1Column.Setumei_Kyokumen(PureMemory.kifu_endTeme, hyoji); break;
                    case CommandMode.NingenYoConsoleGame: SpkBan_1Column.Setumei_NingenGameYo(PureMemory.kifu_endTeme, hyoji); break;
                }
            }
            else
            {

            }

            return Pure.SUCCESSFUL_FALSE;
        }

#if DEBUG
        /// <summary>
        /// ダンプ
        /// </summary>
        /// <param name="line"></param>
        /// <param name="ky2"></param>
        /// <param name="hyoji"></param>
        public static bool TryFail_Dump(string line, IHyojiMojiretu hyoji)
        {
            int caret = 0;
            if (Util_String.MatchAndNext("dump", line, ref caret))
            {
                if (line.Length<=caret)
                {
                    Pure.Sc.Push("dumpコマンド");
                    bool ret = SpkDump.TryFail_Dump(hyoji);
                    Pure.Sc.Pop();
                    return ret;
                }
#if DEBUG
                else if (Util_String.MatchAndNext("sasite", line, ref caret))
                {
                    SasiteSeiseiAccessor.DumpSasiteSeisei(hyoji);
                }
#endif
            }

            return false;
        }
#endif
    }
}
