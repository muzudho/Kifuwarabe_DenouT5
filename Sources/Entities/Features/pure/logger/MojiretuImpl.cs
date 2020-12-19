using System;
using System.Text;

namespace kifuwarabe_shogithink.pure.logger
{
    /// <summary>
    /// Unity の C# はバージョンが古いのか、StringBuilderに使えないメソッドがあるのでラッピングしておくぜ☆（＾～＾）
    /// </summary>
    public class MojiretuImpl : INewString
    {
        public MojiretuImpl()
        {
            m_str_ = new StringBuilder();
        }

        StringBuilder m_str_;

        public bool IsHataraku { get { return true; } }

        public void Append(string val) { m_str_.Append(val); }
        public void Append(int val) { m_str_.Append(val); }
        public void Append(uint val) { m_str_.Append(val); }
        public void Append(long val) { m_str_.Append(val); }
        public void Append(ulong val) { m_str_.Append(val); }
        public void Append(float val) { m_str_.Append(val); }
        public void Append(double val) { m_str_.Append(val); }
        public void Append(bool val) { m_str_.Append(val); }

        public void AppendLine(string val) { m_str_.AppendLine(val); }
        public void AppendLine(int val) { m_str_.AppendLine(val.ToString()); }
        public void AppendLine(uint val) { m_str_.AppendLine(val.ToString()); }
        public void AppendLine(long val) { m_str_.AppendLine(val.ToString()); }
        public void AppendLine(ulong val) { m_str_.AppendLine(val.ToString()); }
        public void AppendLine(float val) { m_str_.AppendLine(val.ToString()); }
        public void AppendLine(double val) { m_str_.AppendLine(val.ToString()); }
        public void AppendLine(bool val) { m_str_.AppendLine(val.ToString()); }
        public void AppendLine() { m_str_.AppendLine(); }

        public void Clear()
        {
            m_str_.Length = 0;
            //m_str_.Clear();
        }

        public int Length
        {
            get
            {
                return m_str_.Length;
            }
        }

        public void Insert(int nanmojime, string mojiretu)
        {
            m_str_.Insert( nanmojime, mojiretu);
        }

        public string ToContents()
        {
            return m_str_.ToString();
        }

        public override string ToString()
        {
            throw new Exception("ToString ではなく ToContents を使えだぜ☆（＞＿＜）");
        }
    }

}
