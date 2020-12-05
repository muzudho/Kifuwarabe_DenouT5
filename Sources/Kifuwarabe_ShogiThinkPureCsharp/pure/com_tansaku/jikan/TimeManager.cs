using System.Diagnostics;
using kifuwarabe_shogithink.pure.com;

namespace kifuwarabe_shogithink.pure.com.jikan
{
    /// <summary>
    /// 思考時間を管理したり、
    /// 定跡ファイルの読み書きタイミングを調整したりと、時間周りを一括で担当するぜ☆（＾～＾）
    /// </summary>
    public class TimeManager
    {
        public TimeManager()
        {
            stopwatch_Tansaku = new Stopwatch();
            stopwatch_Savefile = new Stopwatch();
            stopwatch_RenzokuRandomRule = new Stopwatch();
        }

        /// <summary>
        /// ストップウォッチ（探索用）
        /// </summary>
        public Stopwatch stopwatch_Tansaku { get; set; }
        public void RestartStopwatch_Tansaku()
        {
            // Restart()
            stopwatch_Tansaku.Reset();
            stopwatch_Tansaku.Start();
        }
        /// <summary>
        /// ストップウォッチ（定跡読み書き用）
        ///
        /// ・アプリケーション開始とともに　計測スタート。
        /// ・定跡等外部ファイルの保存時、計測スタートから　Ｎミリ秒経過後は、定跡等外部ファイルの保存が可能。（通常、再読み込みは不要）
        /// 　定跡等外部ファイルの保存後に　計測リスタート。
        /// ・アプリケーション終了時に　定跡等外部ファイルを保存すること。（quitコマンドの重要性）
        /// </summary>
        public Stopwatch stopwatch_Savefile { get; set; }
        public void RestartStopwatch_Savefile()
        {
            // Restart()
            stopwatch_Savefile.Reset();
            stopwatch_Savefile.Start();
        }
        /// <summary>
        /// ストップウォッチ（自動対局時、ルール変更用）
        /// </summary>
        public Stopwatch stopwatch_RenzokuRandomRule { get; set; }
        public void RestartStopwatch_RenzokuRandomRule()
        {
            stopwatch_RenzokuRandomRule.Stop();
            stopwatch_RenzokuRandomRule.Start();
        }

        /// <summary>
        /// 前回　読み筋情報　を出力した時間☆（単位はミリ秒）
        /// </summary>
        public long lastJohoTime { get; set; }

        /// <summary>
        /// 探索を打ち切る時間を超えていれば真☆（探索中用）
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOver_TansakuChu()
        {
            bool isTimeOver =
                ComSettei.useTimeOver && // 時間切れ設定がオフなら無制限
                ComSettei.sikoJikan_KonkaiNoTansaku < stopwatch_Tansaku.ElapsedMilliseconds;
            return isTimeOver;
        }

        /// <summary>
        /// 探索を打ち切る時間を超えていれば真☆（反復深化探索用）
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOver_IterationDeeping()
        {
            bool isTimeOver = ComSettei.sikoJikan_KonkaiNoTansaku < stopwatch_Tansaku.ElapsedMilliseconds;
            return isTimeOver;
        }

        /// <summary>
        /// 定跡等外部ファイルの保存間隔の調整だぜ☆　もう保存していいなら真だぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOver_Savefile()
        {
            // （＾～＾）少なくとも１分は間隔を開けるようにするかだぜ☆
            return 60 * 1000 < stopwatch_Savefile.ElapsedMilliseconds;
        }
        /// <summary>
        /// 連続対局時のルール変更間隔の調整だぜ☆　もう変更していいなら真だぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public bool IsTimeOver_RenzokuRandomRule()
        {
            // （＾～＾）少なくとも１分は間隔を開けるようにするかだぜ☆
            return 60 * 1000 < stopwatch_RenzokuRandomRule.ElapsedMilliseconds;
        }
    }
}
