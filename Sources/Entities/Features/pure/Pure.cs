#if DEBUG

using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
#else
using System.Text;

using System.Diagnostics;
#endif

namespace kifuwarabe_shogithink.pure
{
    public static class Pure
    {
        public const bool SUCCESSFUL_FALSE = false;
        public static bool FailTrue(string message)
        {
#if DEBUG
            Pure.Sc.AddErr(message);
#endif
            return true;
        }

        /// <summary>
        /// 番号を 1 から始めるために 1 を足したいときに使います
        /// </summary>
        public const int ORIGIN1 = 1;
        /// <summary>
        /// 両端のうち、片側という意味ぐらいの 1
        /// </summary>
        public const int KATAGAWA1 = 1;

        /// <summary>
        /// スタックコールの略☆（＾～＾）
        /// </summary>
        public static class Sc
        {
            static Sc()
            {
#if DEBUG
                stackcall_ = new List<StringBuilder>();
                stackcallDbgMojiretuList = new List<StringBuilder>();
#endif
            }

#if DEBUG
            static List<StringBuilder> stackcall_;
            /// <summary>
            /// エラー文字列等の出力先
            /// </summary>
            static List<StringBuilder> stackcallDbgMojiretuList;
#endif
            /// <summary>
            /// エラーメッセージ等を入れろだぜ☆（＾～＾）
            /// </summary>
            /// <param name="err"></param>
            [Conditional("DEBUG")]
            public static void AddErr(string err)
            {
#if DEBUG
                stackcall_[stackcall_.Count-1].AppendLine(err);
#endif
            }
            [Conditional("DEBUG")]
            public static void Push(string hint)
            {
#if DEBUG
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(hint);
                stackcall_.Add(sb);
                if (0 < stackcallDbgMojiretuList.Count)
                {
                    stackcallDbgMojiretuList.Add(stackcallDbgMojiretuList[stackcallDbgMojiretuList.Count - 1]);
                }
                else
                {
                    stackcallDbgMojiretuList.Add(PureAppli.syuturyoku1);
                }
#endif
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="hint"></param>
            /// <param name="dbgMojiretu">これは、ゲームの出力とは分けた出力だぜ☆（＾～＾）</param>
            [Conditional("DEBUG")]
            public static void Push(string hint, StringBuilder dbgMojiretu)
            {
#if DEBUG
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(hint);
                stackcall_.Add(sb);
                stackcallDbgMojiretuList.Add(dbgMojiretu);
#endif
            }
            [Conditional("DEBUG")]
            public static void Pop()
            {
#if DEBUG
                stackcall_.RemoveAt(stackcall_.Count - 1);
                stackcallDbgMojiretuList.RemoveAt(stackcallDbgMojiretuList.Count - 1);
#endif
            }

            public static string ToContents()
            {
#if DEBUG
                StringBuilder sb = new StringBuilder();
                foreach (StringBuilder line in stackcall_)
                {
                    sb.AppendLine(line.ToString());
                }
                return sb.ToString();
#else
                return "";
#endif
            }
            public static string ToDbgString()
            {
#if DEBUG
                return dbgMojiretu.ToContents();
#else
                return "";
#endif
            }
#if DEBUG
            /// <summary>
            /// これは、ゲームの出力とは分けた出力だぜ☆（＾～＾）
            /// </summary>
            public static StringBuilder dbgMojiretu
            {
                get
                {
                    if (0 < stackcall_.Count)
                    {
                        return stackcallDbgMojiretuList[stackcall_.Count - 1];
                    }
                    else
                    {
#if DEBUG
                        // デバッグ・モードでは、バンバン出力するぜ☆（＾▽＾）
                        return PureAppli.syuturyoku1;
#else
                        // 出力は全部無視するオブジェクトだぜ☆（＾～＾）
                        return PureAppli.karappoSyuturyoku;
#endif
                    }
                }
            }
#endif
                    }

    }
}
