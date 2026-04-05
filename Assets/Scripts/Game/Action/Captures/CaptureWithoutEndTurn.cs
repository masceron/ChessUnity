using Game.Effects.States;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Captures
{
    /// <summary>
    ///     Capture action dùng cho quân có State <see cref="Cooperative"/>.
    ///     Implement <see cref="IDontEndTurn"/> để action này không kết thúc turn của người chơi.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class CaptureWithoutEndTurn : Action, IDontEndTurn, ICaptures
    {
        [MemoryPackConstructor]
        private CaptureWithoutEndTurn()
        {
        }

        public CaptureWithoutEndTurn(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            PieceManager.Ins.Destroy(GetTargetPos());
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
            
            MatchManager.Ins.GameState.Kill(GetTargetAsPiece());
            MatchManager.Ins.GameState.Move(GetMakerAsPiece(), GetTargetPos());
        }
    }
}
