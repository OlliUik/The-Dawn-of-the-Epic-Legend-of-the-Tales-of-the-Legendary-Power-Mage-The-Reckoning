using System.Collections.Generic;

namespace PowerMage
{
    public static class Teams
    {
        public enum Team
        {
            NONE,
            GOODGUYS,
            BADBOYS
        };

        public static bool IsEnemy(Entity caller, Entity target)
        {
            switch (target.team)
            {
                case Team.GOODGUYS:
                    {
                        if (target.team == Team.BADBOYS)
                        {
                            return true;
                        }
                        return false;
                    }
                case Team.BADBOYS:
                    {
                        if (target.team == Team.GOODGUYS)
                        {
                            return true;
                        }
                        return false;
                    }
                default: return false;
            }
        }
    }
}
