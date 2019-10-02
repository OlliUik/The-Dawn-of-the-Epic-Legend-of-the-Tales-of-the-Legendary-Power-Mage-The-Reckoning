using System.Collections.Generic;

namespace PowerMage
{
    public static class Teams
    {
        public static List<Entity> teamGoodGuys = new List<Entity>();
        public static List<Entity> teamBadBoys = new List<Entity>();

        public enum Team
        {
            NONE,
            GOODGUYS,
            BADBOYS
        };

        public static void AddToTeam(Entity e, Team t)
        {
            Modify(e, t, 0);
        }

        public static void RemoveFromTeam(Entity e, Team t)
        {
            Modify(e, t, 1);
        }

        private static void Modify(Entity e, Team t, int action)
        {
            //List of actions:
            //Add:      0
            //Remove:   1

            switch (t)
            {
                case Team.NONE: break;
                case Team.BADBOYS:
                    {
                        if (action == 0) teamBadBoys.Add(e);
                        else teamBadBoys.Remove(e);
                    }
                    break;
                case Team.GOODGUYS:
                    {
                        if (action == 0) teamGoodGuys.Add(e);
                        else teamGoodGuys.Remove(e);
                    }
                    break;
                default:
                    UnityEngine.Debug.LogError(e + " has an invalid team!");
                    break;
            }
        }
    }
}
