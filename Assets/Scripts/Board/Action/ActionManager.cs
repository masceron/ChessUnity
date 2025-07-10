using Core.General;
using Unity.IL2CPP.CompilerServices;

namespace Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ActionManager
    {
        private static GameState _state;
        private static MatchManager _manager;

        public static void Init(GameState g, MatchManager m)
        {
            _state = g;
            _manager = m;
        }
        public static void Execute(Action action)
        {
            if (action.DoesMoveCapture)
            {
                if (!_manager.Roll(25))
                {
                    action.Success = false;
                }
            }
            action.ApplyAction(_state);
        }
    }
}