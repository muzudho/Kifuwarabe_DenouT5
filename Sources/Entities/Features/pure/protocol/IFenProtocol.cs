namespace kifuwarabe_shogithink.pure.protocol
{
    interface IFenProtocol
    {
        string fen { get; }

        /// <summary>
        /// 平手初期局面
        /// </summary>
        string startpos { get; }

        /// <summary>
        /// 対局者１の駒、または打の表記。
        /// </summary>
        string motigomaT1 { get; }
        string motigomaT2 { get; }

        /// <summary>
        /// 盤上の駒
        /// </summary>
        string banjoT1 { get; }
        string banjoT2 { get; }

        string suji { get; }
        string dan { get; }

        /// <summary>
        /// 改造FEN の盤上句。
        /// fen krz/1h1/1H1/ZRK - 1
        /// みたいなやつと、
        /// startpos
        /// みたいなやつがある。
        /// </summary>
        string position { get; }
        /// <summary>
        /// 改造FEN の持ち駒句。
        /// </summary>
        string motigomaPos { get; }
        string motigomaNasi { get; }

        /// <summary>
        /// 手番。数字は改造FEN。b,wはSFEN。
        /// </summary>
        string tebanPos { get; }

        /// <summary>
        /// 投了
        /// </summary>
        string toryo { get; }
    }
}
