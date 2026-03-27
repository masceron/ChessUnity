using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class BlackcapBassletActive : Action, ISkills
    {
        public BlackcapBassletActive(int maker, int target) : base(maker, target)
        {
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(Maker, Target));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}