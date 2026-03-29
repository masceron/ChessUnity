using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Internal
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CarapaceKill : Action, IInternal
    {
        public CarapaceKill(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Kill(GetTarget());
        }
    }
}