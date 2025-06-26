using Core;
using Unity.IL2CPP.CompilerServices;

namespace Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ActionManager
    {
        public static void Execute(GameState gameState, IAction action)
        {
            var move = action.MakeEncodedMove();
            
            if (gameState.IsLegal(move))
            {
                action.ApplyAction();
            }
        }
    }
}