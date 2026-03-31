using Game.Action.Internal;
using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class EyeOfMimicExecute : Action, IRelicAction
    {
        [MemoryPackConstructor]
        private EyeOfMimicExecute()
        {
        }

        public EyeOfMimicExecute(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        protected override void ModifyGameState()
        {
            // apply 1 turn nhưng vì ApplyEffect tự động ++duration nên ở đây để là 0
            ActionManager.EnqueueAction(new ApplyEffect(new CopyCapturesMethod(GetFrom(), GetTargetPos(), 0)));
        }
    }
}