using Core;
using Unity.IL2CPP.CompilerServices;

namespace Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ActionManager
    {
        public static void Execute(GameState gameState, Action action)
        {
            if (!gameState.IsLegal(action.Move)) return;
            gameState.Execute(action.Move);
            action.ApplyAction();
        }
    }
}