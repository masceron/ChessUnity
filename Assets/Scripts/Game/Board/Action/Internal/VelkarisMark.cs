using Game.Board.Effects.Others;
using Game.Board.Piece.PieceLogic.Commanders;
using static Game.Board.General.MatchManager;

namespace Game.Board.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMark: Action, IInternal
    {
        public VelkarisMark(int p, ushort f, ushort t): base(p, false)
        {
            From = f;
            To = t;
        }

        protected override void Animate()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new VelkarisMarked(gameState.MainBoard[To])));
        }

        protected override void ModifyGameState()
        {
            var caller = (Velkaris)gameState.MainBoard[From];
            caller.SkillCooldown = 0;
            caller.Marked = gameState.MainBoard[To];
        }
    }
}