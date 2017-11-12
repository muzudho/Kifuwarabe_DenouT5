using kifuwarabe_shogithink.pure.com;

namespace kifuwarabe_shogithink.pure.com.jikan
{
    public abstract class Util_TimeManager
    {
        public static bool IsEnableShowJoho()
        {
            return 0 <= ComSettei.johoJikan; // 正の数で;
        }

        public static bool CanShowJoho()
        {
            return
                IsEnableShowJoho()
                &&
                ComSettei.timeManager.lastJohoTime + ComSettei.johoJikan <=
                ComSettei.timeManager.stopwatch_Tansaku.ElapsedMilliseconds
                ;
        }

        public static void DoneShowJoho()
        {
            ComSettei.timeManager.lastJohoTime = ComSettei.timeManager.stopwatch_Tansaku.ElapsedMilliseconds;
        }
    }
}
