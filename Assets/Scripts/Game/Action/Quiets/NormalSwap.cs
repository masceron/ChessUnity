using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class NormalSwap : Action, IQuiets
    {
        [MemoryPackConstructor]
        private NormalSwap()
        {
        }

        public NormalSwap(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Swap(GetFrom(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Swap(GetMakerAsPiece(), GetTargetAsPiece());
        }
    }
}