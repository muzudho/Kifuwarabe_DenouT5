#if DEBUG
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;

using kifuwarabe_shogithink.pure.software;
using System;
using System.Diagnostics;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using System.Collections.Generic;
    using Grayscale.Kifuwarabi.Entities.Take1Base;
#else
using System.Collections.Generic;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.control;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.software;
using System.Diagnostics;
using Grayscale.Kifuwarabi.Entities.Take1Base;
#endif

namespace kifuwarabe_shogithink.pure.genkyoku
{
    /// <summary>
    /// 局面オペレーション
    /// </summary>
    public static class GenkyokuOpe
    {
        /// <summary>
        /// 勝負なし調査☆
        /// 指し次いではいけない局面なら真だぜ☆（＾～＾）
        /// （Ａ）自分のらいおんがいない☆
        /// （Ｂ）相手のらいおんがいない☆
        /// （Ｃ）自分のらいおんがトライしている☆
        /// （Ｄ）相手のらいおんがトライしている☆
        /// </summary>
        /// <returns></returns>
        public static bool IsSyobuNasi()
        {
            Piece raionJibun = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, PureMemory.kifu_teban);
            Piece raionAite = Med_Koma.KomasyuruiAndTaikyokusyaToKoma(Komasyurui.R, PureMemory.kifu_aiteban);

            // （Ａ）自分のらいおんがいない☆
            if (PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.IsEmptyKoma(raionJibun))
            {
                return true;
            }

            // （Ｂ）相手のらいおんがいない☆
            if (PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.IsEmptyKoma(raionAite))
            {
                return true;
            }

            // （Ｃ）自分のらいおんがトライしている☆
            Bitboard bbTmp = new Bitboard();
            bbTmp.Set(BitboardsOmatome.bb_try[PureMemory.kifu_nTeban]);
            PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToSelect_Koma(raionJibun, bbTmp);
            if (!bbTmp.IsEmpty())
            {
                return true;
            }

            // （Ｄ）相手のらいおんがトライしている☆
            bbTmp.Set(BitboardsOmatome.bb_try[PureMemory.kifu_nAiteban]);
            PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.ToSelect_Koma(raionAite, bbTmp);
            if (!bbTmp.IsEmpty())
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 将棋盤の駒を適当に動かすぜ☆（＾▽＾）ｗｗｗ
        /// 主にテスト用だぜ☆（＾▽＾）
        /// </summary>
        public static bool TryFail_Mazeru( FenSyurui f
#if DEBUG
            , IDebugMojiretu reigai1
#endif
            )
        {
            int r;//ランダム値☆
            Piece tmpKm;
            MotigomaItiran motiKomaItiranImpl;//使わない

            // 盤がでかくなると時間がかかる☆（＾～＾）最大 1万回で☆（＾～＾）
            int nokori = 10000;

            // 50回もやれば混ざるだろ☆（＾▽＾）
            for (int i = 0; i < 50; i++)
            {
                int kakuritu = PureSettei.banHeimen + Conv_Motigoma.itiran.Length;//適当☆（＾～＾）
                Komasyurui tmpKs;

                // 盤上にある駒を、別の空き升、あるいは持ち駒に移動するぜ☆（＾▽＾）
                for (int iMs1 = 0; iMs1 < PureSettei.banHeimen; iMs1++)
                {
                    for (int iMs2 = 0; iMs2 < PureSettei.banHeimen; iMs2++)
                    {
                        r = PureSettei.random.Next(kakuritu);
                        if (3 == r || 4 == r || 5 == r || 6 == r)// 確率
                        {
                            // 位置交換成立☆（＾～＾）空白同士の交換とか意味ないこともするぜ☆（＾▽＾）
                            tmpKm = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs1);
                            if (3 == r || 5 == r) {
                                if(PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//混ぜる
                                    (Masu)iMs1, Conv_Koma.Hanten(PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs2)),
                                    true
#if DEBUG
                                , reigai1
#endif
                                ))
                                {
                                    return Pure.FailTrue("TryFail_Oku");
                                }
                            }
                            else {
                                if(PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//混ぜる
                                    (Masu)iMs1, PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs2),
                                    true
#if DEBUG
                                , reigai1
#endif
                                ))
                                {
                                    return Pure.FailTrue("Try_Oku");
                                }
                            }

