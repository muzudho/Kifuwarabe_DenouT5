#if DEBUG
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.logger;
#else
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.logger;
#endif

namespace kifuwarabe_shogithink.pure.project
{
    public class PureProject
    {
        /// <summary>
        /// オワタ☆（＾▽＾）　強制終了の準備だぜ☆（＾～＾）
        /// </summary>
        /// <param name="hyoji"></param>
        public virtual string Owata(string hint, IHyojiMojiretu hyoji)
        {
            return hint + " " + hyoji.ToContents();
        }
        public virtual string Owata(string hint)
        {
            return hint;
        }

        public virtual void HyojiKikiItiran(IHyojiMojiretu hyoji)
        {
            hyoji.AppendLine("未実装");
        }
        public virtual void SnapshotTansaku(IHyojiMojiretu hyoji)
        {
            hyoji.AppendLine("未実装");
        }
        public virtual void HyojiIbasho(string header, IHyojiMojiretu hyoji)
        {
            hyoji.AppendLine(string.Format("未実装 header={0}", header));
        }
        public virtual void HyojiKyokumen(int teme, IHyojiMojiretu hyoji)
        {
            hyoji.AppendLine(string.Format("未実装 teme={0}", teme));
        }
        public virtual void HyojiBitboard(string header, Bitboard bb, IHyojiMojiretu hyoji)
        {
            // 仮実装
            hyoji.Append(HyojiBitboard(header, bb));
        }
        public virtual string HyojiBitboard(string header, Bitboard bb)
        {
            // 仮実装
            return string.Format("{0} bbB={1} bbA={2}", header, bb.value64127, bb.value063);
        }
        /// <summary>
        /// なんでもかんでも出力させたいとき
        /// </summary>
        /// <param name="dbg_hint"></param>
        /// <returns></returns>
        public virtual string Dump()
        {
            return "未実装";
        }

#if DEBUG
        public virtual void Dbg_TryRule1(Bitboard kikiBB, Bitboard trySakiBB)
        {
        }
        public virtual void Dbg_TryRule2(Bitboard spaceBB, Bitboard trySakiBB)
        {
        }
        public virtual void Dbg_TryRule3(Bitboard safeBB, Bitboard trySakiBB)
        {
        }
#endif
    }
}
