using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    public class BlackcapBassletActive : Action, ISkills
    {
        public BlackcapBassletActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }
        
        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new NormalMove(Maker, Target));
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}