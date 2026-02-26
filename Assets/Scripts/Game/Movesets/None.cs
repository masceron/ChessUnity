using System.Collections.Generic;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class None
    {
        public static int Quiets(List<Action.Action> list, int pos, bool isPlayer)
        {
            return 0;
        }

        public static int Captures(List<Action.Action> list, int pos, bool isPlayer)
        {
            return 0;
        }

        public static void Skills(List<Action.Action> list, bool isPlayer)
        {
        }
    }
}