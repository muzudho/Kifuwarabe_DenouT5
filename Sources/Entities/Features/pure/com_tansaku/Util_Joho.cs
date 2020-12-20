#if DEBUG
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.com.jikan;
using kifuwarabe_shogithink.pure.ky;

using kifuwarabe_shogithink.pure.accessor;
#else
using System.Text;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com.hyoka;
using kifuwarabe_shogithink.pure.com.jikan;
using kifuwarabe_shogithink.pure.ky;

#endif

namespace kifuwarabe_shogithink.pure.com
{
    /// <summary>
    /// 探索打ち切り
    /// </summary>
    public static class Util_Joho
    {
        /// <summary>
        /// 読み筋表示用関数
        /// </summary>
        /// <param name="hyokatiNoTaikyokusya"></param>
        /// <param name="gokei"></param>
        /// <param name="komawari"></param>
        /// <param name="okimari"></param>
        /// <param name="riyu"></param>
        /// <param name="riyuHosoku">理由補足</param>
        /// <param name="fukasa"></param>
        /// <param name="nekkoKaranoFukasa"></param>
        /// <param name="yomisuji"></param>
        /// <param name="hyoji"></param>
        /// <param name="hint"></param>
        public delegate void Dlgt_CreateJoho(
            Taikyokusya hyokatiNoTaikyokusya,
            Hyokati hyokatiUtiwake,
            int fukasa,
            int nekkoKaranoFukasa,

            StringBuilder hyoji
#if DEBUG
            , string hint
#endif
            );

        /// <summary>
        /// 無視用の実装☆（＾～＾）
        /// </summary>
        public static Dlgt_CreateJoho Dlgt_IgnoreJoho = (
            Taikyokusya hyokatiNoTaikyokusya,
            Hyokati hyokatiUtiwake,
            int fukasa,
            int nekkoKaranoFukasa,
            StringBuilder hyoji
#if DEBUG
            , string hint
#endif
            ) =>
        {

        };


        public static void JohoMatome(
            int fukasa,
            Hyokati hyokasuToBack,
            StringBuilder hyoji
#if DEBUG
            , string hint
#endif
            )
        {
            if (Util_TimeManager.CanShowJoho())
            {
                PureAppli.dlgt_CreateJoho(
                    PureMemory.kifu_teban,
                    hyokasuToBack,
                    fukasa + 1,// 深さは 0 になっているので、Tansaku していない状態（＝+1 して）に戻すぜ☆
                    HanpukuSinka.happaenoFukasa,
                    hyoji
#if DEBUG
                    ,hint
#endif
                            );
                Util_TimeManager.DoneShowJoho();
            }

        }

    }
}
