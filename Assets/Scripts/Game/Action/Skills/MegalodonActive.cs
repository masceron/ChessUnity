using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class MegalodonActive : Action, ISkills
    {
        private int firstTargetPos;
        private int secondTargetPos;
        
        public MegalodonActive(int maker, int firstTarget, int secondTarget) : base(maker)
        {
            Maker = (ushort)maker;
            firstTargetPos = firstTarget;
            secondTargetPos = secondTarget;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new KillPiece(firstTargetPos));
            ActionManager.EnqueueAction(new KillPiece(secondTargetPos));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}