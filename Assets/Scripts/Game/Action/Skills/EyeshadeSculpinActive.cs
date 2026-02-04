using Game.Action.Internal;
using Game.AI;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class EyeshadeSculpinActive : Action, ISkills, IAIAction
    {
        private PieceLogic firstTarget;
        private PieceLogic secondTarget;
        
        public EyeshadeSculpinActive(int maker, PieceLogic _firstTarget, PieceLogic _secondTarget) : base(maker)
        {
            Maker = (ushort)maker;
            firstTarget = _firstTarget;
            secondTarget = _secondTarget;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, firstTarget), PieceOn(Maker)));
            ActionManager.EnqueueAction(new ApplyEffect(new Shortreach(4, 1, secondTarget), PieceOn(Maker)));
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