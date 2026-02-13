using MemoryPack;
using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class MaskedPufferActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private MaskedPufferActive() { }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            throw new System.NotImplementedException();
        }

        public MaskedPufferActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(1, PieceOn(Maker))));
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 3, PieceOn(Maker))));
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(1, 3, PieceOn(Maker))));
        }

    }
}