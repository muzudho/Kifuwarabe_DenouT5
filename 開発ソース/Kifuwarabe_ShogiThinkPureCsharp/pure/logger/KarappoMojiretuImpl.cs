using System;


namespace kifuwarabe_shogithink.pure.logger
{
    /// <summary>
    /// 仕事をしないクラスだぜ☆（＾▽＾）
    /// </summary>
    public class KarappoMojiretuImpl : INewString
    {
        public KarappoMojiretuImpl()
        {
        }

        public bool IsHataraku { get { return false; } }

        public void Append(string val) { }
        public void Append(int val) { }
        public void Append(uint val) { }
        public void Append(long val) { }
        public void Append(ulong val) { }
        public void Append(float val) { }
        public void Append(double val) { }
        public void Append(bool val) { }

        public void AppendLine(string val) { }
        public void AppendLine(int val) { }
        public void AppendLine(uint val) { }
        public void AppendLine(long val) { }
        public void AppendLine(ulong val) { }
        public void AppendLine(float val) { }
        public void AppendLine(double val) { }
        public void AppendLine(bool val) { }
        public void AppendLine() { }

        public void Clear() { }

        public int Length { get { return 0; } }

        public void Insert(int nanmojime, string mojiretu) { }

        public string ToContents() { return ""; }

        public override string ToString()
        {
            throw new Exception("ToString ではなく ToContents を使えだぜ☆（＞＿＜）");
        }
    }
}
