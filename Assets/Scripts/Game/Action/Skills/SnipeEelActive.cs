using MemoryPack;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class SnipeEelActive : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        public SnipeEelActive(int maker, int target) : base(maker)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(3, BoardUtils.PieceOn(Target)), BoardUtils.PieceOn(Maker)));
        }
    }
}