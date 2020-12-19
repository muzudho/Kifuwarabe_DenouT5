#if DEBUG
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.logger;
using System;
using System.Diagnostics;
using System.IO;
#else
using kifuwarabe_shogithink.pure;
using kifuwarabe_shogithink.pure.logger;
using Nett;
using System;
using System.Diagnostics;
using System.IO;
#endif


namespace kifuwarabe_shogiwin.consolegame.machine
{
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
            if (File.Exists(Path.Combine( LogDirectory, LogFileStem + LogFileExt)))
            {
                File.Delete(Path.Combine(LogDirectory, LogFileStem + LogFileExt));
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
        /// ログ・フォルダー
        /// </summary>
        /// <summary>
        /// ログ・フォルダー
        /// </summary>
        static string LogDirectory
        {
            get
            {
                if (Util_Machine.logDirectory == null)
                {
                    var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                    var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
                    var logDirectory = toml.Get<TomlTable>("Resources").Get<string>("LogDirectory");
                    Util_Machine.logDirectory = Path.Combine(profilePath, logDirectory);
                }

                return Util_Machine.logDirectory;
            }
        }
        static string logDirectory;

        /// <summary>
        /// ログ・ファイル名　拡張子抜き（without extension）
        /// </summary>
        const string LogFileStem = "_auto_log";
        /// <summary>
        /// ログ・ファイル名の拡張子
        /// </summary>
        const string LogFileExt = ".txt";

        /// <summary>
        /// ローカルルール名
        /// ファイル名に付けておかないと、ゴミ分割ファイル削除時に、巻き込まれて削除されてしまうぜ☆（＾～＾）
        /// </summary>
        public const string LOCALRULE_HONSHOGI = "_Honshogi";
        public const string LOCALRULE_SAGARERUHIYOKO = "_SagareruHiyoko";

        /// <summary>
        /// この名前のファイルが存在すれば、連続対局をストップさせるぜ☆
        /// </summary>
        public const string RENZOKU_TAIKYOKU_STOP_FILE = "RenzokuTaikyokuStop.txt";

        /// <summary>
        /// ログファイルの最大容量☆
        /// 目安として、64KB 以下なら快適、200KB にもなると遅さが目立つ感じ☆
        /// </summary>
        public const int LOG_FILE_SAIDAI_YORYO = 64 * 1000;// 64 Kilo Byte
        /// <summary>
        /// ログファイル分割数☆
        /// １つのファイルにたくさん書けないのなら、ファイル数を増やせばいいんだぜ☆（＾▽＾）
        /// </summary>
        public const int LOG_FILE_BUNKATUSU = 50;

        public static void Clear()
        {
            System.Console.Clear();
        }
        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        public static void Flush(IHyojiMojiretu hyoji)
        {
            bool echo = true;

            // USIモードの場合、表示しないぜ☆（＾～＾）
            if (PureSettei.usi)
            {
                echo = false;
            }

            Util_Machine.Flush(echo, hyoji);
        }
        public static void Flush_NoEcho(IHyojiMojiretu hyoji)
        {
            Util_Machine.Flush(false, hyoji);
        }
        public static void Flush_USI(IHyojiMojiretu hyoji)
        {
            Util_Machine.Flush(true, hyoji);
        }
        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        public static void Flush(bool echo, IHyojiMojiretu hyoji)
        {
            if (0 < hyoji.Length)
            {
                if (echo)
                {
                    // コンソールに表示
                    System.Console.Out.Write(hyoji.ToContents());
                }

                // ログの書き込み
                // _1 ～ _10 等のファイル名末尾を付けて、ログをローテーションするぜ☆（＾▽＾）
                string bestFile;
                {
                    int maxFileSize = Util_Machine.LOG_FILE_SAIDAI_YORYO;
                    int maxFileCount = Util_Machine.LOG_FILE_BUNKATUSU;
                    long newestFileSize = 0;
                    int oldestFileIndex = -1;
                    DateTime oldestFileTime = DateTime.MaxValue;
                    int newestFileIndex = -1;
                    DateTime newestFileTime = DateTime.MinValue;
                    int noExistsFileIndex = -1;
                    int existFileCount = 0;
                    // まず、ログファイルがあるか、Ｎ個確認するぜ☆（＾▽＾）
                    for (int i = 0; i < maxFileCount; i++)
                    {
                        string file = Path.Combine( LogDirectory, LogFileStem + "_" + (i + 1) + LogFileExt);

                        // ファイルがあるか☆
                        if (File.Exists(file))
                        {
                            FileInfo fi = new FileInfo(file);
                            DateTime fileTime = fi.LastWriteTimeUtc;

                            if (fileTime < oldestFileTime)
                            {
                                oldestFileIndex = i;
                                oldestFileTime = fileTime;
                            }

                            if (newestFileTime < fileTime)
                            {
                                newestFileIndex = i;
                                newestFileTime = fileTime;
                                newestFileSize = fi.Length;
                            }

                            existFileCount++;
                        }
                        else if(-1==noExistsFileIndex)
                        {
                            noExistsFileIndex = i;
                        }
                    }

                    if (existFileCount < 1)
                    {
                        // ログ・ファイルが１つも無ければ、新規作成するぜ☆（＾▽＾）

                        if (!Directory.Exists(LogDirectory))
                        {
                            Directory.CreateDirectory(LogDirectory);
                        }

                        bestFile = Path.Combine(LogDirectory, LogFileStem + "_1" + LogFileExt);

                        FileStream fs = File.Create(bestFile);
                        fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                    }
                    else
                    {
                        // ファイルがある場合は、一番新しいファイルに書き足すぜ☆（＾▽＾）

                        bestFile = Path.Combine(LogDirectory, LogFileStem + "_" + (newestFileIndex+1) + LogFileExt);
                        // 一番新しいファイルのサイズが n バイト を超えている場合は、
                        // 新しいファイルを新規作成するぜ☆（＾▽＾）
                        if (maxFileSize < newestFileSize) // n バイト以上なら
                        {

                            if (maxFileCount <= existFileCount)
                            {
                                // ファイルが全部ある場合は、一番古いファイルを消して、一から書き込むぜ☆
                                bestFile = Path.Combine(LogDirectory, LogFileStem + "_" + (oldestFileIndex+1) + LogFileExt);
                                File.Delete(bestFile);

                                FileStream fs = File.Create(bestFile);
                                fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                            }
                            else
                            {
                                // まだ作っていないファイルを作って、書き込むぜ☆（＾▽＾）
                                bestFile = Path.Combine(LogDirectory, LogFileStem + "_" + (noExistsFileIndex+1) + LogFileExt);

                                FileStream fs = File.Create(bestFile);
                                fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                            }
                        }
                    }
                }

                for (int retry=0; retry<2; retry++)
                {
                    try
                    {
                        File.AppendAllText(bestFile, hyoji.ToContents());
                        break;
                    }
                    catch (Exception )
                    {
                        if (0==retry)
                        {
                            // 書き込みに失敗することもあるぜ☆（＾～＾）
                            // 10秒待機して　再挑戦しようぜ☆（＾▽＾）
                            hyoji.AppendLine("ログ書き込み失敗、10秒待機☆");
                            // フラッシュは、できないぜ☆（＾▽＾）この関数だぜ☆（＾▽＾）ｗｗｗｗ
                            System.Threading.Thread.Sleep(10000);
                        }
                        else
                        {
                            // 無理☆（＾▽＾）ｗｗｗ
                            throw ;
                        }
                    }
                }
                // ログ書き出し、ここまで☆（＾▽＾）
                hyoji.Clear();
            }
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
        public static void Assert(bool condition, string message, IHyojiMojiretu hyoji)
        {
            if (!condition)
            {
                hyoji.Append( message);
                string message2 = hyoji.ToContents();
                Util_Machine.Flush(hyoji);
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
            // TODO: Unityで使うなら、このメソッドの中身は消そうぜ☆（＾▽＾）

            kakushiJikken();
        }

        /// <summary>
        /// デバッグ・モード用診断。
        /// </summary>
        /// <param name="logger"></param>
        [Conditional("DEBUG")]
        public static void Fail(IHyojiMojiretu hyoji)
        {
            string message = hyoji.ToContents();
            Util_Machine.Flush(hyoji);
            Debug.Fail(message);
            throw new System.Exception(message);
        }
    }
}