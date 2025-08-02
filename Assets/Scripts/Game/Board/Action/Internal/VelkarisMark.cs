using Game.Board.Effects.Others;
using Game.Board.Piece.PieceLogic.Commanders;
using static Game.Common.BoardUtils;

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
            ActionManager.EnqueueAction(new ApplyEffect(new VelkarisMarked(PieceOn(To))));
        }

        protected override void ModifyGameState()
        {
            SetCooldown(Caller, 0);
            ((Velkaris)PieceOn(Caller)).Marked = PieceOn(To);
        }
    }
}