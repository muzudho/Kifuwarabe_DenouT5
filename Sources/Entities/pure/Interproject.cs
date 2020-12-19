#if DEBUG
using kifuwarabe_shogithink.pure.project;
#else
using kifuwarabe_shogithink.pure.project;
#endif

namespace kifuwarabe_shogithink.pure
{
    /// <summary>
    /// 外部プロジェクトに処理を書く場所を開放したもの。
    /// 
    /// 「Ｃ＃ピュア」プロジェクトの実装コードを減らすために使うぜ☆（＾～＾）
    /// </summary>
    public static class Interproject
    {
        static Interproject()
        {
            project = new PureProject();
        }

        public static PureProject project;
    }
}
