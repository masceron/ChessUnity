using System;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class LongSnoutedSeahorseActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private LongSnoutedSeahorseActive()
        {
        }

        public LongSnoutedSeahorseActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void Animate()
        {
            PieceManager.Ins.Swap(GetMakerAsPiece().Pos, GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Swap(GetMakerAsPiece(), GetTargetAsPiece());
            SetCooldown(GetMakerAsPiece(), ((IPieceWithSkill)GetMakerAsPiece()).TimeToCooldown);
        }
    }
}