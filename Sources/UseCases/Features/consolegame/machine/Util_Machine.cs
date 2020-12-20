namespace kifuwarabe_shogiwin.consolegame.machine
{
    using Grayscale.Kifuwarabi.Entities.Logging;
#if DEBUG
using kifuwarabe_shogithink.pure;

using System;
using System.Diagnostics;
using System.IO;
#else
    using kifuwarabe_shogithink.pure;
    
    using Nett;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
#endif

    /// <summary>
    /// Unity で使えない関数は、これで包（くる）むぜ☆（＾▽＾）
    /// ここを空っぽ機能にすれば Unity でも動くんじゃないか☆（＾▽＾）
    /// </summary>
    public abstract class Util_Machine
    {
        static Util_Machine()
        {
            System.Console.Title = "きふわらべ";

            // ログ・ファイルがあれば削除するぜ☆
            if (File.Exists(Logger.LogFile))
            {
                File.Delete(Logger.LogFile);
            }

            // 254文字までしか入力できない。（Console.DefaultConsoleBufferSize = 256 引く CRLF 2文字）
            // そこで文字数を拡張する。400手の棋譜を読めるようにしておけば大丈夫か☆（＾～＾）
            // 参照: 「C# Console.ReadLine で長い文字列を入力したい場合」https://teratail.com/questions/19398
            Console.SetIn(new StreamReader(Console.OpenStandardInput(4096)));
        }

        /// <summary>
        /// デバッグ・モードでのみ実行する関数のかたちです。引数の無い隠し実験。
        /// </summary>
        public delegate void Dlgt_KakushiJikken();

        /// <summary>
        /// この名前のファイルが存在すれば、連続対局をストップさせるぜ☆
        /// </summary>
        public const string RENZOKU_TAIKYOKU_STOP_FILE = "RenzokuTaikyokuStop.txt";

        public static void Clear()
        {
            System.Console.Clear();
        }
        public static string ReadLine()
        {
            // コンソールからのキー入力を受け取るぜ☆（＾▽＾）
            return System.Console.In.ReadLine();
        }

        /// <summary>
        /// デバッグ・ウィンドウからの行入力
        /// </summary>
        /// <returns></returns>
        public static void ReadKey()
        {
            System.Console.ReadKey();
        }

        /// <summary>
        /// 定跡、二駒、成績ファイルが 3x4の盤サイズしか対応していないので、
        /// 振り分けるんだぜ☆（＾～＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsEnableBoardSize()
        {
            return PureSettei.banTateHaba == 4 &&
                PureSettei.banYokoHaba == 3;
        }


        /// <summary>
        /// デバッグ・ウィンドウからのキー入力
        /// </summary>
        public static char PushAnyKey()
        {
            System.ConsoleKeyInfo keyInfo = System.Console.ReadKey();
            return keyInfo.KeyChar;
        }

        /// <summary>
        /// 連続対局をストップさせる場合、真☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public static bool IsRenzokuTaikyokuStop()
        {
            // ファイルがあれば、連続対局をストップさせるぜ☆（＾▽＾）
            return File.Exists(Util_Machine.RENZOKU_TAIKYOKU_STOP_FILE);
        }


        /// <summary>
        /// デバッグ・モード用診断。
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message, StringBuilder hyoji)
        {
            if (!condition)
            {
                hyoji.Append(message);
                string message2 = hyoji.ToString();
                Logger.Flush(hyoji);
                Debug.Assert(condition, message2);
            }
        }

        /// <summary>
        /// デバッグ・モードでのみ実行するプログラムを書くものです。
        /// </summary>
        /// <param name="logger"></param>
        [Conditional("DEBUG")]
        public static void DoKakushiJikken(Dlgt_KakushiJikken kakushiJikken)
        {
            kakushiJikken();
        }

        /// <summary>
        /// デバッグ・モード用診断。
        /// </summary>
        /// <param name="logger"></param>
        [Conditional("DEBUG")]
        public static void Fail(StringBuilder hyoji)
        {
            string message = hyoji.ToString();
            Logger.Flush(hyoji);
            Debug.Fail(message);
            throw new System.Exception(message);
        }
    }
}