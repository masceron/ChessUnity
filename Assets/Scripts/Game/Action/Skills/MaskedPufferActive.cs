using System;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class MaskedPufferActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private MaskedPufferActive()
        {
        }

        public MaskedPufferActive(int maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(1, GetMaker())));
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(1, 3, GetMaker())));
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(1, 3, GetMaker())));
        }
    }
}