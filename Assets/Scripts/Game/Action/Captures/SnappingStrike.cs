using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SnappingStrike : Action, ICaptures
    {
        [MemoryPackConstructor]
        private SnappingStrike()
        {
        }

        public SnappingStrike(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
            MatchManager.Ins.GameState.Kill(GetTarget() as PieceLogic);
        }
    }
}