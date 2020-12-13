using System;

namespace kifuwarabe_shogithink.pure.move
{
    /// <summary>
    /// 指し手のリスト☆（＾▽＾）
    /// 合法手数は 38 が上限のようだが☆（＾～＾）
    /// </summary>
    public class MoveList
    {
        public MoveList()
        {
            // List<MoveKakucho> では範囲外インデックスエラーが出るので、配列にしてみるぜ☆
            moveList = new Move[PureMemory.MaxMove];
            moveTypeList = new MoveType[PureMemory.MaxMove];
            listCount = 0;
        }

        /// <summary>
        /// 指し手のリスト
        /// </summary>
        public Move[] moveList { get; set; }
        /// <summary>
        /// 指し手のリスト（理由）
        /// </summary>
        public MoveType[] moveTypeList { get; set; }

        public int listCount { get; set; }
        public void AddList(Move ss, MoveType ssType)
        {
            try
            {
                moveList[listCount] = ss;
                moveTypeList[listCount] = ssType;
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
                Array.Clear(moveList, 0, listCount);
                Array.Clear(moveTypeList, 0, listCount);
                listCount = 0;
            }
            catch (Exception )
            {
                throw ;
            }
        }
    }
}