                            if (4 == r || 5 == r) {
                                if(PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//混ぜる
                                    (Masu)iMs2, Conv_Koma.Hanten(tmpKm), true
#if DEBUG
                                , reigai1
#endif
                                ))
                                {
                                    return Pure.FailTrue("Try_Oku");
                                }
                            }
                            else {
                                if (PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//混ぜる
                                    (Masu)iMs2, tmpKm, true
#if DEBUG
                                , reigai1
#endif
                                ))
                                {
                                    return Pure.FailTrue("Try_Oku");
                                }
                            }

                            nokori--;
                        }
                        else if ((1 == r || 2 == r) && PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ExistsKomaZenbu((Masu)iMs1))
                        {
                            // 持駒交換成立☆（＾▽＾）
                            Piece km_tmp = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs1);
                            tmpKs = Med_Koma.KomaToKomasyurui(km_tmp);

                            //Taikyokusya tai_tmp = Med_Koma.KomaToTaikyokusya(km_tmp);

                            // どちらの持駒にするかはランダムで☆（＾～＾）
                            Motigoma mk = Med_Koma.KomasyuruiAndTaikyokusyaToMotiKoma(tmpKs, 1 == r ? Taikyokusya.T1 : Taikyokusya.T2);

                            switch (tmpKs)
                            {
                                case Komasyurui.Z:
                                    {
                                        PureMemory.gky_ky.motigomaItiran.Fuyasu(mk);
                                        Piece km_remove = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs1);
                                        Debug.Assert(Conv_Koma.IsOk(km_remove), "km_remove can not remove");
                                        if (PureMemory.gky_ky.shogiban.TryFail_TorinozokuKoma(
                                            (Masu)iMs1,
                                            km_remove, Conv_Masu.masu_error, true
#if DEBUG
                                            , reigai1
#endif
                                        ))
                                        {
                                            return Pure.FailTrue("TryFail_Torinozoku(4)");
                                        }
                                    }
                                    break;
                                case Komasyurui.K:
                                    {
                                        PureMemory.gky_ky.motigomaItiran.Fuyasu(mk);
                                        Piece km_remove = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs1);
                                        Debug.Assert(Conv_Koma.IsOk(km_remove), "km_remove can not remove");
                                        if (PureMemory.gky_ky.shogiban.TryFail_TorinozokuKoma(
                                            (Masu)iMs1,
                                            km_remove,
                                            Conv_Masu.masu_error, true
#if DEBUG
                                            , reigai1
#endif
                                        ))
                                        {
                                            return Pure.FailTrue("TryFail_Torinozoku(5)");
                                        }
                                    }
                                    break;
                                case Komasyurui.PH://thru
                                case Komasyurui.H:
                                    {
                                        PureMemory.gky_ky.motigomaItiran.Fuyasu(mk);
                                        Piece km_remove = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs1);
                                        Debug.Assert(Conv_Koma.IsOk(km_remove), "km_remove can not remove");
                                        if (PureMemory.gky_ky.shogiban.TryFail_TorinozokuKoma(
                                            (Masu)iMs1,
                                            km_remove,
                                            Conv_Masu.masu_error, true
#if DEBUG
                                            , reigai1
#endif
                                        ))
                                        {
                                            return Pure.FailTrue("TryFail_Torinozoku(6)");
                                        }
                                    }
                                    break;
                            }

                            nokori--;
                        }
                    }

                    // ひんぱんに、ひよこ／にわとりの入れ替えだぜ☆（＾▽＾）ｗｗｗ
                    {
                        Piece km;
                        r = PureSettei.random.Next(kakuritu);
                        if (r % 5 < 2)
                        {
                            if (PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ExistsKoma(Piece.P1, (Masu)iMs1) || PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ExistsKoma(Piece.P2, (Masu)iMs1))
                            {
                                if (0 == r) {
                                    km = Piece.PP1;
                                }
                                else {
                                    km = Piece.PP2;
                                }
                            }
                            else if (PureMemory.gky_ky.shogiban.yomiIbashoBan_yoko.IsOn(Piece.PP1, (Masu)iMs1) || PureMemory.gky_ky.shogiban.yomiIbashoBan_yoko.IsOn(Piece.PP2, (Masu)iMs1))
                            {
                                if (0 == r) {
                                    km = Piece.P1;
                                }
                                else {
                                    km = Piece.P2;
                                }
                            }
                            else
                            {
                                km = Piece.Yososu;
                            }

                            if (km!=Piece.Yososu)
                            {
                                if (PureMemory.gky_ky.shogiban.TryFail_OkuKoma(// 混ぜる
                                    (Masu)iMs1, km, true
#if DEBUG
                                    , reigai1
#endif
                                ))
                                {
                                    return Pure.FailTrue("TryFail_Oku");
                                }
                            }
                        }
                    }

                    for (int iMk2 = 0; iMk2 < Conv_Motigoma.itiran.Length; iMk2++)
                    {
                        Piece km = Piece.Yososu;
                        r = PureSettei.random.Next(kakuritu);
                        if ((1 == r || 2 == r) && PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ExistsKomaZenbu((Masu)iMs1) &&
                            PureMemory.gky_ky.motigomaItiran.yomiMotigomaItiran.HasMotigoma((Motigoma)iMk2))
                        {
                            // 持駒交換成立☆（＾▽＾）
                            switch ((Motigoma)iMk2)
                            {
                                case Motigoma.Z:

                                    if (!PureMemory.gky_ky.motigomaItiran.Try_Herasu(out motiKomaItiranImpl, Motigoma.Z
#if DEBUG
                        , reigai1
#endif
                        ))
                                    {
                                        return Pure.FailTrue("Try_Herasu");
                                    }

                                    if (1 == r) {
                                        km = Piece.B1;
                                    }
                                    else {
                                        km = Piece.B2;
                                    }
                                    break;
                                case Motigoma.K:

                                    if (!PureMemory.gky_ky.motigomaItiran.Try_Herasu(out motiKomaItiranImpl, Motigoma.K
#if DEBUG
                        , reigai1
#endif
                        ))
                                    {
                                        return Pure.FailTrue("Try_Herasu");
                                    }

                                    if (1 == r) {
                                        km = Piece.R1;
                                    }
                                    else {
                                        km = Piece.R2;
                                    }
                                    break;
                                case Motigoma.H:
                                    if (!PureMemory.gky_ky.motigomaItiran.Try_Herasu(out motiKomaItiranImpl, Motigoma.H
#if DEBUG
                        , (IDebugMojiretu)reigai1
#endif
                        ))
                                    {
                                        return Pure.FailTrue("Try_Herasu");
                                    }

                                    if (1 == r) {
                                        km = Piece.P1;
                                    }
                                    else {
                                        km = Piece.P2;
                                    }
                                    break;
                                case Motigoma.z:
                                    if (!PureMemory.gky_ky.motigomaItiran.Try_Herasu(out motiKomaItiranImpl, Motigoma.z
#if DEBUG
                        , (IDebugMojiretu)reigai1
#endif
                        ))
                                    {
                                        return Pure.FailTrue("Try_Herasu");
                                    }

                                    if (1 == r) {
                                        km = Piece.B2;
                                    }
                                    else {
                                        km = Piece.B1;
                                    }
                                    break;
                                case Motigoma.k:
                                    if (!PureMemory.gky_ky.motigomaItiran.Try_Herasu(out motiKomaItiranImpl, Motigoma.k
#if DEBUG
                        , reigai1
#endif
                        ))
                                    {
                                        return Pure.FailTrue("Try_Herasu");
                                    }

                                    if (1 == r) {
                                        km = Piece.R2;
                                    }
                                    else {
                                        km = Piece.R1;
                                    }
                                    break;
                                case Motigoma.h:
                                    if (!PureMemory.gky_ky.motigomaItiran.Try_Herasu(out motiKomaItiranImpl, Motigoma.h
#if DEBUG
                        , reigai1
#endif
                        ))
                                    {
                                        return Pure.FailTrue("Try_Herasu");
                                    }

                                    if (1 == r) {
                                        km = Piece.P2;
                                    }
                                    else {
                                        km = Piece.P1;
                                    }
                                    break;
                            }

                            if (Piece.Yososu!=km)
                            {
                                if (PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//混ぜる
                                    (Masu)iMs1, km, true
#if DEBUG
                                        , reigai1
#endif
                                        ))
                                {
                                    return Pure.FailTrue("TryFail_Oku");
                                }
                            }

                            nokori--;
                        }
                    }

                    if (nokori < 0) { break; }
                }

                // FIXME: 手番をひっくり返す機能は無いぜ☆（＾～＾）

                if (nokori < 0)
                {
                    break;
                }
            }

            // らいおんの先後を調整するぜ☆（＾▽＾）
            {
                Taikyokusya tb = Taikyokusya.T1;
                r = PureSettei.random.Next(2);
                if (0 == r)
                {
                    tb = Conv_Taikyokusya.Hanten(tb);
                }

                for (int iMs1 = 0; iMs1 < PureSettei.banHeimen; iMs1++)
                {
                    /*
                    // トライしてたら、位置を変えるぜ☆（＾▽＾）ｗｗｗ
                    if (Koma.R == this.Komas[iMs1] && Conv_Masu.IsTried(Taikyokusya.T1, (Masu)iMs1))
                    {
                        int iMs2 = iMs1 + 9;//9升足しておくか☆（＾▽＾）ｗｗｗ
                        tmpKm = this.Komas[iMs1];
                        this.Komas[iMs1] = this.Komas[iMs2];
                        this.Komas[iMs2] = tmpKm;
                    }
                    else if (Koma.r == this.Komas[iMs1] && Conv_Masu.IsTried(Taikyokusya.T2, (Masu)iMs1))
                    {
                        int iMs2 = iMs1 - 9;//9升引いておくか☆（＾▽＾）ｗｗｗ
                        tmpKm = this.Komas[iMs1];
                        this.Komas[iMs1] = this.Komas[iMs2];
                        this.Komas[iMs2] = tmpKm;
                    }
                    */

                    if (PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ExistsKoma(Piece.K1, (Masu)iMs1) || PureMemory.gky_ky.shogiban.ibashoBan_yk00.yomiIbashoBan.ExistsKoma(Piece.K2, (Masu)iMs1))
                    {
                        Piece km = Piece.Yososu;
                        if (tb == Taikyokusya.T1) {
                            km = Piece.K1;
                        }
                        else {
                            km = Piece.K2;
                        }

                        if (Piece.Yososu!=km)
                        {
                            if (PureMemory.gky_ky.shogiban.TryFail_OkuKoma(//混ぜる
                                (Masu)iMs1, km, true
#if DEBUG
                                , reigai1
#endif
                            ))
                            {
                                return Pure.FailTrue("TryFail_Oku");
                            }
                        }

                        tb = Conv_Taikyokusya.Hanten(tb);
                    }
                }
            }

            // 駒を配置したあとで使えだぜ☆（＾～＾）
            PureMemory.gky_ky.shogiban.Tukurinaosi_RemakeKiki();
            return Pure.SUCCESSFUL_FALSE;
        }

        /// <summary>
        /// らいおん　は　どこかな～☆？（＾▽＾）ｗｗｗ
        /// 使いやすいが、高速化されていないので、テストコード用だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static Masu Lookup(Piece km)
        {
            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                if (PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs) == km)
                {
                    return (Masu)iMs;
                }
            }
            return Conv_Masu.masu_error;
        }


        /// <summary>
        /// 盤上に駒を置くだけ。
        /// 
        /// クリアーしない（もうクリアーしてあるはず）。適用しない。利きを更新しない。
        /// </summary>
        public static bool TryFail_DoHirate_KomaNarabe(
            FenSyurui f
#if DEBUG
            , IDebugMojiretu reigai1
#endif
            )
        {
            bool updateKiki = false;

            // 0 ～ 初期局面升数-1
            // FIXME: 本将棋にしても、これが12のままになっている

            // 初期局面升数 ～ 将棋盤の升数-1

            for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
            {
                if (iMs < HirateShokiKyokumen.banjo.Length)
                {
                    Piece km = HirateShokiKyokumen.banjo[iMs];
                    if (Piece.Kuhaku != km)
                    {
                        if(PureMemory.gky_ky.shogiban.TryFail_OkuKoma(
                            (Masu)iMs, km, updateKiki
#if DEBUG
                            , reigai1
#endif
                            ))
                        {
                            return Pure.FailTrue("TryFail_Oku");
                        }
                    }
                }
                else
                {
                    Piece km_remove = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma((Masu)iMs);
                    Debug.Assert(Conv_Koma.IsOk(km_remove), string.Format("km_remove can not remove 初期局面升数={0} 盤上の升数={1}", HirateShokiKyokumen.banjo.Length, PureSettei.banHeimen));
                    if (PureMemory.gky_ky.shogiban.TryFail_TorinozokuKoma(
                        (Masu)iMs,
                        km_remove,
                        Conv_Masu.masu_error, updateKiki
#if DEBUG
                        , reigai1
#endif
                        ))
                    {
                        return Pure.FailTrue("TryFail_Torinozoku(8)");
                    }
                }
            }
            // ここではまだ、利きチェックは働かない
            return Pure.SUCCESSFUL_FALSE;
        }

        /// <summary>
        /// 対人表示用☆（＾～＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToSuji_WithError( Masu ms)
        {
            if (Conv_Masu.IsBanjo(ms))
            {
                return ((int)ms) % PureSettei.banYokoHaba + 1;
            }
            return Conv_Masu.ERROR_SUJI;
        }
        /// <summary>
        /// 対人表示用☆（＾～＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToDan_WithError(Masu ms)
        {
            if (Conv_Masu.IsBanjo(ms))
            {
                return ((int)ms) / PureSettei.banYokoHaba + 1;
            }
            return Conv_Masu.ERROR_DAN;
        }






        public static bool CanDoMove( Move ss, out MoveMatigaiRiyu reason)
        {
            if (Move.Toryo == ss) { reason = MoveMatigaiRiyu.Karappo; return true; }// 投了はＯＫだぜ☆（＾～＾）

            // 打つ駒調べ
            MotigomaSyurui mksUtta = AbstractConvMove.GetUttaKomasyurui(ss);// 打った駒の種類
            bool utta = MotigomaSyurui.Yososu != mksUtta;
            if (utta)
            {
                // 「打」の場合、持ち駒チェック☆
                if (!PureMemory.gky_ky.yomiKy.yomiMotigomaItiran.HasMotigoma(Med_Koma.MotiKomasyuruiAndTaikyokusyaToMotiKoma(mksUtta, PureMemory.kifu_teban)))
                {
                    // 持駒が無いのに打とうとしたぜ☆（＞＿＜）
                    reason = MoveMatigaiRiyu.NaiMotiKomaUti;
                    return false;
                }
            }

            // 移動先、打つ先　調べ☆
            Masu ms_dst = AbstractConvMove.GetDstMasu_WithoutErrorCheck((int)ss); // 移動先升
            if (!Conv_Masu.IsBanjo(ms_dst))
            {
                // 盤外に移動しようとしたぜ☆（＾～＾）
                reason = MoveMatigaiRiyu.BangaiIdo;
                return false;
            }
            Piece km_dst = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma(ms_dst);
            Taikyokusya tai_dstKm = Med_Koma.KomaToTaikyokusya(km_dst);
            if (km_dst != Piece.Kuhaku && PureMemory.kifu_teban == tai_dstKm)
            {
                // 自分の駒を取ろうとするのは、イリーガル・ムーブだぜ☆（＾▽＾）
                reason = MoveMatigaiRiyu.TebanKomaNoTokoroheIdo;
                return false;
            }
            else if (utta && km_dst != Piece.Kuhaku)
            {
                // 駒があるところに打ち込んではいけないぜ☆（＾▽＾）
                reason = MoveMatigaiRiyu.KomaGaAruTokoroheUti;
                return false;
            }


            // 移動元調べ☆
            Piece km_src;
            if (utta)
            {
                // 「打」のときは　ここ。
                km_src = Med_Koma.MotiKomasyuruiAndTaikyokusyaToKoma(mksUtta, PureMemory.kifu_teban);
            }
            else
            {
                Masu ms_src = AbstractConvMove.GetSrcMasu_WithoutErrorCheck((int)ss); // 移動先升
                km_src = PureMemory.gky_ky.yomiKy.yomiShogiban.yomiIbashoBan.GetBanjoKoma(ms_src);
                Taikyokusya tai_srcKm = Med_Koma.KomaToTaikyokusya(km_src);
                if (km_src == Piece.Kuhaku)
                {
                    // 空き升に駒があると思って動かそうとするのは、イリーガル・ムーブだぜ☆（＾▽＾）
                    reason = MoveMatigaiRiyu.KuhakuWoIdo;
                    return false;
                }
                else if (tai_srcKm != PureMemory.kifu_teban)
                {
                    // 相手の駒を動かそうとするのは、イリーガル・ムーブだぜ☆（＾▽＾）
                    reason = MoveMatigaiRiyu.AiteNoKomaIdo;
                    return false;
                }


                // 移動方向調べ
                if (!BitboardsOmatome.KomanoUgokikataYk00.IsIntersect(
                    km_src,
                    ms_src,//この２つのマスが交わっているか
                    ms_dst//この２つのマスが交わっているか
                    ))
                {
                    // その駒の種類からは、ありえない動きをしたぜ☆（＾▽＾）
//#if DEBUG
                    
//                    throw new Exception($"その駒の種類からは、ありえない動きをしたぜ☆（＾▽＾） ms1=[{ ms_src }] ms2=[{ ms_dst }]");
//#else
                    reason = MoveMatigaiRiyu.SonoKomasyuruiKarahaArienaiUgoki;
                    return false;
//#endif
                }
            }


            // 成り調べ
            if (AbstractConvMove.IsNatta(ss))//成りを指示した場合
            {
                switch (PureSettei.gameRule)
                {
                    case GameRule.DobutuShogi:
                        {
                            if (Med_Koma.KomaToKomasyurui(km_src) != Komasyurui.H)
                            {
                                // ひよこ以外が、にわとりになろうとしました☆
                                reason = MoveMatigaiRiyu.NarenaiNari;
                                return false;
                            }
                        }
                        break;
                    case GameRule.HonShogi:
                        {
                            switch (Med_Koma.KomaToKomasyurui(km_src))
                            {
                                case Komasyurui.H:
                                case Komasyurui.K:
                                case Komasyurui.N:
                                case Komasyurui.S:
                                case Komasyurui.U:
                                case Komasyurui.Z:
                                    // セーフ
                                    break;
                                case Komasyurui.I:
                                case Komasyurui.PH:
                                case Komasyurui.PK:
                                case Komasyurui.PN:
                                case Komasyurui.PS:
                                case Komasyurui.PU:
                                case Komasyurui.PZ:
                                case Komasyurui.R:
                                case Komasyurui.Yososu://FIXME:
                                default:
                                    {
                                        // 成れる駒以外が、成ろうとしました☆
                                        reason = MoveMatigaiRiyu.NarenaiNari;
                                        return false;
                                    }
                            }
                        }
                        break;
                }
            }

            reason = MoveMatigaiRiyu.Karappo;
            return true;
        }



    }
}
