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

        public LongSnoutedSeahorseActive(int maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new NotImplementedException();
        }

        protected override void Animate()
        {
            PieceManager.Ins.Swap(GetMakerPos(), GetTargetPos());
        }

        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Swap(GetMaker() as PieceLogic, GetTarget());
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker() as PieceLogic).TimeToCooldown);
        }
    }
}