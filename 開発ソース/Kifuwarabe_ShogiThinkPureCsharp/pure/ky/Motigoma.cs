#if DEBUG
using kifuwarabe_shogithink.pure.com.hyoka;
#else
using kifuwarabe_shogithink.pure.com.hyoka;
#endif

namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 先後付きの持ち駒だぜ☆（＾▽＾）
    /// </summary>
    public enum Motigoma
    {
        /// <summary>
        /// ▲ぞう（角）
        /// </summary>
        Z,
        /// <summary>
        /// ▽ぞう（角）
        /// </summary>
        z,
        /// <summary>
        /// ▲きりん（飛）
        /// </summary>
        K,
        /// <summary>
        /// ▽きりん（飛）
        /// </summary>
        k,
        /// <summary>
        /// ▲ひよこ（歩）
        /// </summary>
        H,
        /// <summary>
        /// ▽ひよこ（歩）
        /// </summary>
        h,
        /// <summary>
        /// ▲いぬ（金）
        /// </summary>
        I,
        /// <summary>
        /// ▽いぬ（金）
        /// </summary>
        i,
        /// <summary>
        /// ▲ねこ（銀）
        /// </summary>
        N,
        /// <summary>
        /// ▽ねこ（銀）
        /// </summary>
        n,
        /// <summary>
        /// ▲うさぎ（桂）
        /// </summary>
        U,
        /// <summary>
        /// ▽うさぎ（桂）
        /// </summary>
        u,
        /// <summary>
        /// ▲いのしし（香）
        /// </summary>
        S,
        /// <summary>
        /// ▽いのしし（香）
        /// </summary>
        s,
        /// <summary>
        /// 先手のぞう～後手のひよこ　までの要素の個数になるぜ☆（＾▽＾）
        /// </summary>
        Yososu
    }
    public abstract class Conv_Motigoma
    {
        public static readonly Motigoma[] itiran = {
            // ぞう（対局者１、対局者２）
            Motigoma.Z,Motigoma.z,
            // きりん
            Motigoma.K,Motigoma.k,
            // ひよこ
            Motigoma.H,Motigoma.h,
            // いぬ
            Motigoma.I,Motigoma.i,
            // ねこ
            Motigoma.N,Motigoma.n,
            // うさぎ
            Motigoma.U,Motigoma.u,
            // いのしし
            Motigoma.S,Motigoma.s,
        };

        public static readonly string[] m_dfen_ = {
            // ぞう（対局者１、対局者２）
            "Z","z",
            // きりん
            "K","k",
            // ひよこ
            "H","h",
            // いぬ
            "I","i",
            // ねこ
            "N","n",
            // うさぎ
            "U","u",
            // いのしし
            "S","s",
        };
        public static readonly string[] m_sfen_ = {
            // ぞう（対局者１、対局者２）
            "B","b",
            // きりん
            "R","r",
            // ひよこ
            "P","p",
            // いぬ
            "G","g",
            // ねこ
            "S","s",
            // うさぎ
            "N","n",
            // いのしし
            "L","l",
        };

        public readonly static string[] m_setumeiMojiretu_ = {
            // ぞう（対局者１、対局者２）
            "▲ぞ","▽ぞ",
            "▲き","▽き",
            "▲ひ","▽ひ",
            "▲い","▽い",
            "▲ね","▽ね",
            "▲う","▽う",
            "▲し","▽し",
        };

        public static bool IsOk(Motigoma mk)
        {
            return Motigoma.Z <= mk && mk < Motigoma.Yososu;
        }
    }

}
