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
        private const int carapaceDuration = 4;

        [MemoryPackConstructor]
        private ScalyheadSculpinActive()
        {
        }

        public ScalyheadSculpinActive(int maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Carapace(carapaceDuration, GetTargetAsPiece())));
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}