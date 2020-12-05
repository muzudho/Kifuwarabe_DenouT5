#if DEBUG
using kifuwarabe_shogithink.pure.com.jikan;
using System.Diagnostics;
#else
using kifuwarabe_shogithink.pure.com.jikan;
using System.Diagnostics;
#endif

namespace kifuwarabe_shogithink.pure.com
{
    public static class ComSettei
    {
        static ComSettei()
        {
            johoJikan = 3000;

            timeManager = new TimeManager();

            useTimeOver = true;

            sikoJikan = 500;
            sikoJikanRandom = 500;

            saidaiFukasa = 1;
        }

        /// <summary>
        /// 読み筋情報 を表示する間隔（単位：ミリ秒）
        /// タイム・マネージャーで使用
        /// </summary>
        public static int johoJikan { get; set; }

        /// <summary>
        /// 時間管理
        /// </summary>
        public static TimeManager timeManager { get; set; }

        /// <summary>
        /// 時間切れの使用有無☆ 主にデバッグのトレース時に使用☆（＾～＾）
        /// </summary>
        public static bool useTimeOver { get; set; }

        #region 思考時間
        /// <summary>
        /// 思考に使っていい時間。単位はミリ秒☆
        /// </summary>
        public static long sikoJikan { get; set; }
        /// <summary>
        /// 思考に使っていい時間に、ランダムに追加される分の最大量☆　単位はミリ秒☆
        /// ランダム関数の制限で int 型☆ 0～（この数字未満）
        /// </summary>
        public static int sikoJikanRandom { get; set; }
        /// <summary>
        /// 今回の探索で、思考に使っていい時間。単位はミリ秒☆（ランダム時間込み）
        /// </summary>
        public static long sikoJikan_KonkaiNoTansaku { get; private set; }
        /// <summary>
        /// 今回の探索で使っていい時間（ランダム時間込み）
        /// </summary>
        /// <returns></returns>
        public static void SetSikoJikan_KonkaiNoTansaku()
        {
            sikoJikan_KonkaiNoTansaku = sikoJikan + PureSettei.random.Next(sikoJikanRandom);

            Debug.Assert(0 < sikoJikan_KonkaiNoTansaku, "思考時間が1ミリ秒も無いぜ☆（＾～＾）！\n" +
                "SikoJikan=" + sikoJikan + "\n" +
                "SikoJikan_KonkaiNoTansaku=" + sikoJikan_KonkaiNoTansaku + "\n" +
                "");
        }
        #endregion

        /// <summary>
        /// 何手まで読むか☆
        /// 0～
        /// </summary>
        public static int saidaiFukasa { get; set; }

        /// <summary>
        /// ひもづき評価を使うなら真
        /// </summary>
        public static bool himodukiHyokaTukau { get; set; }

    }
}
