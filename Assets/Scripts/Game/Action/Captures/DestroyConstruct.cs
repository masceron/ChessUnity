using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class DestroyConstruct : Action, ICaptures
    {
        [MemoryPackConstructor]
        private DestroyConstruct()
        {
        }

        public DestroyConstruct(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Kill(GetMaker() as PieceLogic);
            MatchManager.Ins.GameState.Kill(GetTarget() as PieceLogic);
        }
    }
}