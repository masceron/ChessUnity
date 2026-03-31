using Game.Action.Internal;
using Game.Effects.Others;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    public class ChinookSalmonActive : Action, ISkills
    {
        public ChinookSalmonActive(PieceLogic maker, int target) : base(maker, target)
        {
        }
        
        protected override void ModifyGameState()
        {
            var maker = GetMakerAsPiece();
            ActionManager.EnqueueAction(new ApplyEffect(new ChinookSalmonBeforeDead(maker, GetTargetPos()), maker));
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}