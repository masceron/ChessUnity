using Game.Action.Internal;
using static Game.Common.BoardUtils;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Skills
{
    public class ThreadPipefishActive : Action, ISkills
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

        public int AIPenaltyValue(PieceLogic maker)
        {
            throw new System.NotImplementedException();
        }
    }
}