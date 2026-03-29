using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class NormalCapture : Action, ICaptures
    {
        [MemoryPackConstructor]
        private NormalCapture()
        {
        }

        public NormalCapture(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
            
            MatchManager.Ins.GameState.Kill(GetTarget());
            MatchManager.Ins.GameState.Move(GetMaker(), GetTargetPos());
        }
    }
}