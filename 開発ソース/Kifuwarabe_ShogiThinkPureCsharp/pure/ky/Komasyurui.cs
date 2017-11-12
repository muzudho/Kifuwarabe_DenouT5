namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 先後を付けない、盤上の駒だぜ☆（＾▽＾）
    /// </summary>
    public enum Komasyurui
    {
        /// <summary>
        /// らいおん
        /// </summary>
        R,

        /// <summary>
        /// ぞう
        /// </summary>
        Z,

        /// <summary>
        /// パワーアップぞう
        /// </summary>
        PZ,

        /// <summary>
        /// きりん
        /// </summary>
        K,

        /// <summary>
        /// パワーアップきりん
        /// </summary>
        PK,

        /// <summary>
        /// ひよこ
        /// </summary>
        H,

        /// <summary>
        /// にわとり
        /// </summary>
        PH,

        /// <summary>
        /// いぬ
        /// </summary>
        I,

        /// <summary>
        /// ねこ
        /// </summary>
        N,

        /// <summary>
        /// パワーアップねこ
        /// </summary>
        PN,

        /// <summary>
        /// うさぎ
        /// </summary>
        U,

        /// <summary>
        /// パワーアップうさぎ
        /// </summary>
        PU,

        /// <summary>
        /// いのしし
        /// </summary>
        S,

        /// <summary>
        /// パワーアップいのしし
        /// </summary>
        PS,

        /// <summary>
        /// らいおん～にわとり　までの要素の個数になるぜ☆（＾▽＾）
        /// どの駒の種類にも当てはまらない場合に、Yososu と書くことがある☆（＾▽＾）ｗｗｗ
        /// </summary>
        Yososu
    }


    public abstract class Conv_Komasyurui
    {
        public static readonly Komasyurui[] itiran = {
            Komasyurui.R,// らいおん
            Komasyurui.Z,// ぞう
            Komasyurui.PZ,
            Komasyurui.K,// きりん
            Komasyurui.PK,
            Komasyurui.H,// ひよこ
            Komasyurui.PH,// にわとり
            Komasyurui.I,
            Komasyurui.N,
            Komasyurui.PN,
            Komasyurui.U,
            Komasyurui.PU,
            Komasyurui.S,
            Komasyurui.PS
        };
        ///// <summary>
        ///// 飛び利きを持つ駒の種類
        ///// </summary>
        //public static readonly Komasyurui[] ItiranTobikiki = {
        //    // ぞう、パワーアップぞう
        //    Komasyurui.Z, Komasyurui.PZ,
        //    // きりん、パワーアップきりん
        //    Komasyurui.K, Komasyurui.PK,
        //    // いのしし
        //    Komasyurui.S
        //};
        ///// <summary>
        ///// 指し手生成のオーダリング用
        ///// </summary>
        //public static readonly Komasyurui[] ItiranYowaimonoJun = {
        //    Komasyurui.H,
        //    Komasyurui.PH,
        //    Komasyurui.S,
        //    Komasyurui.PS,
        //    Komasyurui.U,
        //    Komasyurui.PU,
        //    Komasyurui.Neko,
        //    Komasyurui.PNeko,
        //    Komasyurui.I,
        //    Komasyurui.Z,
        //    Komasyurui.PZ,
        //    Komasyurui.K,
        //    Komasyurui.PK,
        //    Komasyurui.R,
        //};
        public static readonly Komasyurui[] itiranToNari = {
            // らいおん
            Komasyurui.R,
            // ぞう、パワーアップぞう
            Komasyurui.PZ, Komasyurui.PZ,
            // きりん、パワーアップきりん
            Komasyurui.PK, Komasyurui.PK,
            // ひよこ、にわとり
            Komasyurui.PH, Komasyurui.PH,
            // いぬ
            Komasyurui.I,
            // ねこ、パワーアップねこ
            Komasyurui.PN, Komasyurui.PN,
            // うさぎ、パワーアップうさぎ
            Komasyurui.PU, Komasyurui.PU,
            // いのしし、パワーアップいのしし
            Komasyurui.PS, Komasyurui.PS,
            // 要素数
            Komasyurui.Yososu
        };
        public static Komasyurui ToNariCase(Komasyurui ks) { return itiranToNari[(int)ks]; }
        public static readonly Komasyurui[] itiranToNarazu = {
            // らいおん
            Komasyurui.R,
            // ぞう、パワーアップぞう
            Komasyurui.Z, Komasyurui.Z,
            // きりん、パワーアップきりん
            Komasyurui.K, Komasyurui.K,
            // ひよこ、にわとり
            Komasyurui.H, Komasyurui.H,
            // いぬ
            Komasyurui.I,
            // ねこ、パワーアップねこ
            Komasyurui.N, Komasyurui.N,
            // うさぎ、パワーアップうさぎ
            Komasyurui.U, Komasyurui.U,
            // いのしし、パワーアップいのしし
            Komasyurui.S, Komasyurui.S,
            // 要素数
            Komasyurui.Yososu
        };
        public static Komasyurui ToNarazuCase(Komasyurui ks) { return itiranToNarazu[(int)ks]; }

        public readonly static string[] m_ningenyoMijikaiFugo_ = {
            "ら",
            "ぞ",
            "+Z",
            "き",
            "+K",
            "ひ",
            "に",
            "い",
            "ね",
            "+N",
            "う",
            "+U",
            "し",
            "+S",
            "×",
        };

        public readonly static string[] m_sfen_ = {
            "K",
            "B",
            "+B",
            "R",
            "+R",
            "P",
            "+P",
            "G",
            "S",
            "+S",
            "N",
            "+N",
            "L",
            "+L",
            "×",
        };
        /// <summary>
        /// 改造Fen
        /// </summary>
        public readonly static string[] m_dfen_ = {
            "R",
            "Z",
            "+Z",
            "K",
            "+K",
            "H",
            "+H",
            "I",
            "N",
            "+N",
            "U",
            "+U",
            "S",
            "+S",
            "×",
        };
    }

}
