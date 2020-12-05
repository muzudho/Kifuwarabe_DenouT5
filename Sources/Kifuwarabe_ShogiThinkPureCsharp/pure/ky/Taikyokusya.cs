namespace kifuwarabe_shogithink.pure.ky
{
    /// <summary>
    /// 対局者☆
    /// いわゆる先後☆（＾▽＾）
    /// 
    /// （＾～＾）（１）「手番」「相手番」、（２）「対局者１」「対局者２」、（３）「或る対局者」「その反対の対局者」を
    /// 使い分けたいときがあるんだぜ☆
    /// </summary>
    public enum Taikyokusya
    {
        /// <summary>
        /// 対局者１
        /// </summary>
        T1,

        /// <summary>
        /// 対局者２
        /// </summary>
        T2,

        /// <summary>
        /// 要素の個数、または該当無しに使っていいぜ☆（＾▽＾）
        /// </summary>
        Yososu
    }

    public abstract class Conv_Taikyokusya
    {
        /// <summary>
        /// 対局者一覧
        /// </summary>
        public static readonly Taikyokusya[] itiran = {
            Taikyokusya.T1,
            Taikyokusya.T2
            };
        public static readonly string[] namaeItiran =
        {
            "対局者１",
            "対局者２"
        };

        /// <summary>
        /// 対局者反転
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static Taikyokusya Hanten(Taikyokusya ts)
        {
            switch (ts)
            {
                case Taikyokusya.T1: return Taikyokusya.T2;
                case Taikyokusya.T2: return Taikyokusya.T1;
                default: return ts;
            }
        }

        /// <summary>
        /// 対局者1 なら 1、
        /// 対局者2 なら 2、
        /// それ以外なら -1 （エラー）を返すものとするぜ☆（＾▽＾）
        /// </summary>
        public readonly static string[] m_dfen_ = { "1", "2", "-1" };
        public readonly static string[] m_sfen_ = { "b", "w", "x" };

        public static bool IsOk(Taikyokusya ts)
        {
            return Taikyokusya.T1 <= ts && ts < Taikyokusya.Yososu;
        }
    }

    /// <summary>
    /// 人称だぜ☆（＾▽＾）
    /// </summary>
    public enum Ninsyo
    {
        /// <summary>
        /// わたし（手番）
        /// </summary>
        Watasi,

        /// <summary>
        /// あなた（相手番）
        /// </summary>
        Anata,

        /// <summary>
        /// 要素数。該当なしのフラグとしても利用可能
        /// </summary>
        Yososu
    }

    public abstract class Conv_Ninsyo
    {
        /// <summary>
        /// 手番一覧
        /// </summary>
        public static readonly Ninsyo[] itiran = {
            Ninsyo.Watasi,
            Ninsyo.Anata
            };

        /// <summary>
        /// 手番反転
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static Ninsyo Hanten(Ninsyo tb)
        {
            switch (tb)
            {
                case Ninsyo.Watasi: return Ninsyo.Anata;
                case Ninsyo.Anata: return Ninsyo.Watasi;
                default: return tb;
            }
        }

        /// <summary>
        /// 手番 なら friend、
        /// 相手番 なら opponent、
        /// それ以外なら 空文字列 を返すものとするぜ☆（＾▽＾）
        /// </summary>
        public readonly static string[] m_toMojiretu_ = { "friend", "opponent", "" };

        public static bool IsOk(Ninsyo tb)
        {
            return Ninsyo.Watasi <= tb && tb <= Ninsyo.Anata;
        }
    }

}
