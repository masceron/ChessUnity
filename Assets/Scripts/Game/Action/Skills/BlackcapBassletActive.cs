using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class BlackcapBassletActive : Action, ISkills
    {
        public BlackcapBassletActive(PieceLogic maker, int target) : base(maker, target)
        {
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(GetFrom(), GetTargetPos()));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}