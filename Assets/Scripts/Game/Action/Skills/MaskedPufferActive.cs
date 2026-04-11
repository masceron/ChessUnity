using System;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class MaskedPufferActive : Action, ISkills
    {
        private const int Strength = 1;
        private const int Duration = 3;
        private const int Strength2 = 1;
        private const int Duration2 = 3;
        private const int Duration3 = 3;
        [MemoryPackConstructor]
        private MaskedPufferActive()
        {
        }

        public MaskedPufferActive(PieceLogic maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(Duration, GetMakerAsPiece())));
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(Duration, Strength, GetMakerAsPiece())));
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(Duration2, Strength2, GetMakerAsPiece())));

            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}