using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.AI;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    public class ThreadPipefishActive : Action, ISkills, IAIAction
    {
        public ThreadPipefishActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new ThreadPipefishEffect(PieceOn(Maker), PieceOn(Target))));
        }

        public void CompleteActionForAI()
        {
            throw new System.NotImplementedException();
        }

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}