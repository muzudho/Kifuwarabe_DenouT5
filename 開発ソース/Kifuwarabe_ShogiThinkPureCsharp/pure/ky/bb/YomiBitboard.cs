namespace kifuwarabe_shogithink.pure.ky.bb
{
    /// <summary>
    /// 遅くなるので、探索部では、あまり使わない
    /// </summary>
    public class YomiBitboard
    {
        public YomiBitboard(Bitboard hontai)
        {
            hontai_ = hontai;
        }
        Bitboard hontai_;

        /// <summary>
        /// 指定升のビットが立っていれば真
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool IsOn(Masu ms)
        {
            return hontai_.IsOn(ms);
        }

        /// <summary>
        /// コピーします
        /// </summary>
        /// <param name="bb"></param>
        public void CopyTo(Bitboard bb)
        {
            bb.Set(this.hontai_);
        }
        /// <summary>
        /// AND算します
        /// </summary>
        public void MaskTo(Bitboard bb)
        {
            bb.Siborikomi(this.hontai_);
        }
    }
}
