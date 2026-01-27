using Game.Effects.Others;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMark: Action, IInternal
    {
        public VelkarisMark(int p, ushort f, ushort t): base(p)
        {
            Maker = f;
            Target = t;
        }

        protected override void Animate()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new VelkarisMarked(PieceOn(Target)), PieceOn(Maker)));
        }

        protected override void ModifyGameState()
        {
            SetCooldown(Maker, 0);
            ((Velkaris)PieceOn(Maker)).Marked = PieceOn(Target);
        }
    }
}