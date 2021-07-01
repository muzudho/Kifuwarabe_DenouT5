using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grayscale.Kifuwarabi.Entities.Take1Base;

namespace kifuwarabe_shogithink.pure.ky.bb
{
    /// <summary>
    /// 隣利き（短い利き）
    /// </summary>
    public static class Util_Tonarikiki
    {
        public static void Tukurinaosi()
        {
            foreach (Piece km_all in Conv_Koma.itiran)
            {
                // 居場所マス
                for (int iMs = 0; iMs < PureSettei.banHeimen; iMs++)
                {
                    Masu ms = (Masu)iMs;

                    switch (Med_Koma.KomaToKomasyurui(km_all))
                    {
                        #region らいおん
                        case Komasyurui.R:
                            {
                                TasuKonoUe(km_all, ms);// 上
                                TasuKonoMigiue(km_all, ms);// 右上
                                TasuKonoMigi(km_all, ms);// 右
                                TasuKonoMigisita(km_all, ms);// 右下
                                TasuKonoSita(km_all, ms);// 下
                                TasuKonoHidarisita(km_all, ms);// 左下
                                TasuKonoHidari(km_all, ms);// 左
                                TasuKonoHidariue(km_all, ms);// 左上
                            }
                            break;
                        #endregion
                        #region ぞう
                        case Komasyurui.Z:
                            {
                                if (!PureSettei.tobikikiTukau)
                                {
                                    // 飛び利きを使わない場合
                                    TasuKonoMigiue(km_all, ms);// 右上
                                    TasuKonoMigisita(km_all, ms);// 右下
                                    TasuKonoHidarisita(km_all, ms);// 左下
                                    TasuKonoHidariue(km_all, ms);// 左上
                                }
                            }
                            break;
                        #endregion
                        #region パワーアップぞう
                        case Komasyurui.PZ:
                            {
                                if (!PureSettei.tobikikiTukau)
                                {
                                    // 飛び利きを使わない場合
                                    TasuKonoMigiue(km_all, ms);// 右上
                                    TasuKonoMigisita(km_all, ms);// 右下
                                    TasuKonoHidarisita(km_all, ms);// 左下
                                    TasuKonoHidariue(km_all, ms);// 左上
                                }
                                TasuKonoUe(km_all, ms);// 上
                                TasuKonoMigi(km_all, ms);// 右
                                TasuKonoSita(km_all, ms);// 下
                                TasuKonoHidari(km_all, ms);// 左
                            }
                            break;
                        #endregion
                        #region きりん
                        case Komasyurui.K:
                            {
                                if (!PureSettei.tobikikiTukau)
                                {
                                    // 飛び利きを使わない場合
                                    TasuKonoUe(km_all, ms);// 上
                                    TasuKonoMigi(km_all, ms);// 右
                                    TasuKonoSita(km_all, ms);// 下
                                    TasuKonoHidari(km_all, ms);// 左
                                }
                            }
                            break;
                        #endregion
                        #region パワーアップきりん
                        case Komasyurui.PK:
                            {
                                if (!PureSettei.tobikikiTukau)
                                {
                                    // 飛び利きを使わない場合
                                    TasuKonoMigiue(km_all, ms);// 右上
                                    TasuKonoMigisita(km_all, ms);// 右下
                                    TasuKonoHidarisita(km_all, ms);// 左下
                                    TasuKonoHidariue(km_all, ms);// 左上
                                }
                                TasuKonoUe(km_all, ms);// 上
                                TasuKonoMigi(km_all, ms);// 右
                                TasuKonoSita(km_all, ms);// 下
                                TasuKonoHidari(km_all, ms);// 左
                            }
                            break;
                        #endregion
                        #region ひよこ
                        case Komasyurui.H:
                            {
                                TasuKonoUe(km_all, ms);// 上
                            }
                            break;
                        #endregion
                        #region にわとり
                        case Komasyurui.PH:
                            {
                                TasuKonoUe(km_all, ms);// 上
                                TasuKonoMigiue(km_all, ms);// 右上
                                TasuKonoMigi(km_all, ms);// 右
                                TasuKonoSita(km_all, ms);// 下
                                TasuKonoHidari(km_all, ms);// 左
                                TasuKonoHidariue(km_all, ms);// 左上
                            }
                            break;
                        #endregion
                        #region いぬ
                        case Komasyurui.I:
                            {
                                TasuKonoUe(km_all, ms);// 上
                                TasuKonoMigiue(km_all, ms);// 右上
                                TasuKonoMigi(km_all, ms);// 右
                                TasuKonoSita(km_all, ms);// 下
                                TasuKonoHidari(km_all, ms);// 左
                                TasuKonoHidariue(km_all, ms);// 左上
                            }
                            break;
                        #endregion
                        #region ねこ
                        case Komasyurui.N:
                            {
                                TasuKonoUe(km_all, ms);// 上
                                TasuKonoMigiue(km_all, ms);// 右上
                                TasuKonoMigisita(km_all, ms);// 右下
                                TasuKonoHidarisita(km_all, ms);// 左下
                                TasuKonoHidariue(km_all, ms);// 左上
                            }
                            break;
                        #endregion
                        #region パワーアップねこ
                        case Komasyurui.PN:
                            {
                                TasuKonoUe(km_all, ms);// 上
                                TasuKonoMigiue(km_all, ms);// 右上
                                TasuKonoMigi(km_all, ms);// 右
                                TasuKonoSita(km_all, ms);// 下
                                TasuKonoHidari(km_all, ms);// 左
                                TasuKonoHidariue(km_all, ms);// 左上
                            }
                            break;
                        #endregion
                        #region うさぎ
                        case Komasyurui.U:
                            {
                                TasuKeimatobiMigi(km_all, ms);// 桂馬跳び右
                                TasuKeimatobiHidari(km_all, ms);// 桂馬跳び左
                            }
                            break;
                        #endregion
                        #region パワーアップうさぎ
                        case Komasyurui.PU:
                            {
                                TasuKonoUe(km_all, ms);// 上
                                TasuKonoMigiue(km_all, ms);// 右上
                                TasuKonoMigi(km_all, ms);// 右
                                TasuKonoSita(km_all, ms);// 下
                                TasuKonoHidari(km_all, ms);// 左
                                TasuKonoHidariue(km_all, ms);// 左上
                            }
                            break;
                        #endregion
                        #region いのしし
                        case Komasyurui.S:
                            {
                                if (!PureSettei.tobikikiTukau)
                                {
                                    // 飛び利きを使わない場合
                                    TasuKonoUe(km_all, ms);// 上
                                }
                            }
                            break;
                        #endregion
                        #region パワーアップいのしし
                        case Komasyurui.PS:
                            {
                                TasuKonoUe(km_all, ms);// 上
                                TasuKonoMigiue(km_all, ms);// 右上
                                TasuKonoMigi(km_all, ms);// 右
                                TasuKonoSita(km_all, ms);// 下
                                TasuKonoHidari(km_all, ms);// 左
                                TasuKonoHidariue(km_all, ms);// 左上
                            }
                            break;
                        #endregion
                        default:
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// この上
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKonoUe(Piece km, Masu ms)
        {
            switch (Med_Koma.KomaToTaikyokusya(km))
            {
                case Taikyokusya.T1:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_UeHajiDan(ms))
                    {
                        Masu ms_tmp = ms - PureSettei.banYokoHaba;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                case Taikyokusya.T2:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_SitaHajiDan(ms))
                    {
                        Masu ms_tmp = ms + PureSettei.banYokoHaba;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                default: break;
            }
        }
        /// <summary>
        /// この右上
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKonoMigiue(Piece km, Masu ms)
        {
            switch (Med_Koma.KomaToTaikyokusya(km))
            {
                case Taikyokusya.T1:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_UeHajiDan(ms) && !BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_MigiHajiSuji(ms))
                    {
                        Masu ms_tmp = ms - PureSettei.banYokoHaba + 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                case Taikyokusya.T2:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_SitaHajiDan(ms) && !BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_HidariHajiSuji(ms))
                    {
                        Masu ms_tmp = ms + PureSettei.banYokoHaba - 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                default: break;
            }
        }
        /// <summary>
        /// この右
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKonoMigi(Piece km, Masu ms)
        {
            switch (Med_Koma.KomaToTaikyokusya(km))
            {
                case Taikyokusya.T1:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_MigiHajiSuji(ms))
                    {
                        Masu ms_tmp = ms + 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                case Taikyokusya.T2:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_HidariHajiSuji(ms))
                    {
                        Masu ms_tmp = ms - 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                default: break;
            }
        }
        /// <summary>
        /// この右下
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKonoMigisita(Piece km, Masu ms)
        {
            switch (Med_Koma.KomaToTaikyokusya(km))
            {
                case Taikyokusya.T1:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_SitaHajiDan(ms) && !BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_MigiHajiSuji(ms))
                    {
                        Masu ms_tmp = ms + PureSettei.banYokoHaba + 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                case Taikyokusya.T2:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_UeHajiDan(ms) && !BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_HidariHajiSuji(ms))
                    {
                        Masu ms_tmp = ms - PureSettei.banYokoHaba - 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                default: break;
            }
        }
        /// <summary>
        /// この下
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKonoSita(Piece km, Masu ms)
        {
            switch (Med_Koma.KomaToTaikyokusya(km))
            {
                case Taikyokusya.T1:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_SitaHajiDan(ms))
                    {
                        Masu ms_tmp = ms + PureSettei.banYokoHaba;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                case Taikyokusya.T2:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_UeHajiDan(ms))
                    {
                        Masu ms_tmp = ms - PureSettei.banYokoHaba;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                default: break;
            }
        }
        /// <summary>
        /// この左下
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKonoHidarisita(Piece km, Masu ms)
        {
            switch (Med_Koma.KomaToTaikyokusya(km))
            {
                case Taikyokusya.T1:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_SitaHajiDan(ms) && !BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_HidariHajiSuji(ms))
                    {
                        Masu ms_tmp = ms + PureSettei.banYokoHaba - 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                case Taikyokusya.T2:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_UeHajiDan(ms) && !BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_MigiHajiSuji(ms))
                    {
                        Masu ms_tmp = ms - PureSettei.banYokoHaba + 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                default: break;
            }
        }
        /// <summary>
        /// この左
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKonoHidari(Piece km, Masu ms)
        {
            switch (Med_Koma.KomaToTaikyokusya(km))
            {
                case Taikyokusya.T1:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_HidariHajiSuji(ms))
                    {
                        Masu ms_tmp = ms - 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                case Taikyokusya.T2:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_MigiHajiSuji(ms))
                    {
                        Masu ms_tmp = ms + 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                default: break;
            }
        }
        /// <summary>
        /// この左上
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKonoHidariue(Piece km, Masu ms)
        {
            switch (Med_Koma.KomaToTaikyokusya(km))
            {
                case Taikyokusya.T1:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_UeHajiDan(ms) && !BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_HidariHajiSuji(ms))
                    {
                        Masu ms_tmp = ms - PureSettei.banYokoHaba - 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                case Taikyokusya.T2:
                    if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_SitaHajiDan(ms) && !BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_MigiHajiSuji(ms))
                    {
                        Masu ms_tmp = ms + PureSettei.banYokoHaba + 1;
                        if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km,ms, ms_tmp); }
                    }
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 桂馬跳び右
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKeimatobiMigi(Piece km, Masu ms)
        {
            Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km);
            if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_UsagigaMiginiToberu(tai, ms))
            {
                switch (tai)
                {
                    case Taikyokusya.T1:
                        {
                            Masu ms_tmp = ms - 2 * PureSettei.banYokoHaba + 1;
                            if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km, ms, ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        {
                            Masu ms_tmp = ms + 2 * PureSettei.banYokoHaba - 1;
                            if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km, ms, ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
        }
        /// <summary>
        /// 桂馬跳び左
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        static void TasuKeimatobiHidari(Piece km, Masu ms)
        {
            Taikyokusya tai = Med_Koma.KomaToTaikyokusya(km);
            if (!BitboardsOmatome.YomiBitboardsOmatome.IsIntersect_UsagigaHidariniToberu(tai, ms))
            {
                switch (tai)
                {
                    case Taikyokusya.T1:
                        {
                            Masu ms_tmp = ms - 2 * PureSettei.banYokoHaba - 1;
                            if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km, ms, ms_tmp); }
                        }
                        break;
                    case Taikyokusya.T2:
                        {
                            Masu ms_tmp = ms + 2 * PureSettei.banYokoHaba + 1;
                            if (Conv_Masu.IsBanjo(ms_tmp)) { BitboardsOmatome.KomanoUgokikataYk00.StandupElement(km, ms, ms_tmp); }
                        }
                        break;
                    default: break;
                }
            }
        }

    }
}
