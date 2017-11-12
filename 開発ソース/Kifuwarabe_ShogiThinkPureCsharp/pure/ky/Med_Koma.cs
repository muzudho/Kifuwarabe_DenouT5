using kifuwarabe_shogithink.pure.ky.tobikiki;

namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// mediator.
    /// </summary>
    public abstract class Med_Koma
    {
        static Med_Koma()
        {
            komasyuruiNamaeItiran = new string[Conv_Taikyokusya.itiran.Length][];
            for (int iTai=0; iTai<Conv_Taikyokusya.itiran.Length;iTai++)
            {
                komasyuruiNamaeItiran[iTai] = new string[Conv_Komasyurui.itiran.Length];
                int iKs = 0;
                foreach (Koma km_tai in Conv_Koma.itiranTai[(int)iTai])
                {
                    komasyuruiNamaeItiran[iTai][iKs] = Conv_Koma.GetName(km_tai);
                    iKs++;
                }
            }
        }

        #region 駒種類の名前
        /// <summary>
        /// 目視確認用の文字列を返すぜ☆（＾▽＾）
        /// [対局者,駒種類]
        /// </summary>
        static string[][] komasyuruiNamaeItiran;
        public static string[] GetKomasyuruiNamaeItiran(Taikyokusya tai)
        {
            return komasyuruiNamaeItiran[(int)tai];
        }
        public static string GetKomasyuruiNamae(Taikyokusya tai, Komasyurui ks)
        {
            return komasyuruiNamaeItiran[(int)tai][(int)ks];
        }
        #endregion


        #region 駒→駒種類
        static Komasyurui[] m_KomaToKomasyurui_ = {
            // らいおん（対局者１、対局者２）
            Komasyurui.R,
            Komasyurui.R,

            // ぞう
            Komasyurui.Z,
            Komasyurui.Z,

            // パワーアップぞう
            Komasyurui.PZ,
            Komasyurui.PZ,

            // きりん
            Komasyurui.K,
            Komasyurui.K,

            // パワーアップきりん
            Komasyurui.PK,
            Komasyurui.PK,

            // ひよこ
            Komasyurui.H,
            Komasyurui.H,

            // にわとり
            Komasyurui.PH,
            Komasyurui.PH,

            // いぬ
            Komasyurui.I,
            Komasyurui.I,

            // ねこ
            Komasyurui.N,
            Komasyurui.N,

            // パワーアップねこ
            Komasyurui.PN,
            Komasyurui.PN,

            // うさぎ
            Komasyurui.U,
            Komasyurui.U,

            // パワーアップうさぎ
            Komasyurui.PU,
            Komasyurui.PU,

            // いのしし
            Komasyurui.S,
            Komasyurui.S,

            // パワーアップいのしし
            Komasyurui.PS,
            Komasyurui.PS,

            Komasyurui.Yososu,// 駒のない升だぜ☆（＾▽＾）
            Komasyurui.Yososu// 要素数だが、空白升、該当無し、としても使うぜ☆（＾▽＾）
        };
        public static Komasyurui KomaToKomasyurui(Koma km) { return Med_Koma.m_KomaToKomasyurui_[(int)km]; }
        #endregion

        #region 駒→手番
        static Taikyokusya[] m_KomaToTaikyokusya_ = {
            // らいおん（対局者１、対局者２）
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ぞう
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップぞう
            Taikyokusya.T1,
            Taikyokusya.T2,

            // きりん
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップきりん
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ひよこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // にわとり
            Taikyokusya.T1,
            Taikyokusya.T2,

            // いぬ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ねこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップねこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // うさぎ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップうさぎ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // いのしし
            Taikyokusya.T1,
            Taikyokusya.T2,

            // パワーアップいのしし
            Taikyokusya.T1,
            Taikyokusya.T2,

            Taikyokusya.Yososu,//駒のない升だぜ☆（＾▽＾）
            Taikyokusya.Yososu// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        };
        public static Taikyokusya KomaToTaikyokusya(Koma km) { return m_KomaToTaikyokusya_[(int)km]; }
        #endregion

        #region 駒→飛び利き種類
        static TobikikiSyurui[] m_TobikikiSyurui_ = {
            // らいおん（対局者１、対局者２）
            TobikikiSyurui.None,
            TobikikiSyurui.None,

            // ぞう
            TobikikiSyurui.Zou,
            TobikikiSyurui.Zou,

            // パワーアップぞう
            TobikikiSyurui.Zou,
            TobikikiSyurui.Zou,

            // きりん
            TobikikiSyurui.Kirin,
            TobikikiSyurui.Kirin,

            // パワーアップきりん
            TobikikiSyurui.Kirin,
            TobikikiSyurui.Kirin,

            // ひよこ
            TobikikiSyurui.None,
            TobikikiSyurui.None,

            // にわとり
            TobikikiSyurui.None,
            TobikikiSyurui.None,

            // いぬ
            TobikikiSyurui.None,
            TobikikiSyurui.None,

            // ねこ
            TobikikiSyurui.None,
            TobikikiSyurui.None,

            // パワーアップねこ
            TobikikiSyurui.None,
            TobikikiSyurui.None,

            // うさぎ
            TobikikiSyurui.None,
            TobikikiSyurui.None,

            // パワーアップうさぎ
            TobikikiSyurui.None,
            TobikikiSyurui.None,

            // いのしし
            TobikikiSyurui.Inosisi,
            TobikikiSyurui.Inosisi,

            // パワーアップいのしし
            TobikikiSyurui.Inosisi,
            TobikikiSyurui.Inosisi,

            TobikikiSyurui.None,//駒のない升だぜ☆（＾▽＾）
            TobikikiSyurui.None,// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        };
        public static TobikikiSyurui KomaToTobikikiSyurui(Koma km) { return m_TobikikiSyurui_[(int)km]; }
        #endregion


        #region 盤上の駒→持駒
        /// <summary>
        /// 盤上の駒を、駒台の駒（相手の方に行く）に変換するぜ☆（＾▽＾）
        /// [駒]
        /// </summary>
        static Motigoma[] m_BanjoKomaToMotiKoma_ = {
            // らいおん（対局者１、対局者２）
            Motigoma.Yososu,
            Motigoma.Yososu,

            // ぞう
            Motigoma.z,
            Motigoma.Z,

            // パワーアップぞう
            Motigoma.z,
            Motigoma.Z,

            // きりん
            Motigoma.k,
            Motigoma.K,

            // パワーアップきりん
            Motigoma.k,
            Motigoma.K,

            // ひよこ
            Motigoma.h,
            Motigoma.H,

            // にわとり
            Motigoma.h,
            Motigoma.H,

            // いぬ
            Motigoma.i,
            Motigoma.I,

            // ねこ
            Motigoma.n,
            Motigoma.N,

            // パワーアップねこ
            Motigoma.n,
            Motigoma.N,

            // うさぎ
            Motigoma.u,
            Motigoma.U,

            // パワーアップうさぎ
            Motigoma.u,
            Motigoma.U,

            // いのしし
            Motigoma.s,
            Motigoma.S,

            // パワーアップいのしし
            Motigoma.s,
            Motigoma.S,

            Motigoma.Yososu,//駒のない升だぜ☆（＾▽＾）
            Motigoma.Yososu// 空白～後手のにわとり　までの要素の個数になるぜ☆（＾▽＾）
        };
        public static Motigoma BanjoKomaToMotiKoma(Koma km) { return Med_Koma.m_BanjoKomaToMotiKoma_[(int)km]; }
        #endregion

        #region 駒種類と手番→持駒
        /// <summary>
        /// 指し手の駒の種類を、駒台の駒に変換するぜ☆（＾▽＾）
        /// [駒種類][手番]
        /// </summary>
        static Motigoma[,] m_KomasyuruiAndTaikyokusyaToMotiKoma_ = {
            // らいおん打
            { Motigoma.Yososu, Motigoma.Yososu },

            // ぞう打
            { Motigoma.Z, Motigoma.z },

            // パワーアップぞう打
            { Motigoma.Z, Motigoma.z },

            // きりん打
            { Motigoma.K, Motigoma.k },

            // パワーアップきりん打
            { Motigoma.K, Motigoma.k },

            // ひよこ打
            { Motigoma.H, Motigoma.h },

            // にわとり打　→　持駒ひよこ　（成らずとして判定する必要あり）
            { Motigoma.H, Motigoma.h },//(2017-05-02 22:10 Modify){ MotiKoma.H, MotiKoma.H },

            // いぬ打
            { Motigoma.I, Motigoma.i },

            // ねこ打
            { Motigoma.N, Motigoma.n },

            // パワーアップねこ打
            { Motigoma.N, Motigoma.n },

            // うさぎ打
            { Motigoma.U, Motigoma.u },

            // パワーアップうさぎ打
            { Motigoma.U, Motigoma.u },

            // いのしし打
            { Motigoma.S, Motigoma.s },

            // パワーアップいのしし打
            { Motigoma.S, Motigoma.s },

            // らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            { Motigoma.Yososu, Motigoma.Yososu },
        };
        public static Motigoma KomasyuruiAndTaikyokusyaToMotiKoma(Komasyurui ks, Taikyokusya tai) { return Med_Koma.m_KomasyuruiAndTaikyokusyaToMotiKoma_[(int)ks,(int)tai]; }
        #endregion

        #region 駒種類と手番→駒
        static Koma[,] m_KomasyuruiAndTaikyokusyaToKoma_ =
        {
            { Koma.R, Koma.r },// らいおん
            { Koma.Z, Koma.z },// ぞう
            { Koma.PZ, Koma.pz },// パワーアップぞう
            { Koma.K, Koma.k },// きりん
            { Koma.PK, Koma.pk },// パワーアップきりん
            { Koma.H, Koma.h },// ひよこ
            { Koma.PH, Koma.ph },// にわとり
            { Koma.I, Koma.i },// いぬ
            { Koma.N, Koma.n },// ねこ
            { Koma.PN, Koma.pn },// パワーアップねこ
            { Koma.U, Koma.u },// うさぎ
            { Koma.PU, Koma.pu },// パワーアップうさぎ
            { Koma.S, Koma.s },// いのしし
            { Koma.PS, Koma.ps },// パワーアップいのしし
            { Koma.Kuhaku, Koma.Kuhaku },// らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
        };
        public static Koma KomasyuruiAndTaikyokusyaToKoma(Komasyurui ks, Taikyokusya tb) {
            return m_KomasyuruiAndTaikyokusyaToKoma_[(int)ks, (int)tb];
        }
        public static Koma ToRaion(Taikyokusya tb)
        {
            return m_KomasyuruiAndTaikyokusyaToKoma_[(int)Komasyurui.R, (int)tb];
        }
    #endregion

    #region 駒種類→持駒種類
    static MotigomaSyurui[] m_KomasyuruiToMotiKomasyurui_ =
        {
            // らいおん
            MotigomaSyurui.Yososu,

            // ぞう
            MotigomaSyurui.Z,

            // パワーアップぞう
            MotigomaSyurui.Z,

            // きりん
            MotigomaSyurui.K,

            // パワーアップきりん
            MotigomaSyurui.K,

            // ひよこ
            MotigomaSyurui.H,

            // にわとり
            MotigomaSyurui.H,

            // いぬ
            MotigomaSyurui.I,

            // ねこ
            MotigomaSyurui.Neko,

            // パワーアップねこ
            MotigomaSyurui.Neko,

            // うさぎ
            MotigomaSyurui.U,

            // パワーアップうさぎ
            MotigomaSyurui.U,

            // いのしし
            MotigomaSyurui.S,

            // パワーアップいのしし
            MotigomaSyurui.S,

            // らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            MotigomaSyurui.Yososu,
        };
        public static MotigomaSyurui KomasyuruiToMotiKomasyrui(Komasyurui ks)
        {
            return Med_Koma.m_KomasyuruiToMotiKomasyurui_[(int)ks];
        }
        #endregion

        #region 持駒種類→駒種類
        static Komasyurui[] m_MotiKomasyuruiToKomasyurui_ =
        {
            // ぞう
            Komasyurui.Z,

            // きりん
            Komasyurui.K,

            // ひよこ
            Komasyurui.H,

            // いぬ
            Komasyurui.I,

            // ねこ
            Komasyurui.N,

            // うさぎ
            Komasyurui.U,

            // いのしし
            Komasyurui.S,

            // らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            Komasyurui.Yososu,
        };
        public static Komasyurui MotiKomasyuruiToKomasyrui(MotigomaSyurui mks)
        {
            return Med_Koma.m_MotiKomasyuruiToKomasyurui_[(int)mks];
        }
        #endregion

        #region 持駒→駒
        public static Koma MotiKomaToKoma(Motigoma mk)
        {
            return MotiKomasyuruiAndTaikyokusyaToKoma(MotiKomaToMotiKomasyrui(mk), MotiKomaToTaikyokusya(mk));
        }
        #endregion

        #region 持駒→駒種類
        static Komasyurui[] m_MotiKomaToKomasyurui_ =
        {
            // ぞう（対局者１、対局者２）
            Komasyurui.Z,
            Komasyurui.Z,

            // きりん
            Komasyurui.K,
            Komasyurui.K,

            // ひよこ
            Komasyurui.H,
            Komasyurui.H,

            // いぬ
            Komasyurui.I,
            Komasyurui.I,

            // ねこ
            Komasyurui.N,
            Komasyurui.N,

            // うさぎ
            Komasyurui.U,
            Komasyurui.U,

            // いのしし
            Komasyurui.S,
            Komasyurui.S,

            // 要素の個数、または　どの駒の種類にも当てはまらない場合☆（＾▽＾）ｗｗｗ
            Komasyurui.Yososu,
        };
        public static Komasyurui MotiKomaToKomasyrui(Motigoma mk)
        {
            return Med_Koma.m_MotiKomaToKomasyurui_[(int)mk];
        }
        #endregion

        #region 持駒→持駒種類
        static MotigomaSyurui[] m_MotiKomaToMotiKomasyurui_ =
        {
            // ぞう（対局者１、対局者２）
            MotigomaSyurui.Z,
            MotigomaSyurui.Z,

            // きりん
            MotigomaSyurui.K,
            MotigomaSyurui.K,

            // ひよこ
            MotigomaSyurui.H,
            MotigomaSyurui.H,

            // いぬ
            MotigomaSyurui.I,
            MotigomaSyurui.I,

            // ねこ
            MotigomaSyurui.Neko,
            MotigomaSyurui.Neko,

            // うさぎ
            MotigomaSyurui.U,
            MotigomaSyurui.U,

            // いのしし
            MotigomaSyurui.S,
            MotigomaSyurui.S,

            // 要素の個数、または　どの駒の種類にも当てはまらない場合☆（＾▽＾）ｗｗｗ
            MotigomaSyurui.Yososu,
        };
        public static MotigomaSyurui MotiKomaToMotiKomasyrui(Motigoma mk)
        {
            return Med_Koma.m_MotiKomaToMotiKomasyurui_[(int)mk];
        }
        #endregion

        #region 持駒→手番
        static Taikyokusya[] m_MotiKomaToTaikyokusya_ =
        {
            // ぞう（対局者１、対局者２）
            Taikyokusya.T1,
            Taikyokusya.T2,

            // きりん
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ひよこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // いぬ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // ねこ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // うさぎ
            Taikyokusya.T1,
            Taikyokusya.T2,

            // いのしし
            Taikyokusya.T1,
            Taikyokusya.T2,

            // 要素の個数、または　どの駒の種類にも当てはまらない場合☆（＾▽＾）ｗｗｗ
            Taikyokusya.Yososu,
        };
        public static Taikyokusya MotiKomaToTaikyokusya(Motigoma mk)
        {
            return Med_Koma.m_MotiKomaToTaikyokusya_[(int)mk];
        }
        #endregion

        #region 持駒種類と手番→持駒
        static Motigoma[,] m_MotiKomasyuruiAndTaikyokusyaToMotiKoma_ =
        {
            // ぞう
            { Motigoma.Z, Motigoma.z },

            // きりん
            { Motigoma.K, Motigoma.k },

            // ひよこ
            { Motigoma.H, Motigoma.h },

            // いぬ
            { Motigoma.I, Motigoma.i },

            // ねこ
            { Motigoma.N, Motigoma.n },

            // うさぎ
            { Motigoma.U, Motigoma.u },

            // いのしし
            { Motigoma.S, Motigoma.s },

            // 要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            { Motigoma.Yososu, Motigoma.Yososu },
        };
        public static Motigoma MotiKomasyuruiAndTaikyokusyaToMotiKoma(MotigomaSyurui mks, Taikyokusya tai)
        {
            return m_MotiKomasyuruiAndTaikyokusyaToMotiKoma_[(int)mks, (int)tai];
        }
        #endregion

        #region 持駒種類と手番→駒
        static Koma[,] m_MotiKomasyuruiAndTaikyokusyaToKoma_ =
        {
            // ぞう
            { Koma.Z, Koma.z },

            // きりん
            { Koma.K, Koma.k },

            // ひよこ
            { Koma.H, Koma.h },// にわとり　にはならないぜ☆（＾～＾）

            // いぬ
            { Koma.I, Koma.i },

            // ねこ
            { Koma.N, Koma.n },

            // うさぎ
            { Koma.U, Koma.u },

            // いのしし
            { Koma.S, Koma.s },

            // 要素の個数になるぜ☆（＾▽＾）
            // どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
            { Koma.Yososu, Koma.Yososu },
        };
        public static Koma MotiKomasyuruiAndTaikyokusyaToKoma(MotigomaSyurui mks, Taikyokusya tb)
        {
            return Med_Koma.m_MotiKomasyuruiAndTaikyokusyaToKoma_[(int)mks, (int)tb];
        }
        #endregion

    }
}
