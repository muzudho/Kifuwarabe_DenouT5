#if DEBUG
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.play;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.logger;
using kifuwarabe_shogithink.pure.speak.play;
#endif

namespace kifuwarabe_shogithink.pure.speak.com
{
    public static class SpkYomisuji
    {
        public static void Setumei(FenSyurui f, IHyojiMojiretu hyoji)
        {
            bool isBelow = false;
            SasiteSeiseiAccessor.ScanBestYomisuji((int iKifu, ref bool toBreak)=>
            {
                if (isBelow)
                {
                    // 2週目以降は空白を挟むぜ☆（＾～＾）
                    hyoji.Append(" ");
                }
                else
                {
                    isBelow = true;
                }

                SpkSasite.AppendFenTo(f, PureMemory.kifu_sasiteAr[iKifu], hyoji);
            });
        }

    }
}
