#if DEBUG
using kifuwarabe_shogithink.pure.logger;
using System.Diagnostics;

namespace kifuwarabe_shogithink.pure.project
{
    public delegate void TestBlock(IHyojiMojiretu hyoji);

    public abstract class Util_Test
    {
        public static bool TestMode { get; set; }

        [Conditional("DEBUG")]
        public static void TestCode(TestBlock testBlock)
        {
            if (Util_Test.TestMode)
            {
                testBlock(PureAppli.syuturyoku1);
            }
        }

        [Conditional("DEBUG")]
        public static void Append(string line, IHyojiMojiretu hyoji)
        {
            if (Util_Test.TestMode)
            {
                hyoji.Append(line);
            }
        }
        [Conditional("DEBUG")]
        public static void AppendLine(string line, IHyojiMojiretu hyoji)
        {
            if (Util_Test.TestMode)
            {
                hyoji.AppendLine(line);
            }
        }
        ///// <summary>
        ///// バッファーに溜まっているログを吐き出します。
        ///// </summary>
        //[Conditional("DEBUG")]
        //public static void Flush(IViewMojiretu syuturyoku)
        //{
        //    if (Util_Test.TestMode)
        //    {
        //        Util_Machine.Flush(syuturyoku);
        //    }
        //}
    }
}
#endif
