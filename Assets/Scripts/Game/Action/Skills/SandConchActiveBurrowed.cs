using Game.Action.Internal;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class SandConchActiveBurrowed : Action, ISkills
    {
        public SandConchActiveBurrowed(int maker) : base(maker)
        {
            Maker = maker;
            Target = maker;
        }
        
        protected override void ModifyGameState()
        {
            var maker = PieceOn(Maker);
            ActionManager.EnqueueAction(new ApplyEffect(new Burrowed(4, maker), maker));
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}