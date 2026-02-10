using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Relics
{
    public class DiurnalActive : Action, ISkills
    {
        public DiurnalActive(int maker, int target) : base(maker)
        {
            Target = target;
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }

        protected override void ModifyGameState()
        {
            
            ActionManager.EnqueueAction(new NormalMove(Maker, Target));
        }
    }
}
