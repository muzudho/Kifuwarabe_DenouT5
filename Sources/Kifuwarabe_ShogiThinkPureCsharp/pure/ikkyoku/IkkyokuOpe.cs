#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.sasiteorder;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.sasite;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.sasiteorder;
using kifuwarabe_shogithink.pure.sasite;
#endif

namespace kifuwarabe_shogithink.pure.ikkyoku
{
    public static class IkkyokuOpe
    {
        public static bool Try_Rnd(
#if DEBUG
            IDebugMojiretu dbg_reigai
#endif
            )
        {
            //グローバル変数に指し手がセットされるぜ☆（＾▽＾）
            PureMemory.SetTnskFukasa(PureMemory.FUKASA_MANUAL);
            MoveGenAccessor.DoSasitePickerBegin(SasiteType.N21_All);
            SasitePicker01.SasitePicker_01(SasiteType.N21_All, true);

            if (PureMemory.ssss_sasitelist[PureMemory.FUKASA_MANUAL].listCount < 1)
            {
                Move ss = Move.Toryo;
                SasiteType ssType = SasiteType.N00_Karappo;
                if (DoSasiteOpe.TryFail_DoSasite_All(ss, ssType
#if DEBUG
                    , PureSettei.fenSyurui
                    , dbg_reigai
                    , false
                    , "Try_Rnd(1)"
#endif
                    ))
                {
                    return false;
                }
                // 手番を進めるぜ☆（＾～＾）
                MoveGenAccessor.AddKifu(ss, ssType, PureMemory.dmv_ks_c);
//#if DEBUG
//                Util_Tansaku.Snapshot("Rndコマンド", dbg_reigai);
//#endif

            }
            else
            {
                Move ss = PureMemory.ssss_sasitelist[PureMemory.FUKASA_MANUAL].list_sasite[PureSettei.random.Next(PureMemory.ssss_sasitelist[PureMemory.FUKASA_MANUAL].listCount)];

                SasiteType ssType = SasiteType.N00_Karappo;
                if (DoSasiteOpe.TryFail_DoSasite_All( ss, ssType
#if DEBUG
                    , PureSettei.fenSyurui
                    , dbg_reigai
                    , false
                    , "Try_Rnd(2)"
#endif
                    ))
                {
                    return false;
                }
                // 手番を進めるぜ☆（＾～＾）
                MoveGenAccessor.AddKifu(ss, ssType, PureMemory.dmv_ks_c);
//#if DEBUG
//                Util_Tansaku.Snapshot("Rnd(2)", dbg_reigai);
//#endif

            }
            return true;
        }

    }
}
