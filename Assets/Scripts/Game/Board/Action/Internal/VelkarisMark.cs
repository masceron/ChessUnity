using Game.Board.Effects.Others;
using Game.Board.General;
using Game.Board.Piece.PieceLogic.Commanders;

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
            ActionManager.EnqueueAction(new ApplyEffect(new VelkarisMarked(MatchManager.Ins.GameState.PieceBoard[To])));
        }

        protected override void ModifyGameState()
        {
            var caller = (Velkaris)MatchManager.Ins.GameState.PieceBoard[From];
            caller.SkillCooldown = 0;
            caller.Marked = MatchManager.Ins.GameState.PieceBoard[To];
        }
    }
}