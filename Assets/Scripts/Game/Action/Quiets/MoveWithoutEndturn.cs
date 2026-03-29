using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Quiets
{
    /// <summary>
    ///     Move action dùng cho quân có State <see cref="Game.Effects.States.PieceStateType.Cooperative"/>.
    ///     Implement <see cref="IDontEndTurn"/> để action này không kết thúc turn của người chơi.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class MoveWithoutEndturn : Action, IDontEndTurn, IQuiets
    {
        [MemoryPackConstructor]
        private MoveWithoutEndturn()
        {
        }

        public MoveWithoutEndturn(PieceLogic maker, int target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
            PieceManager.Ins.Move(GetFrom(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            BoardUtils.Move(GetMaker() as PieceLogic, GetTargetPos());
        }
    }
}
