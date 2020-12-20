#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.accessor;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.conv.genkyoku.play;
using kifuwarabe_shogithink.pure.genkyoku;
using kifuwarabe_shogithink.pure.ikkyoku;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;

using kifuwarabe_shogithink.pure.move;
using kifuwarabe_shogithink.pure.speak.genkyoku;
using kifuwarabe_shogiwin.consolegame.console.command;
using kifuwarabe_shogiwin.consolegame.machine;
using System;
using kifuwarabe_shogithink.pure.listen.genkyoku;
using System.Collections.Generic;
using kifuwarabe_shogithink.pure.control;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.com;
using kifuwarabe_shogithink.pure.ky;
using kifuwarabe_shogithink.pure.listen;
using kifuwarabe_shogithink.pure.listen.play;
using kifuwarabe_shogithink.pure.move;
#endif

namespace kifuwarabe_shogiwin.consolegame.console
{
    public static class Console02
    {

        #region コンソールゲーム用の機能☆
        /// <summary>
        /// 人間の番☆
        /// </summary>
        /// <returns></returns>
        public static bool IsNingenNoBan()
        {
            return (PureMemory.kifu_teban == Taikyokusya.T1 && !PureSettei.p1Com) // コンピューターでない場合
                    ||
                    (PureMemory.kifu_teban == Taikyokusya.T2 && !PureSettei.p2Com) // コンピューターでない場合
                    ;
        }
        /// <summary>
        /// コンピューターの番☆
        /// </summary>
        /// <returns></returns>
        public static bool IsComputerNoBan()
        {
            return (PureMemory.kifu_teban == Taikyokusya.T1 && PureSettei.p1Com) // 対局者１でコンピューター☆
                        ||
                        (PureMemory.kifu_teban == Taikyokusya.T2 && PureSettei.p2Com) // 対局者２でコンピューター☆
                        ;
        }



        /// <summary>
        /// 定跡等外部ファイルの保存間隔の調整だぜ☆　もう保存していいなら真だぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsOk_SavefileTimeSpan()
        {
            return ComSettei.timeManager.IsTimeOver_Savefile();
        }
        /// <summary>
        /// 保存間隔の調整だぜ☆　保存が終わったら呼び出せだぜ☆（＾▽＾）
        /// </summary>
        public static void Restart_SavefileTimeSpan()
        {
            ComSettei.timeManager.RestartStopwatch_Savefile();
        }

        /// <summary>
        /// 連続対局時のルール変更間隔の調整だぜ☆　もう変更していいなら真だぜ☆（＾▽＾）
        /// 
        /// ルールを変更するときに必要となる、ファイルの読み書き時間を回避するためのものだぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsTimeOver_RenzokuRandomRule()
        {
            return ComSettei.timeManager.IsTimeOver_RenzokuRandomRule();
        }
        /// <summary>
        /// 変更間隔の調整だぜ☆　変更が終わったら呼び出せだぜ☆（＾▽＾）
        /// </summary>
        public static void Restart_RenzokuRandomRuleTimeSpan()
        {
            ComSettei.timeManager.RestartStopwatch_RenzokuRandomRule();
        }
        #endregion


    }
}
