using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class SirenHarpoonExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private SirenHarpoonExecute()
        {
        }

        public SirenHarpoonExecute(PieceLogic target) : base(null, target)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Controlled(-1, GetTargetAsPiece())));
            ActionManager.EnqueueAction(new ApplyEffect(new Pacified(1, GetTargetAsPiece())));
        }
    }
}