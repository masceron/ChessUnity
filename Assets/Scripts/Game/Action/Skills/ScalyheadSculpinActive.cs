using System;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class ScalyheadSculpinActive : Action, ISkills
    {
        private const int CarapaceDuration = 4;

        [MemoryPackConstructor]
        private ScalyheadSculpinActive()
        {
        }

        public ScalyheadSculpinActive(PieceLogic maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(CarapaceDuration, GetTargetAsPiece())));
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}