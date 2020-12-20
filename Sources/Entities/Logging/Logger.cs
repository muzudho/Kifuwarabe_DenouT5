namespace Grayscale.Kifuwarabi.Entities.Logging
{
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

    public static class Logger
    {
        public static string LogFile
        {
            get
            {
                return Path.Combine(Logger.LogDirectory, Logger.LogFileStem + Logger.LogFileExt);
            }
        }

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
                if (Logger.logDirectory == null)
                {
                    var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
                    var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));
                    var logDirectory = toml.Get<TomlTable>("Resources").Get<string>("LogDirectory");
                    Logger.logDirectory = Path.Combine(profilePath, logDirectory);
                }

                return Logger.logDirectory;
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
        /// ログファイルの最大容量☆
        /// 目安として、64KB 以下なら快適、200KB にもなると遅さが目立つ感じ☆
        /// </summary>
        public const int LOG_FILE_SAIDAI_YORYO = 64 * 1000;// 64 Kilo Byte
        /// <summary>
        /// ログファイル分割数☆
        /// １つのファイルにたくさん書けないのなら、ファイル数を増やせばいいんだぜ☆（＾▽＾）
        /// </summary>
        public const int LOG_FILE_BUNKATUSU = 50;

        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        public static void Flush(StringBuilder hyoji)
        {
            bool echo = true;

            // USIモードの場合、表示しないぜ☆（＾～＾）
            if (PureSettei.usi)
            {
                echo = false;
            }

            Logger.Flush(echo, hyoji);
        }
        public static void Flush_NoEcho(StringBuilder hyoji)
        {
            Logger.Flush(false, hyoji);
        }
        public static void Flush_USI(StringBuilder hyoji)
        {
            Logger.Flush(true, hyoji);
        }
        /// <summary>
        /// バッファーに溜まっているログを吐き出します。
        /// </summary>
        public static void Flush(bool echo, StringBuilder hyoji)
        {
            if (0 < hyoji.Length)
            {
                if (echo)
                {
                    // コンソールに表示
                    System.Console.Out.Write(hyoji.ToString());
                }

                // ログの書き込み
                // _1 ～ _10 等のファイル名末尾を付けて、ログをローテーションするぜ☆（＾▽＾）
                string bestFile;
                {
                    int maxFileSize = Logger.LOG_FILE_SAIDAI_YORYO;
                    int maxFileCount = Logger.LOG_FILE_BUNKATUSU;
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
                        string file = Path.Combine(LogDirectory, $"{LogFileStem}_{ (i + 1) }{LogFileExt}");

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
                        else if (-1 == noExistsFileIndex)
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

                        bestFile = Path.Combine(LogDirectory, $"{LogFileStem}_1{ LogFileExt}");

                        FileStream fs = File.Create(bestFile);
                        fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                    }
                    else
                    {
                        // ファイルがある場合は、一番新しいファイルに書き足すぜ☆（＾▽＾）

                        bestFile = Path.Combine(LogDirectory, $"{LogFileStem}_{ (newestFileIndex + 1) }{ LogFileExt}");
                        // 一番新しいファイルのサイズが n バイト を超えている場合は、
                        // 新しいファイルを新規作成するぜ☆（＾▽＾）
                        if (maxFileSize < newestFileSize) // n バイト以上なら
                        {

                            if (maxFileCount <= existFileCount)
                            {
                                // ファイルが全部ある場合は、一番古いファイルを消して、一から書き込むぜ☆
                                bestFile = Path.Combine(LogDirectory, $"{LogFileStem}_{ (oldestFileIndex + 1) }{ LogFileExt}");
                                File.Delete(bestFile);

                                FileStream fs = File.Create(bestFile);
                                fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                            }
                            else
                            {
                                // まだ作っていないファイルを作って、書き込むぜ☆（＾▽＾）
                                bestFile = Path.Combine(LogDirectory, $"{LogFileStem}_{ (noExistsFileIndex + 1) }{ LogFileExt}");

                                FileStream fs = File.Create(bestFile);
                                fs.Close(); // File.Create したあとは、必ず Close() しないと、ロックがかかったままになる☆（＾▽＾）
                            }
                        }
                    }
                }

                for (int retry = 0; retry < 2; retry++)
                {
                    try
                    {
                        File.AppendAllText(bestFile, hyoji.ToString());
                        break;
                    }
                    catch (Exception)
                    {
                        if (0 == retry)
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
                            throw;
                        }
                    }
                }
                // ログ書き出し、ここまで☆（＾▽＾）
                hyoji.Clear();
            }
        }
    }
}
