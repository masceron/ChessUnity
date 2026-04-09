using Game.Action.Internal;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Quiets
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class NormalMove : Action, IQuiets
    {
        [MemoryPackConstructor]
        private NormalMove()
        {
        }

        public NormalMove(PieceLogic maker, int target) : base(maker, target)
        {
        }

        protected override void Animate()
        {
            
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new Move(GetMakerAsPiece(), GetTargetPos()));
        }
    }
}