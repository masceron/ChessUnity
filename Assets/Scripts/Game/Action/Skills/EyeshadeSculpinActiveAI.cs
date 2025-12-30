
using System;
using Game.Action.Internal;
using Game.Action.Internal.Pending;
using Game.AI;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class EyeshadeSculpinActiveAI : Action, ISkills, IAIAction
    {
        private PieceLogic firstTarget;
        private PieceLogic secondTarget;
        
        public EyeshadeSculpinActiveAI(int maker, PieceLogic _firstTarget, PieceLogic _secondTarget) : base(maker)
        {
            Maker = (ushort)maker;
            firstTarget = _firstTarget;
            secondTarget = _secondTarget;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, firstTarget)));
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, secondTarget)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }
        
    }
}