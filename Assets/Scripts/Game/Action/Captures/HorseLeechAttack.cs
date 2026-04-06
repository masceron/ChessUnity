using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Captures
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class HorseLeechAttack : Action, ICaptures
    {
        [MemoryPackConstructor]
        private HorseLeechAttack()
        {
        }

        public HorseLeechAttack(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(4, GetTargetAsPiece()), GetMakerAsPiece()));
            ActionManager.EnqueueAction(new KillPiece(null, GetMakerAsPiece()));
        }
    }
}