#if DEBUG
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using System.Diagnostics;
using kifuwarabe_shogithink.pure.accessor;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using System.Diagnostics;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif

namespace kifuwarabe_shogithink.pure.genkyoku
{
    /// <summary>
    /// 局面に加え、一局の中での局面としての情報を付加したものだぜ☆（＾～＾）
    /// 
    /// 棋譜データは持ってないぜ☆（＾～＾）
    /// </summary>
    public static class Genkyoku
    {
        #region 状態
        /// <summary>
        /// 決着
        /// </summary>
        /// <returns></returns>
        public static bool IsKettyaku()
        {
            return PureMemory.gky_kekka != TaikyokuKekka.Karappo;
        }
        /// <summary>
        /// 局面（駒の配置）の一致判定だぜ☆（＾▽＾）
        /// 
        /// 重い処理がある☆ 探索で使うような内容じゃないぜ☆（＾～＾）開発中用だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="motiKomas1"></param>
        /// <returns></returns>
        public static bool Equals_ForDevelop(Shogiban shogiban_hikaku, int[] motiKomas1)
        {
            Debug.Assert(PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.GetArrayLength() == motiKomas1.Length, "局面の一致判定");

            // 盤上の一致判定
            for (int iTai = 0; iTai < Conv_Taikyokusya.itiran.Length; iTai++)
            {
                Taikyokusya tai = (Taikyokusya)iTai;

                if (!PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.Equals_KomaZenbu_ForDevelop(tai, shogiban_hikaku.ibashoBan_yk00.yomiIbashoBan)) { return false; }

                for (int iKs = 0; iKs < Conv_Komasyurui.itiran.Length; iKs++)
                {
                    Komasyurui ks = Conv_Komasyurui.itiran[iKs];
                    Piece km = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, tai);

                    if (!PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.Equals_Koma_ForDevelop(km, shogiban_hikaku.ibashoBan_yk00.yomiIbashoBan)) { return false; }
                }
            }

            // 持ち駒の一致判定
            for (int iMk = 0; iMk < Conv_Motigoma.itiran.Length; iMk++)
            {
                if (PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.Count((Motigoma)iMk) != motiKomas1[iMk])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
