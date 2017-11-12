namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 升のことだぜ☆（＾▽＾）
    /// 盤のタテ・ヨコ幅は変わるので、定数にはできないぜ☆（＾～＾）
    /// 最初の升は 0 番な（＾～＾）
    /// </summary>
    public enum Masu
    {
    }

    /// <summary>
    /// 将棋盤の升に関する変換だぜ☆（＾▽＾）
    /// </summary>
    public abstract class Conv_Masu
    {
        public const int ERROR_SUJI = 0;
        public const int ERROR_DAN = 0;

        /// <summary>
        /// 筋番号だぜ☆（＾▽＾）盤外なら Conv_Masu.ERROR_SUJI だぜ☆（＾▽＾）
        /// 左端筋が０☆（＾～＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToSujiO0_WithoutErrorCheck(int ms)
        {
            return ms % PureSettei.banYokoHaba;
        }
        /// <summary>
        /// 筋番号だぜ☆（＾▽＾）盤外なら Conv_Masu.ERROR_SUJI だぜ☆（＾▽＾）
        /// 左端筋が１☆（＾～＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToSujiO1_WithoutErrorCheck(int ms)
        {
            return ms % PureSettei.banYokoHaba + 1;
        }
        /// <summary>
        /// 下側に自分の陣地がある視点の筋番号だぜ☆（＾▽＾）
        /// 左端筋が０☆（＾～＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToSujiO0_JibunSiten_BySujiO0(Taikyokusya tb, int sujiO0)
        {
            if (tb == Taikyokusya.T1)
            {
                return sujiO0;
            }
            return PureSettei.banYokoHaba - sujiO0 - 1;
        }
        /// <summary>
        /// 下側に自分の陣地がある視点の筋番号だぜ☆（＾▽＾）
        /// 左端筋が１☆（＾～＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToSujiO1_JibunSiten_BySujiO1(Taikyokusya tb, int sujiO1)
        {
            if (tb == Taikyokusya.T1)
            {
                return sujiO1;
            }
            return PureSettei.banYokoHaba - (sujiO1 - 1);
        }

        /// <summary>
        /// 段番号だぜ☆（＾▽＾）盤外なら Conv_Masu.ERROR_DAN だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToDanO0_WithoutErrorCheck(int ms)
        {
            return ms / PureSettei.banYokoHaba;
        }
        /// <summary>
        /// 段番号だぜ☆（＾▽＾）盤外なら Conv_Masu.ERROR_DAN だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToDanO1_WithoutErrorCheck(int ms)
        {
            return ms / PureSettei.banYokoHaba + 1;
        }
        /// <summary>
        /// 下側に自分の陣地がある視点の段番号だぜ☆（＾▽＾）
        /// 例：対局者１でも２でも、トライルールは　らいおん　が１段目に入ったときだぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToDanO1_JibunSiten(Taikyokusya tb, Masu ms)
        {
            if (tb == Taikyokusya.T1)
            {
                return ToDanO1_WithoutErrorCheck((int)ms);
            }
            return ToDanO1_WithoutErrorCheck(PureSettei.banHeimen - 1 - (int)ms);
        }
        /// <summary>
        /// 下側に自分の陣地がある視点の段番号だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToDanO0_JibunSiten_ByDanO0(Taikyokusya tb, int danO0)
        {
            if (tb == Taikyokusya.T1)
            {
                return danO0;
            }
            return PureSettei.banTateHaba - danO0 - 1;
        }
        /// <summary>
        /// 下側に自分の陣地がある視点の段番号だぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToDanO1_JibunSiten_ByDanO1(Taikyokusya tb, int danO1)
        {
            if (tb == Taikyokusya.T1)
            {
                return danO1;
            }
            return PureSettei.banTateHaba - (danO1 - 1);
        }

        /// <summary>
        /// 左上筋番号だぜ☆（＾▽＾）
        /// 4321
        /// 5xooo
        /// 6oxoo
        /// 7ooxo
        ///  ooox
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToHidariAgariSujiO0_WithoutErrorCheck(int ms)
        {
            // ゼロ・オリジンで計算
            // 盤のヨコ幅は +1 が含まれている
            int rxO1 = PureSettei.banYokoHaba - ms % PureSettei.banYokoHaba;
            int yO0 = ms / PureSettei.banYokoHaba;

            return rxO1 + yO0 - 1;
        }
        /// <summary>
        /// 左上筋番号だぜ☆（＾▽＾）
        /// 4321
        /// 5xooo
        /// 6oxoo
        /// 7ooxo
        ///  ooox
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToHidariAgariSujiO1_WithoutErrorCheck(int ms)
        {
            return ToHidariAgariSujiO0_WithoutErrorCheck(ms) + 1;
        }
        /// <summary>
        /// 左下筋番号だぜ☆（＾▽＾）
        ///  ooox
        /// 1ooxo
        /// 2oxoo
        /// 3xooo
        /// 4567
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToHidariSagariSujiO0_WithoutErrorCheck(int ms)
        {
            // ゼロ・オリジンで計算
            int xO0 = ms % PureSettei.banYokoHaba;
            int yO0 = ms / PureSettei.banYokoHaba;

            return xO0 + yO0;
        }
        /// <summary>
        /// 左下筋番号だぜ☆（＾▽＾）
        ///  ooox
        /// 1ooxo
        /// 2oxoo
        /// 3xooo
        /// 4567
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static int ToHidariSagariSujiO1_WithoutErrorCheck(int ms)
        {
            return ToHidariSagariSujiO0_WithoutErrorCheck(ms) + 1;
        }

        public static Masu ToMasu( int suji, int dan)
        {
            return (Masu)(PureSettei.banYokoHaba * (dan - 1) + (suji - 1));
        }

        /// <summary>
        /// ゲーム盤の左上隅の升番号だぜ☆（＾～＾）
        /// </summary>
        public const int A1 = 0;
        /// <summary>
        /// エラー値の意図で升の数を使うときだぜ☆（＾～＾）
        /// </summary>
        public static Masu masu_error { get { return (Masu)PureSettei.banHeimen; } }
        /// <summary>
        /// 将棋盤の盤上なら真だぜ☆（＾▽＾）
        /// 盤上になければ、駒台かエラーのどちらかだぜ☆（＾▽＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static bool IsBanjo(Masu ms)
        {
            return A1 <= ms && ms < masu_error;
        }
        /// <summary>
        /// 有効範囲の数字を入れているか、アサート用だぜ☆（＾▽＾）
        /// エラー値を含む☆（＾～＾）
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static bool IsBanjoOrError(Masu ms)
        {
            return A1 <= ms && ms <= masu_error;
        }

    }
}
