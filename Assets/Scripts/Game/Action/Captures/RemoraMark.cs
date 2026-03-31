using Game.Action.Internal;
using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class RemoraMark : Action, ICaptures
    {
        [MemoryPackConstructor]
        private RemoraMark()
        {
        }

        public RemoraMark(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new RemoraMarked(GetMakerAsPiece(), GetTargetAsPiece()),
                GetMakerAsPiece()));
        }
    }
}