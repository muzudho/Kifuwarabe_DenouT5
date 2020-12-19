#if DEBUG
using kifuwarabe_shogithink.pure.ky.bb;
using System;
using kifuwarabe_shogithink.pure.logger;
#else
using kifuwarabe_shogithink.pure.ky.bb;
using System;
#endif

namespace kifuwarabe_shogithink.pure.ky
{
    public static class KyokumenOperation
    {
        /// <summary>
        /// 持ち駒の枚数を数えます。
        /// </summary>
        /// <param name="moti"></param>
        /// <returns></returns>
        public static int CountMaisu(string moti)
        {
            if (moti == "")
            {
                return 0;
            }
            else if (moti.Length == 1)
            {
                // 「Z」など１文字を想定。
                return 1;
            }

            // とりあえず複数桁の持ち駒に対応☆
            int result;
            if (int.TryParse(moti.Substring(0, moti.Length - 1), out result))
            {
                return result;
            }

            throw new Exception("持ち駒の枚数のパース・エラー☆");
        }

        /// <summary>
        /// 将棋盤を１８０度ひっくり返すぜ☆（＾▽＾）
        /// 主にテスト用だぜ☆（＾▽＾）
        /// 
        /// 参考:「ビットの並びを反転する」http://blog.livedoor.jp/techblog1/archives/5365383.html
        /// </summary>
        public static void Hanten()
        {
            // 盤上
            {
                // 左右反転して、先後も入替
                Bitboard tmp_T1 = PureMemory.gky_ky.shogiban.ibashoBan_yk00.CloneBB_KomaZenbu(Taikyokusya.T1);
                Bitboard tmp_T2 = PureMemory.gky_ky.shogiban.ibashoBan_yk00.CloneBB_KomaZenbu(Taikyokusya.T2);
                tmp_T1.Bitflip128();
                tmp_T2.Bitflip128();
                PureMemory.gky_ky.shogiban.ibashoBan_yk00.Set_KomaZenbu(Taikyokusya.T1, tmp_T1);
                PureMemory.gky_ky.shogiban.ibashoBan_yk00.Set_KomaZenbu(Taikyokusya.T2, tmp_T2);

                for (int iKs = 0; iKs < Conv_Komasyurui.itiran.Length; iKs++)
                {
                    Komasyurui ks = Conv_Komasyurui.itiran[iKs];

                    Koma km1 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, Taikyokusya.T1);
                    tmp_T1 = PureMemory.gky_ky.shogiban.ibashoBan_yk00.CloneBb_Koma(km1);
                    tmp_T1.Bitflip128();
                    PureMemory.gky_ky.shogiban.ibashoBan_yk00.Set_Koma(km1, tmp_T1);

                    Koma km2 = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(ks, Taikyokusya.T2);
                    tmp_T2 = PureMemory.gky_ky.shogiban.ibashoBan_yk00.CloneBb_Koma(km2);
                    tmp_T2.Bitflip128();
                    PureMemory.gky_ky.shogiban.ibashoBan_yk00.Set_Koma(km2, tmp_T2);
                }
                // 盤面反転、駒の先後も反転だぜ☆（＾▽＾）
            }
            // 持ち駒
            {
                MotigomaItiran tmp = new MotigomaItiran();
                foreach (Motigoma mk in Conv_Motigoma.itiran)
                {
                    MotigomaSyurui mks = Med_Koma.MotiKomaToMotiKomasyrui(mk);
                    Taikyokusya tai = Med_Koma.MotiKomaToTaikyokusya(mk);
                    Motigoma hantenMotikoma = Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(mks, Conv_Taikyokusya.Hanten(tai));
                    tmp.Set(mk, PureMemory.gky_ky.motigomaItiran.yomiMotigomaItiran.Count(hantenMotikoma));
                }
                PureMemory.gky_ky.motigomaItiran.Set(tmp);
            }
        }


    }
}
