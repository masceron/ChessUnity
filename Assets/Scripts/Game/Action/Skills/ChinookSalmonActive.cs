using Game.Action.Internal;
using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class ChinookSalmonActive : Action, ISkills
    {
        public ChinookSalmonActive(int maker, int target) : base(maker, target)
        {
        }
        
        protected override void ModifyGameState()
        {
            var maker = GetMaker();
            ActionManager.EnqueueAction(new ApplyEffect(new ChinookSalmonBeforeDead(maker, GetTargetPos()), maker));
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}