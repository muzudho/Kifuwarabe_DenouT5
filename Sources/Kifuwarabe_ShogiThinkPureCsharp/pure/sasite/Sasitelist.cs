using kifuwarabe_shogithink.pure.com.sasiteorder;
using System;

namespace kifuwarabe_shogithink.pure.sasite
{
    /// <summary>
    /// 指し手のリスト☆（＾▽＾）
    /// 合法手数は 38 が上限のようだが☆（＾～＾）
    /// </summary>
    public class Sasitelist
    {
        public Sasitelist()
        {
            // List<SasiteKakucho> では範囲外インデックスエラーが出るので、配列にしてみるぜ☆
            list_sasite = new Sasite[PureMemory.SAIDAI_SASITE];
            list_sasiteType = new SasiteType[PureMemory.SAIDAI_SASITE];
            listCount = 0;
        }

        /// <summary>
        /// 指し手のリスト
        /// </summary>
        public Sasite[] list_sasite { get; set; }
        /// <summary>
        /// 指し手のリスト（理由）
        /// </summary>
        public SasiteType[] list_sasiteType { get; set; }

        public int listCount { get; set; }
        public void AddList(Sasite ss, SasiteType ssType)
        {
            try
            {
                list_sasite[listCount] = ss;
                list_sasiteType[listCount] = ssType;
                listCount++;
            }
            catch (Exception )
            {
                throw ;
            }
        }
        public void ClearList()
        {
            try
            {
                Array.Clear(list_sasite, 0, listCount);
                Array.Clear(list_sasiteType, 0, listCount);
                listCount = 0;
            }
            catch (Exception )
            {
                throw ;
            }
        }
    }
}
