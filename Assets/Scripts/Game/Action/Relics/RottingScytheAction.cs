using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class RottingScytheAction : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private RottingScytheAction()
        {
        }

        public RottingScytheAction(int maker) : base(null, maker)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(GetTargetAsPiece()));
        }
    }
}