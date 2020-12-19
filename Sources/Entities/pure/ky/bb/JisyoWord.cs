namespace kifuwarabe_shogithink.pure.ky.bb
{
    /// <summary>
    /// 出典：「第4話 パターンで辞書引きしようぜ☆？」http://seiga.nicovideo.jp/watch/mg254964?track=ct_episode
    /// </summary>
    public static class JisyoWord
    {
        public static Bitboard Extract_NewBB(YomiBitboard yomiBB_base, int cutHead, YomiBitboard yomiBB_mask)
        {
            Bitboard ret = new Bitboard();

            yomiBB_base.CopyTo(ret);

            // 頭を切り詰めます
            ret.LeftShift(cutHead);
            // マスクで抽出します
            yomiBB_mask.MaskTo(ret);
            return ret;
        }

    }
}
