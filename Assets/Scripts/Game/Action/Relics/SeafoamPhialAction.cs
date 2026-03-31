using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class SeafoamPhialAction : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private SeafoamPhialAction()
        {
        }

        public SeafoamPhialAction(PieceLogic target) : base(null, target)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new Purify(null, GetTargetAsPiece()));
            ActionManager.EnqueueAction(new ApplyEffect(new Haste(3, 1, GetTargetAsPiece())));
        }
    }
}