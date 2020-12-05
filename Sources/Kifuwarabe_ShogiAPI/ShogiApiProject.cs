#if DEBUG
using kifuwarabe_shogithink.pure.ky.bb;
using kifuwarabe_shogithink.pure.project;
#else
using kifuwarabe_shogithink.pure.project;
#endif

namespace kifuwarabe_shogiapi
{
    public class ShogiApiProject : PureProject
    {
#if DEBUG
        public override void Dbg_TryRule1(Bitboard kikiBB, Bitboard trySakiBB)
        {
            //未実装
        }
        public override void Dbg_TryRule2(Bitboard spaceBB, Bitboard trySakiBB)
        {
            //未実装
        }
        public override void Dbg_TryRule3(Bitboard safeBB, Bitboard trySakiBB)
        {
            //未実装
        }
#endif
    }
}
