namespace kifuwarabe_shogithink.pure.logger
{
    public interface ICommandMojiretu
    {
        bool IsHataraku { get; }

        void Append(string val);
        void Append(int val);
        void Append(uint val);
        void Append(long val);
        void Append(ulong val);
        void Append(float val);
        void Append(double val);
        void Append(bool val);

        void AppendLine(string val);
        void AppendLine(int val);
        void AppendLine(uint val);
        void AppendLine(long val);
        void AppendLine(ulong val);
        void AppendLine(float val);
        void AppendLine(double val);
        void AppendLine(bool val);

        void AppendLine();

        void Clear();

        int Length { get; }

        void Insert(int nanmojime, string mojiretu);

        string ToContents();
    }

    /// <summary>
    /// 表示、ログ等に使う文字列。
    /// 将棋エンジンとしては出力しない文字列等
    /// </summary>
    public interface IHyojiMojiretu : ICommandMojiretu
    {
    }

#if DEBUG
    /// <summary>
    /// デバッグ・モード時のみ出力するメッセージだぜ☆（＾▽＾）
    /// </summary>
    public interface IDebugMojiretu : IHyojiMojiretu
    {
    }
#endif

    public interface INewString :
#if DEBUG
        IDebugMojiretu
#else
        IHyojiMojiretu
#endif
    {
    }

}
