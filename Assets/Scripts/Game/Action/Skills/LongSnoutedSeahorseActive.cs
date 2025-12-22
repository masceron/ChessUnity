using System;
using System.Collections.Generic;
using Game.AI;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class LongSnoutedSeahorseActive : Action, ISkills, IAIAction
    {
        public LongSnoutedSeahorseActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void Animate()
        {
            PieceManager.Ins.Swap(Maker, Target);
        }
        protected override void ModifyGameState()
        {
            MatchManager.Ins.GameState.Swap(Maker, Target);
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }

        public void CompleteActionForAI()
        {
            throw new NotImplementedException();
        }
    }
}