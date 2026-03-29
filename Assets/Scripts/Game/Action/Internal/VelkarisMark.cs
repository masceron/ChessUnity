using Game.Effects.Others;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMark : Action, IInternal
    {
        public VelkarisMark(int f, int t) : base(f, t)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new VelkarisMarked(GetTarget()), GetMaker() as PieceLogic));

            SetCooldown(GetMaker() as PieceLogic, 0);
            ((Velkaris)GetMaker() as PieceLogic).Marked = GetTarget();
        }
    }
}