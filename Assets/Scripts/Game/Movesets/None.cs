using System.Collections.Generic;

namespace Game.Movesets
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class None
    {
        public static void Quiets(List<Action.Action> list, int pos)
        {
        }

        public static void Captures(List<Action.Action> list, int pos)
        {
        }

        public static void Skills(List<Action.Action> list)
        {
            return;
        }
    }
}