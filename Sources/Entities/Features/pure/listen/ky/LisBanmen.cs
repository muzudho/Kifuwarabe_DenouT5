#if DEBUG
using kifuwarabe_shogithink.pure.ky;

using System;
using kifuwarabe_shogithink.pure.accessor;
#else
using kifuwarabe_shogithink.pure.accessor;
using System;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ky;
#endif

namespace kifuwarabe_shogithink.pure.listen.ky
{
    public static class LisBanmen
    {
        /// <summary>
        /// 盤面の設定☆（＾～＾）
        /// 
        /// "キラゾ"
        /// "　ヒ　"
        /// "　ひ　"
        /// "ぞらき"
        /// といった書式で盤上を設定。改行は無視される。
        /// 
        /// 駒を取り除く操作はしないぜ☆（＾～＾）
        /// </summary>
        /// <param name="banmen_z1"></param>
        public static bool TryFail_SetBanjo(
            string banmen_z1
#if DEBUG
            , IDebugMojiretu reigai1
#endif
            )
        {
            banmen_z1 = banmen_z1.Replace("\r", "");//2017-11-07 USI用に追加
            banmen_z1 = banmen_z1.Replace("\n", "");

            if (banmen_z1.Length != PureSettei.banHeimen)
            {
                throw new Exception(string.Format("盤面カナ入力パースエラー 文字数=[{0}] 盤サイズ=[{1}]", banmen_z1.Length, PureSettei.banHeimen));
            }

            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                Koma km;
                string moji_z1 = banmen_z1.ToCharArray()[iMs].ToString();
                if (!LisKoma.TryParse_ZenkakuKanaNyuryoku(moji_z1, out km))
                {
                    throw new Exception(string.Format("盤面カナ入力パースエラー ms=[{0}] moji_z1=[{1}]", iMs, moji_z1));
                }

                if (Koma.Kuhaku != km)
                {
                    if (PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//AddBanjo
                        (Masu)iMs, km,
                        false//利きは更新しないでおく（駒を置くだけ）
#if DEBUG
                        , reigai1
#endif
                    ))
                    {
                        return Pure.FailTrue("TryFail_Oku");
                    }
                }
            }
            return Pure.SUCCESSFUL_FALSE;
        }

    }
}
