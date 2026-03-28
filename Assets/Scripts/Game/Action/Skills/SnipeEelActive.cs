using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SnipeEelActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private SnipeEelActive()
        {
        }

        public SnipeEelActive(int maker, int target) : base(maker, target)
        {
            Target = target;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(3, BoardUtils.GetTarget()),
                BoardUtils.GetMaker()));
        }
    }
}