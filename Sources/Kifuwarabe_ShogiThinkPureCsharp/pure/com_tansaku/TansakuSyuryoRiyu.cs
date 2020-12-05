namespace kifuwarabe_shogithink.pure.com
{
#if DEBUG
    /// <summary>
    /// 1手投了の不具合を探すために☆（＾～＾）
    /// </summary>
    public enum TansakuSyuryoRiyu
    {
        /// <summary>
        /// 開始
        /// </summary>
        Kaisi,
        /// <summary>
        /// 自分のらいおんがいない局面の場合、投了☆
        /// </summary>
        JibunRaionInai,
        /// <summary>
        /// 一手も読めなかった。
        /// </summary>
        ItteMoYomenakatta,
        /// <summary>
        /// 「－２手詰められ」が返ってきているなら、負けました、をいう場面だぜ☆
        /// </summary>
        Minus2TeTumerare,
        /// <summary>
        /// 反復深化使わない場合
        /// </summary>
        HanpukuSinkaTukawanai,
    }
#endif
}
